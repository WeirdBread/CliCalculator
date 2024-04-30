using System.ComponentModel;

namespace CliCalculator
{
    public static class Extensions
    {
        public static string GetDescription(this Enum en)
        {
            var field = en.GetType().GetField(en.ToString());
            var attr = field?.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (attr is null)
            {
                return string.Empty;
            }

            return ((DescriptionAttribute)attr).Description;
        }
    }
}
