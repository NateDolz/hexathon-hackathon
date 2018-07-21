using System;
using System.IO;
using SoundService;

namespace SoundServiceTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var soundService = new SoundService.SoundService();

            var tempDirectory = Path.GetTempPath();
            var filePath = tempDirectory + @"\SpeechCommands.txt";
            var streamWriter = new StreamWriter(filePath);
            streamWriter.AutoFlush = true;

            soundService.ShowItem = item =>
            {
                Console.WriteLine("Show " + item);
                streamWriter.WriteLine("Show " + item);
            };
            soundService.RemoveItem = item =>
            {
                Console.WriteLine("Remove " + item);
                streamWriter.WriteLine("Remove " + item);
            };
            soundService.TurnItem = (item , direction) =>
            {
                Console.WriteLine("Turn " + item + " " + direction.ToString());
                streamWriter.WriteLine("Turn " + item + " " + direction.ToString());
            };
            soundService.MoveItem = (item, direction) =>
            {
                Console.WriteLine("Move " + item + " " + direction.ToString());
                streamWriter.WriteLine("Move " + item + " " + direction.ToString());
            };
            soundService.ExpandItem = item =>
            {
                Console.WriteLine("Expand " + item);
                streamWriter.WriteLine("Expand " + item);
            };
            soundService.CollapseItem = item =>
            {
                Console.WriteLine("Collapse " + item);
                streamWriter.WriteLine("Collapse " + item);
            };
            soundService.Faster = () =>
            {
                Console.WriteLine("Faster");
                streamWriter.WriteLine("Faster");
            };            
            soundService.Slower = () =>
            {
                Console.WriteLine("Slower");
                streamWriter.WriteLine("Slower");
            };
            soundService.Stop = () =>
            {
                Console.WriteLine("Stop");
                streamWriter.WriteLine("Stop");
            };

            soundService.ShowItem = item => streamWriter.WriteLine("Show " + item);
            soundService.RemoveItem = item => streamWriter.WriteLine("Remove " + item);
            soundService.TurnItem = (item , direction) => streamWriter.WriteLine("Turn " + item + " " + direction.ToString());
            soundService.MoveItem = (item, direction) => streamWriter.WriteLine("Move " + item + " " + direction.ToString());
            soundService.ExpandItem = item => streamWriter.WriteLine("Expand " + item);
            soundService.CollapseItem = item => streamWriter.WriteLine("Collapse " + item);
            soundService.Faster = () => streamWriter.WriteLine("Faster");            
            soundService.Slower = () => streamWriter.WriteLine("Slower");
            soundService.Stop = () => streamWriter.WriteLine("Stop");

            Console.WriteLine("Start Talking...");
            soundService.KickOffListener();
            Console.ReadLine();

            streamWriter.Close();
            soundService.Dispose();
        }
    }
}
