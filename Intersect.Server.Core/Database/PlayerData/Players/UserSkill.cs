using System.ComponentModel.DataAnnotations;

namespace Intersect.Server.Database.PlayerData.Players
{
    public partial class UserSkill
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid SkillId { get; set; }

        [Range(1, 150)]
        public int SkillLevel { get; set; }

        [Range(0, long.MaxValue)]
        public long SkillXp { get; set; }

        // Navigation property  
        public virtual User User { get; set; }
    }
}