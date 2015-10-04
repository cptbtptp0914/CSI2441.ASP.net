namespace A2.University.Web.Models.Business
{
    public static class GradeRules
    {
        public static string GetGrade(int mark)
        {
            if (mark >= 80)
            {
                return "HD";
            }
            else if (mark >= 70)
            {
                return "D";
            }
            else if (mark >= 60)
            {
                return "CR";
            }
            else if (mark >= 50)
            {
                return "C";
            }
            else
            {
                return "N";
            }

        }
    }
}