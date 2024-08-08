using System;

namespace Lemoncode.Hash.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "lemoncode";
            var hash = input.ToSha256();

            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Hash: {hash}");

            Console.WriteLine($"Validate again {input}");
            var isSameHash = hash == input.ToSha256();
            Console.WriteLine($"Is same hash: {isSameHash}");
        }
    }
}