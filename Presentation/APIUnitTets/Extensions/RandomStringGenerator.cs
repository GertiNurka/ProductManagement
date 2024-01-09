using System.Security.Cryptography;
using System.Text;

namespace APIUnitTets.Extensions;

/// <summary>
/// Generates a random string
/// TODO:
///     Extract this in a library
/// </summary>
internal static class RandomStringGenerator
{
    internal static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        byte[] randomBytes = new byte[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        StringBuilder stringBuilder = new StringBuilder(length);

        foreach (byte b in randomBytes)
        {
            stringBuilder.Append(chars[b % chars.Length]);
        }

        return stringBuilder.ToString();
    }
}
