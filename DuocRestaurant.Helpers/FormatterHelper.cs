using System;
using System.Collections.Generic;
using System.Text;

namespace DuocRestaurant.Helpers
{
    public class FormatterHelper : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!this.Equals(formatProvider))
                return null;

            if (string.IsNullOrWhiteSpace(format))
                format = "G";

            string customerString = arg.ToString();
            if (customerString.Length < 8)
                customerString = customerString.PadLeft(8, '0');

            switch (format.ToUpper())
            {
                case "G":
                    return customerString.Substring(0, 1) + "-" +
                           customerString.Substring(1, 5) + "-" +
                           customerString.Substring(5, 4) + "-" +
                           customerString.Substring(9);
                case "S":
                    return customerString.Substring(0, 1) + "/" +
                           customerString.Substring(1, 5) + "/" +
                           customerString.Substring(5, 4) + "/" +
                           customerString.Substring(9);
                case "P":
                    return customerString.Substring(0, 1) + "." +
                           customerString.Substring(1, 5) + "." +
                           customerString.Substring(5, 4) + "." +
                           customerString.Substring(9);
                default:
                    throw new FormatException($"The \"{format}\" format specifier is not supported.");
            }
        }
    }
}
