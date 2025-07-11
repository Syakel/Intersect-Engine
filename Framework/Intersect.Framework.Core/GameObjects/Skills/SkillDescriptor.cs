using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Models;
using Intersect.Server.Utilities;
using Newtonsoft.Json;

namespace Intersect.Framework.Core.GameObjects.Skills;
public partial class SkillDescriptor : DatabaseObject<SkillDescriptor>, IFolderable
{
    public const long DEFAULT_BASE_EXPERIENCE = 100;

    public const long DEFAULT_EXPERIENCE_INCREASE = 50;
    [NotMapped] public Dictionary<int, long> ExperienceOverrides { get; set; } = [];

    [JsonIgnore]
    private long mBaseExp;

    [JsonIgnore]
    private long mExpIncrease;

    [JsonConstructor]
    public SkillDescriptor(Guid id) : base(id)
    {
        Name = "New Skill";
        ExperienceCurve = new ExperienceCurve();
        ExperienceCurve.Calculate(1);
        BaseExp = DEFAULT_BASE_EXPERIENCE;
        ExpIncrease = DEFAULT_EXPERIENCE_INCREASE;
    }
    public SkillDescriptor()
    {
        Name = "New Skill";
        ExperienceCurve = new ExperienceCurve();
        ExperienceCurve.Calculate(1);
        BaseExp = DEFAULT_BASE_EXPERIENCE;
        ExpIncrease = DEFAULT_EXPERIENCE_INCREASE;
    }
    
    public int MaxLevel { get; set; } = 150;

    public long BaseExp
    {
        get => mBaseExp;
        set
        {
            mBaseExp = Math.Max(0, value);
            ExperienceCurve.BaseExperience = Math.Max(1, mBaseExp);
        }
    }

    public long ExpIncrease
    {
        get => mExpIncrease;
        set
        {
            mExpIncrease = Math.Max(0, value);
            ExperienceCurve.Gain = 1 + value / 100.0;
        }
    }

    //Level Up Info
    public bool IncreasePercentage { get; set; }

    [JsonIgnore]
    [NotMapped]
    public ExperienceCurve ExperienceCurve { get; }

    [JsonIgnore]
    [Column("ExperienceOverrides")]
    public string ExpOverridesJson
    {
        get => JsonConvert.SerializeObject(ExperienceOverrides);
        set
        {
            ExperienceOverrides = JsonConvert.DeserializeObject<Dictionary<int, long>>(value ?? "");
            if (ExperienceOverrides == null)
            {
                ExperienceOverrides = new Dictionary<int, long>();
            }
        }
    }
    public string Folder { get; set; } = string.Empty;

    public long ExperienceToNextLevel(int level)
    {
        if (ExperienceOverrides.ContainsKey(level))
        {
            return ExperienceOverrides[level];
        }

        return ExperienceCurve.Calculate(level);
    }
}
