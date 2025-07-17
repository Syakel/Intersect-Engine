using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Intersect.Server.Database;
using Intersect.Server.Database.PlayerData;
using Intersect.Server.Database.PlayerData.Players;
using Intersect.Framework.Core.GameObjects.Skills;

namespace Intersect.Server.Database.PlayerData.Services
{
    public class SkillSynchronizationService
    {
        public static void SyncNewSkillToAllUsers(Guid skillId)//, string skillName)
        {
            using var context = DbInterface.CreatePlayerContext(readOnly: false);

            var existingUsers = context.Users.Select(u => u.Id).ToList();

            var userSkills = existingUsers.Select(userId => new UserSkill
            {
                UserId = userId,
                SkillId = skillId,
                //SkillName = skillName,
                SkillLevel = 1,
                SkillXp = 0
            }).ToList();

            context.User_Skills.AddRange(userSkills);
            context.SaveChanges();
        }
    }
}