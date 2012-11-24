// -----------------------------------------------------------------------
// <copyright file="ConvertToWords.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Menu
{
    /// <summary>
    /// Defines methods for converting numbers to their verbal representation.
    /// </summary>
    public static class ConvertToWords
    {
        /// <summary>
        /// Converts any integer from one to one thousand to its verbal representation.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The verbal representation of the number as a string.</returns>
        public static string ConvertIntToWords(int number)
        {
            string hundred = "hundred";
            string[] otherTens = { string.Empty, string.Empty, "twenty", "thirty", "fourty", "fiftey", "sixtey", "seventy", "eighty", "ninety" };
            string[] teens = { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] singles = { string.Empty, "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            if (number > 999 || number < 1)
            {
                return string.Empty;
            }
            else
            {
                string numberAsWords = string.Empty;
                int leftover = number;
                int hundredCount = leftover / 100;
                leftover = leftover % 100;
                if (hundredCount > 0)
                {
                    numberAsWords += singles[hundredCount] + " " + hundred;
                    if (leftover > 0)
                    {
                        numberAsWords += " and";
                    }
                }

                int tenCount = leftover / 10;
                leftover = leftover % 10;
                if (tenCount == 1)
                {
                    numberAsWords += " " + teens[leftover];
                }
                else if (tenCount != 0)
                {
                    numberAsWords += " " + otherTens[tenCount];
                }

                if (leftover > 0 && tenCount != 1)
                {
                    numberAsWords += " " + singles[leftover];
                }

                return numberAsWords;
            }
        }
    }
}
