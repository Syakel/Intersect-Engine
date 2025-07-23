using Intersect.Core;
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

        public static async Task CleanupDeletedSkill(Guid skillId)
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
                    "Cleaned up {Count} user skill entries for deleted skill {SkillId}",
                    userSkillsToDelete.Count,
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
    }
}