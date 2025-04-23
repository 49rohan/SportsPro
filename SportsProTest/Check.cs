namespace SportsPro.Models
{
    public static class Check
    {
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }
    }
}
