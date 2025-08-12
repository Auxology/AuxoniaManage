namespace AuxoniaManage.Domain.Utils;

public sealed class Generators
{
    private readonly int _length = 100;
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public string RandomVeryLongString
    {
        get
        {
            var random = new Random();
            return new string(Enumerable.Repeat(Chars, _length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}