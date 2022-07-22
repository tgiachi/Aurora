using UnitsNet;
using UnitsNet.Units;

namespace Aurora.Api.Utils
{
    public static class SizeUnitConverter
    {
        public static double ConvertLengthUnit(string unit, double size, string toUnit)
        {
            var from = UnitParser.Default.Parse(unit, typeof(LengthUnit));
            var to = UnitParser.Default.Parse(toUnit, typeof(LengthUnit));

            return UnitConverter.Convert(size, from, to);
        }

        public static double ConvertWeightUnit(string unit, double weight, string toUnit)
        {
            var from = UnitParser.Default.Parse(unit, typeof(MassUnit));
            var to = UnitParser.Default.Parse(toUnit, typeof(MassUnit));

            return UnitConverter.Convert(weight, from, to);
        }

        public static double ConvertWeight(this double weight, string sourceUnit, string destUnit)
        {
            return ConvertWeightUnit(sourceUnit, weight, destUnit);
        }

        public static double ConvertLength(this double value, string sourceUnit, string destUnit)
        {
            return ConvertLengthUnit(sourceUnit, value, destUnit);
        }
    }
}
