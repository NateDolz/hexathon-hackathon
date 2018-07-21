using System;
using SoundService;

namespace SoundServiceTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var soundService = new SoundService.SoundService();
            
            soundService.ShowItem = item => Console.WriteLine("Show " + item);
            soundService.RemoveItem = item => Console.WriteLine("Remove " + item);
            soundService.TurnItem = (item , direction) => Console.WriteLine("Turn " + item + " " + direction.ToString());
            soundService.MoveItem = (item, direction) => Console.WriteLine("Move " + item + " " + direction.ToString());
            soundService.ExpandItem = item => Console.WriteLine("Expand " + item);
            soundService.CollapseItem = item => Console.WriteLine("Collapse " + item);
            soundService.Faster = () => Console.WriteLine("Faster");            
            soundService.Slower = () => Console.WriteLine("Slower");
            soundService.Stop = () => Console.WriteLine("Stop");

            Console.WriteLine("Start Talking...");
            soundService.KickOffListener();
            Console.ReadLine();
        }
    }
}
