namespace Infrastructure.Randomizer.Custom
{
    public class RandomGenerator
    {
        public static string GenerateRandomAlphanumeric(int length)
        {
            const string AlphanumericChars =
                                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                        "abcdefghijklmnopqrstuvwxyz" +
                                        "0123456789";

            var random = new Random();
            return new string(Enumerable
                .Repeat(AlphanumericChars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
