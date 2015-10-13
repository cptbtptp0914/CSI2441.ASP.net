using System.Linq;

namespace A2.University.Web.Models.Business.Services
{

    /// <summary>
    /// Static class generates emails for students and staff.
    /// </summary>
    public static class EmailGenerator
    {
        public const string StudentEmailSuffix = "@our.ecu.edu.au";
        public const string StaffEmailSuffix = "@ecu.edu.au";

        /// <summary>
        /// This static function generates an Email for a student or staff member.
        /// It assumes that the database has already been searched for matching Email addresses,
        /// and the match tally must be passed as param.
        /// 
        /// If matchTally less than 1, return FirstName.charAt[0] + LastName + Email suffix.
        /// Else if matchTally greater than 1, loop over first name til matchTally + LastName + Email suffix.
        /// Example: student, Martin Ponce, 2 matches, Email = maponce@our.ecu.edu.au
        /// </summary>
        /// <param name="emailType">string - Either "student" or "staff"</param>
        /// <param name="matchTally">int - Number of matches found</param>
        /// <param name="firstname">string</param>
        /// <param name="lastname">string</param>
        /// <returns name="Email">string</returns>
        public static string GenerateEmail(string emailType, int matchTally, string firstname, string lastname)
        {
            string email = "";

            switch (emailType)
            {
                // student emails, format: nsurname@our.ecu.edu.au
                case "student":
                    if (matchTally == 0)
                    {
                        email += firstname[matchTally] + lastname + StudentEmailSuffix;
                    }
                    // if not enough letters in FirstName to iterate,
                    else if (matchTally > firstname.Count())
                    {
                        // just concat matchtally to Email, ugly... but it does keep emails unique
                        email += firstname[0] + lastname + matchTally + StudentEmailSuffix;
                    }
                    else
                    {
                        for (int i = 0; i <= matchTally && i < firstname.Count(); i++)
                        {
                            email += firstname[i];
                        }
                        email += lastname + StudentEmailSuffix;
                    }
                    break;

                // staff emails, format: n.LastName@ecu.edu.au
                case "staff":
                    if (matchTally < 1)
                    {
                        email += firstname[0] + "." + lastname + StaffEmailSuffix;
                    }
                    // if not enough letters in FirstName to iterate,
                    else if (matchTally > firstname.Count())
                    {
                        // just concat matchtally to Email, ugly... but it does keep emails unique
                        email += firstname[0] + "." + lastname + matchTally + StaffEmailSuffix;
                    }
                    else
                    {
                        for (int i = 0; i <= matchTally && i < firstname.Count(); i++)
                        {
                            email += firstname[i];
                        }
                        email += "." + lastname + StaffEmailSuffix;
                    }
                    break;
            }

            return email;
        }
    }
}