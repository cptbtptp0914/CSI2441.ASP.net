using System.Collections.Generic;

namespace CSI2441.A2.Web.MVC.Utilities
{

    // TODO: Delete redundant class after hardcoding regex into models.
    public static class RegExDict
    {
        public static readonly Dictionary<string, string> Dictionary;

        static RegExDict()
        {
            Dictionary = new Dictionary<string, string>()
            {
                { "name", "(^[a-zA-Z]+$)"},
                { "studentID", "(^[0-9]{8}$)" },
                { "phone" , "(\\+?\\(?[0-9]{2}\\)?[0-9 ]{10,})" },
                { "postcode", "(^[0-9]{4}$)" },
                { "unitCode", "([A-Z]{3}[0-9]{4})" },
                { "unitCodeSuffix", "([0-9]{4})" },
                { "creditPoints", "(15|20|60)" },
                { "yearSem", "([0-9]{2}[1|2])" },
                { "mark", "(^[0-9]+$)" }
            };
        }

        public static string GetPattern(string word)
        {
            string pattern;
            if (Dictionary.TryGetValue(word, out pattern))
            {
                return pattern;
            }
            else
            {
                return null;
            }
        }
    }
}