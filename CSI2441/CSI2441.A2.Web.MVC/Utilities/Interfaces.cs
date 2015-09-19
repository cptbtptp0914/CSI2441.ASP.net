namespace CSI2441.A2.Web.MVC.Utilities
{
    interface IEmailGenerator
    {
        // Must search db for existing emails first.
        string GenerateEmail(string firstName, string surname);
        // Get list of emails in DB, ORDER BY email
        void GetEmailRecordset();
    }
}
