namespace Intersect.Config;

public partial class PartyOptions
{
    /// <summary>
    /// Defines the maximum amount of members a party can have.
    /// </summary>
    public int MaximumMembers{ get; set; } = 4;

    public int SharedXpRange{ get; set; } = 40;

    public int NpcDeathCommonEventStartRange{ get; set; } = 0;

    /// <summary>
    /// It will determine if you will have an XP bonus for each member, Eg 10, will give 10% more to the monster's total XP, then split equally to all members.
    /// A monster that would give 100 XP, now gives 120, getting 60 for each member, when 2 in the party.
    /// </summary>
    public float BonusExperiencePercentPerMember{ get; set; } = 12;
}
