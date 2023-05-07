using System.Runtime.CompilerServices;

namespace Feipder.Tools.Extensions
{
    public static class DoubleRangeExtansion
    {
        public static bool InRange(this double value, double minValue, double maxValue)
            => value <= maxValue && value >= minValue;
    }
}
