using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.Equality
{
    public static class FloatConverter
    {
        public static string ConvertToBinary(string s)
        {
            var array = s.Split('.');
            int exponent;
            var integerPart = ConvertLeftTo24BitsBinary(array[0]);
            var decimalPart = ConvertRightTo24BitsBinary($"0,{array[1]}".PadRight(20, '0'));
            var result = string.Empty;
            if (integerPart.Length != 0)
            {
                var firstOne = 1 + integerPart.TakeWhile(c => c != '1').Count();
                exponent = 127 + (24 - firstOne);
                var exp = ConvertLeftTo8BitsBinary(exponent.ToString());
                result = $"0{exp}{integerPart.Remove(0, firstOne)}{decimalPart}";
                result = result.Substring(0, 32);
            }
            else
            {
                var firstOne = 1 + decimalPart.TakeWhile(c => c != '1').Count();
                exponent = 127 - firstOne;
                var exp = ConvertLeftTo8BitsBinary(exponent.ToString());
                result = $"0{exp}{decimalPart.Remove(0, firstOne)}".Substring(0, 32).PadRight(32, '0'); ;
            }


            // var exp = ConvertLeftTo8BitsBinary(exponent.ToString());
            return result;
            // return $"sign : 0 exp : {exp} integer : {integerPart} decimal : {decimalPart} ";
        }
        public static string ConvertLeftTo24BitsBinary(string s)
        {
            var n = long.Parse(s);
            if (n >= Math.Pow(2, 24) - 1)
            {
                throw new InvalidOperationException();
            }

            var result = string.Empty;


            for (var i = 23; i >= 0; i--)
            {

                if (n >= Math.Pow(2, i))
                {
                    result += "1";
                    n -= (long)Math.Pow(2, i);
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
        public static string ConvertLeftTo8BitsBinary(string s)
        {
            var n = long.Parse(s);
            if (n >= Math.Pow(2, 8) - 1)
            {
                throw new InvalidOperationException();
            }

            var result = string.Empty;

            for (var i = 7; i >= 0; i--)
            {

                if (n >= Math.Pow(2, i))
                {
                    result += "1";
                    n -= (long)Math.Pow(2, i);
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
        public static string ConvertRightTo24BitsBinary(string s)
        {
            var n = double.Parse(s);
            var result = string.Empty;

            for (var i = 1; i <= 24; i++)
            {

                if (n >= Math.Pow(2, -i))
                {
                    result += "1";
                    n -= Math.Pow(2, -i);

                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
    }
}
