using System.Runtime.InteropServices;
using System.Text;

namespace OnlineStore.Entities.User;

public sealed class UserParameters
{
    public const Role DEFAULT_ROLE = Role.User;

    private static readonly string _alphanumericCharacters = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToString()
                                                             + Enumerable.Range('A', 'A' - 'A' + 1).Select(c => (char)c).ToString()
                                                             + Enumerable.Range('0', '9' - '0' + 1).Select(c => (char)c).ToString()!;
    private UserParameters() { }

    public static class Internet
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsAvailable()
            => InternetGetConnectedState(out int _, 0);
    }

    public sealed class Login
    {
        private const int LENGTH_OF_LOGIN = 10;

        private Login() { }

        public static string Generate()
        {
            StringBuilder loginBuilder = new StringBuilder();
            Random random = new Random();

            for (int index = 0; index < LENGTH_OF_LOGIN; index++)
                loginBuilder.Append(_alphanumericCharacters[random.Next(_alphanumericCharacters.Length)]);

            for (int index = 0; index < LENGTH_OF_LOGIN; index++)
            {
                int randomIndex = index + (int)(random.NextDouble() * (LENGTH_OF_LOGIN - index));
                (loginBuilder[index], loginBuilder[randomIndex])
                    = (loginBuilder[randomIndex], loginBuilder[index]);
            }

            return loginBuilder.ToString();
        }
    }

    public sealed class Password
    {
        private static readonly int LENGTH_OF_PASSWORD = 10;

        private Password() { }

        public static string Generate()
        {
            StringBuilder loginBuilder = new StringBuilder();
            Random random = new Random();

            for (int index = 0; index < LENGTH_OF_PASSWORD; index++)
                loginBuilder.Append(_alphanumericCharacters[random.Next(_alphanumericCharacters.Length)]);

            for (int index = 0; index < LENGTH_OF_PASSWORD; index++)
            {
                int randomIndex = index + (int)(random.NextDouble() * (LENGTH_OF_PASSWORD - index));
                (loginBuilder[index], loginBuilder[randomIndex])
                    = (loginBuilder[randomIndex], loginBuilder[index]);
            }

            return loginBuilder.ToString();
        }
    }
}