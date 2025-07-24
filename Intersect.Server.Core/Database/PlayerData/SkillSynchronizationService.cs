using Intersect.Core;
using Intersect.Framework;
using Intersect.Framework.Core.GameObjects.Skills;
using Intersect.Server.Database.PlayerData.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Intersect.Server.Database.PlayerData.Services
{
    public class SkillSynchronizationService
    {
        public static async Task SyncNewSkillToAllUsers(Guid skillId)
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: false);

            var existingUsers = context.Users.Select(u => u.Id).ToList();

            var userSkills = existingUsers.Select(userId => new UserSkill
            {
                UserId = userId,
                SkillId = skillId,
                SkillLevel = 1,
                SkillXp = 0
            }).ToList();

            context.User_Skills.AddRange(userSkills);
            await context.SaveChangesAsync();
        }

        public static async Task SyncAllSkillsToUser(Guid userId)
        {
            using var gameContext = DbInterface.CreateGameContext(readOnly: true);
            using var playerContext = DbInterface.CreatePlayerContext(readOnly: false);

            // Get all existing skills from the game database  
            var allSkills = await gameContext.Skills.ToListAsync();

            // Create UserSkill entries for this user  
            var userSkills = allSkills.Select(skill => new UserSkill
            {
                UserId = userId,
                SkillId = skill.Id,

                SkillLevel = 1,
                SkillXp = 0
            }).ToList();

            playerContext.User_Skills.AddRange(userSkills);
            await playerContext.SaveChangesAsync();
        }

        public static async Task CleanupDeletedSkill(Guid skillId, string skillName)
        {
            using var playerContext = DbInterface.CreatePlayerContext(readOnly: false);

            // Find all user skills with the deleted skill ID  
            var userSkillsToDelete = await playerContext.User_Skills
                .Where(us => us.SkillId == skillId)
                .ToListAsync();

            if (userSkillsToDelete.Any())
            {
                // Remove all user skill entries for the deleted skill  
                playerContext.User_Skills.RemoveRange(userSkillsToDelete);
                await playerContext.SaveChangesAsync();

                ApplicationContext.Context.Value?.Logger.LogInformation(
                    "Cleaned up {Count} user skill entries for deleted skill '{SkillName}' ({SkillId})",
                    userSkillsToDelete.Count,
                    skillName,
                    skillId
                );
            }
        }

        public static async Task ValidateAllUserSkills()
        {
            using var gameContext = DbInterface.CreateGameContext(readOnly: true);
            using var playerContext = DbInterface.CreatePlayerContext(readOnly: false);

            var allSkills = await gameContext.Skills.ToListAsync();
            var allUsers = await playerContext.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var userSkills = await playerContext.User_Skills
                    .Where(us => us.UserId == user.Id)
                    .Select(us => us.SkillId)
                    .ToListAsync();

                var missingSkills = allSkills
                    .Where(skill => !userSkills.Contains(skill.Id))
                    .ToList();

                if (missingSkills.Any())
                {
                    var newUserSkills = missingSkills.Select(skill => new UserSkill
                    {
                        UserId = user.Id,
                        SkillId = skill.Id,
                        SkillLevel = 1,
                        SkillXp = 0
                    }).ToList();

                    playerContext.User_Skills.AddRange(newUserSkills);
                }
            }

            await playerContext.SaveChangesAsync();
        }

        public static async Task AddSkillExperience(Guid userId, Guid skillId, long expAmount)
        {
            using var playerContext = DbInterface.CreatePlayerContext(readOnly: false);
            using var gameContext = DbInterface.CreateGameContext(readOnly: true);

            var userSkill = await playerContext.User_Skills
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);

            if (userSkill == null) return;

            // Get the skill descriptor for level calculations  
            var skillDescriptor = await gameContext.Skills.FindAsync(skillId);
            if (skillDescriptor == null) return;

            ApplicationContext.Context.Value?.Logger.LogInformation(
                    "Attempting to award {ExpAmount} skill experience to user {UserId} for skill {SkillId}",
                    expAmount, userId, skillId);

            userSkill.SkillXp += expAmount;

            playerContext.Entry(userSkill).State = EntityState.Modified;

            ApplicationContext.Context.Value?.Logger.LogInformation(
                "New skill experience amount: {SkillXp}",
                userSkill.SkillXp);

            // Check for level ups  
            CheckSkillLevelUp(userSkill, skillDescriptor, playerContext);

            await playerContext.SaveChangesAsync();
        }

        private static void CheckSkillLevelUp(UserSkill userSkill, SkillDescriptor skillDescriptor, IPlayerContext context)
        {
            var levelsGained = 0;

            while (userSkill.SkillLevel < skillDescriptor.MaxLevel)
            {
                var expNeeded = skillDescriptor.ExperienceToNextLevel(userSkill.SkillLevel);

                if (userSkill.SkillXp >= expNeeded)
                {
                    userSkill.SkillXp -= expNeeded;
                    userSkill.SkillLevel++;
                    levelsGained++;
                }
                else
                {
                    break;
                }
            }


            if (levelsGained > 0)
            {
                // Log the level up or trigger events  
                ApplicationContext.Context.Value?.Logger.LogInformation(
                    "User {UserId} gained {Levels} levels in {SkillName} (now level {NewLevel})",
                    userSkill.UserId,
                    levelsGained,
                    skillDescriptor.Name,
                    userSkill.SkillLevel
                );
            }
        }

        public static async Task<UserSkillProgress> GetSkillProgress(Guid userId, Guid skillId)
        {
            using var playerContext = DbInterface.CreatePlayerContext(readOnly: true);
            using var gameContext = DbInterface.CreateGameContext(readOnly: true);

            var userSkill = await playerContext.User_Skills
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);

            if (userSkill == null) return null;

            var skillDescriptor = await gameContext.Skills.FindAsync(skillId);
            if (skillDescriptor == null) return null;

            var expToNext = userSkill.SkillLevel < skillDescriptor.MaxLevel
                ? skillDescriptor.ExperienceToNextLevel(userSkill.SkillLevel)
                : 0;

            return new UserSkillProgress
            {
                SkillId = skillId,
                SkillName = skillDescriptor.Name, 
                CurrentLevel = userSkill.SkillLevel,
                CurrentXp = userSkill.SkillXp,
                ExperienceToNextLevel = expToNext,
                MaxLevel = skillDescriptor.MaxLevel,
                IsMaxLevel = userSkill.SkillLevel >= skillDescriptor.MaxLevel
            };
        }

        public class UserSkillProgress
        {
            public Guid SkillId { get; set; }
            public string SkillName { get; set; }
            public int CurrentLevel { get; set; }
            public long CurrentXp { get; set; }
            public long ExperienceToNextLevel { get; set; }
            public int MaxLevel { get; set; }
            public bool IsMaxLevel { get; set; }
        }
    }
}