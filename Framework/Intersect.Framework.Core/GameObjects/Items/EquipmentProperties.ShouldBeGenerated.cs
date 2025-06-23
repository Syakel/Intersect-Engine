using Intersect.Enums;
using Intersect.GameObjects.Ranges;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace Intersect.Framework.Core.GameObjects.Items;

public partial class EquipmentProperties
{
    [JsonIgnore]
    public ItemRange StatRange_Attack
    {
        get => StatRanges.TryGetValue(Stat.Strength, out var range) ? range : StatRange_Attack = new ItemRange();
        set => StatRanges[Stat.Strength] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_Intelligence
    {
        get =>
            StatRanges.TryGetValue(Stat.Intelligence, out var range) ? range : StatRange_Intelligence = new ItemRange();
        set => StatRanges[Stat.Intelligence] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_Defense
    {
        get => StatRanges.TryGetValue(Stat.Defense, out var range) ? range : StatRange_Defense = new ItemRange();
        set => StatRanges[Stat.Defense] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_MagicResist
    {
        get =>
            StatRanges.TryGetValue(Stat.Faith, out var range) ? range : StatRange_MagicResist = new ItemRange();
        set => StatRanges[Stat.Faith] = value;
    }

    [JsonIgnore]
    public ItemRange StatRange_Speed
    {
        get => StatRanges.TryGetValue(Stat.Agility, out var range) ? range : StatRange_Speed = new ItemRange();
        set => StatRanges[Stat.Agility] = value;
    }
}