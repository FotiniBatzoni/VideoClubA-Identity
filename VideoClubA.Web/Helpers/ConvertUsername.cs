using System.Text.RegularExpressions;

namespace VideoClubA.Web.Helpers
{
    public static class ConvertUsername
    {
        public static string[] ToFirstAndLastName(this string username)
        {
            string[] nameParts = Regex.Split(username, @"(?<!^)(?=[A-Z])");

            string firstName;
            string lastName;

            string[] result = new string[]
            {
                firstName = nameParts[0],
                lastName = nameParts[1]
            };
 
            return result;
        }
    }
}
