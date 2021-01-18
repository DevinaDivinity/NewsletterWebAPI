using NewsletterAPI.Common.Enums;

namespace NewsletterAPI.Common.Helpers
{
    public static class Validations
    {
        public static bool IncomingRequest(string value, ValidationType validationType = ValidationType.Basic)
        {
            if (value == string.Empty)
            {
                return false;
            }

            if (validationType.Equals(ValidationType.Email))
            {
                if (!EmailFormat(value)) return false;
            }

            return true;
        }

        private static bool EmailFormat(string email)
        {
            try
            {
                System.Net.Mail.MailAddress correctEmail = new(email);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
