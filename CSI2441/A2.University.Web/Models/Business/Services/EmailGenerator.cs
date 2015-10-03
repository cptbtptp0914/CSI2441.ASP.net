using System.Linq;

namespace A2.University.Web.Models.Business.Services
{
    public static class EmailGenerator
    {
        public static string StudentEmailSuffix = "@our.ecu.edu.au";
        public static string StaffEmailSuffix = "@ecu.edu.au";


        /// <summary>
        /// This static function generates an email for a student or staff member.
        /// It assumes that the database has already been searched for matching email addresses,
        /// and the match tally must be passed as param.
        /// 
        /// If matchTally less than 1, return firstname.charAt[0] + lastname + email suffix.
        /// Else if matchTally greater than 1, loop over first name til matchTally + lastname + email suffix.
        /// Example: student, Martin Ponce, 2 matches, email = maponce@our.ecu.edu.au
        /// </summary>
        /// <param name="emailType">string - Either "student" or "staff"</param>
        /// <param name="matchTally">int - Number of matches found</param>
        /// <param name="firstname">string</param>
        /// <param name="lastname">string</param>
        /// <returns name="email">string</returns>
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
                    // if not enough letters in firstname to iterate,
                    else if (matchTally > firstname.Count())
                    {
                        // just concat matchtally to email, ugly... but it does keep emails unique
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

                // staff emails, format: n.surname@ecu.edu.au
                case "staff":
                    if (matchTally < 1)
                    {
                        email += firstname[0] + "." + lastname + StaffEmailSuffix;
                    }
                    // if not enough letters in firstname to iterate,
                    else if (matchTally > firstname.Count())
                    {
                        // just concat matchtally to email, ugly... but it does keep emails unique
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