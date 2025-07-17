using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [MaxLength(255)]
        public string SkillName { get; set; }

        [Range(1, 150)]
        public int SkillLevel { get; set; } = 1;

        [Range(0, long.MaxValue)]
        public long SkillXp { get; set; } = 0;

        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property  
        public virtual User User { get; set; }
    }
}