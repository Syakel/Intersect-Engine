using Intersect.Core;
using Microsoft.Extensions.Logging;
using NCalc;

namespace Intersect.Server.Utilities;


public partial class ExperienceCurve
{

    public ExperienceCurve()
    {
        BaseExperience = 100;
        Gain = 1.5;
    }

    public long BaseExperience { get; set; }

    public double Gain { get; set; }

    public long Calculate(int level)
    {
        return Calculate(level, BaseExperience, Gain);
    }

    public long Calculate(int level, long baseExperience)
    {
        return Calculate(level, baseExperience, Gain);
    }

    public long Calculate(int level, long baseExperience, double gain)
    {
        return (long) Math.Floor(baseExperience * Math.Pow(gain, level - 1));
    }

}
