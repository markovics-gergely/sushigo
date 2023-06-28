using shared.bll.Validators.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace user.bll.Validators
{
    public class EmailValidator : IValidator
    {
        private readonly string _email;

        public EmailValidator(string email)
        {
            _email = email;
        }

        // Examines the domain part of the email and normalizes it.
        private static string DomainMapper(Match match)
        {
            // Use IdnMapping class to convert Unicode domain names.
            var idn = new IdnMapping();

            // Pull out and process domain name (throws ArgumentException on invalid)
            string domainName = idn.GetAscii(match.Groups[2].Value);

            return match.Groups[1].Value + domainName;
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_email))
            {
                return false;
            }
            try
            {
                // Normalize the domain
                var email = Regex.Replace(_email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                return Regex.IsMatch(
                    email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)
                );
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
