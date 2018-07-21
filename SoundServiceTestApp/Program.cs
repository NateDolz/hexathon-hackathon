using System;
using SoundService;

namespace SoundServiceTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var soundService = new SoundService.SoundService();
            soundService.KeywordRecognized = Console.WriteLine;

            Console.WriteLine("Start Talking...");
            soundService.KickOffListener();
            Console.ReadLine();
        }
    }
}
