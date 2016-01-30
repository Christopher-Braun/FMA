using FMA.Contracts;

namespace FMA.UnitTests.Helpers
{
    public static class Extensions
    {
        public static string GetDisplayText(this MaterialField field)
        {
            return field.Uppper ? field.DefaultValue.ToUpper() : field.DefaultValue;
        }
    }
}
