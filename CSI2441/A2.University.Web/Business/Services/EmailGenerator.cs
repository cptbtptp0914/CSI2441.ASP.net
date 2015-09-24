using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using A2.University.Web.Models.Entities;
using Microsoft.Ajax.Utilities;

namespace A2.University.Web.Business.Services
{
    public static class EmailGenerator
    {
        private static List<string> emails { get; set; }
        private static string firstname;
        private static string surname;

        private static string email;
        private static string emailSuffix = "@our.ecu.edu.au";

        public static void GenerateEmail(string emailType, List<string> emaiList, string firstname, string surname)
        {
//            int matchIndex;
            int matchTally = 0;
            int i;

            if (emailType == "student")
            {
                emails = emaiList;
                emails.Sort();
                email = firstname[0] + surname;
                i = emails.Count;
            }
        }

        private static string BinarySearch (List<string> sortedEmails, string target)
        {

            return email;
        }
    }
}