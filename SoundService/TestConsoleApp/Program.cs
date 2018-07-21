using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RecognitionWithMicrophoneAsync().Wait();
        }


        // Speech recognition from microphone.
        public static async Task RecognitionWithMicrophoneAsync()
        {

            // <recognitionWithMicrophone>
            // Creates an instance of a speech factory with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var factory = SpeechFactory.FromSubscription("9ee338f6889d477a873423c94fb5739d", "westus");

            // Creates a speech recognizer using microphone as audio input. The default language is "en-us".
            using (var recognizer = factory.CreateSpeechRecognizer())
            {
                // Starts recognizing.
                Console.WriteLine("Say something...");

                // Performs recognition.
                // RecognizeAsync() returns when the first utterance has been recognized, so it is suitable 
                // only for single shot recognition like command or query. For long-running recognition, use
                // StartContinuousRecognitionAsync() instead.
                var result = await recognizer.RecognizeAsync().ConfigureAwait(false);

                // Checks result.
                if (result.RecognitionStatus != RecognitionStatus.Recognized)
                {
                    Console.WriteLine($"Recognition status: {result.RecognitionStatus.ToString()}");
                    if (result.RecognitionStatus == RecognitionStatus.Canceled)
                    {
                        Console.WriteLine($"There was an error, reason: {result.RecognitionFailureReason}");
                    }
                    else
                    {
                        Console.WriteLine("No speech could be recognized.\n");
                    }
                }
                else
                {
                    Console.WriteLine($"We recognized: {result.Text}, Offset: {result.OffsetInTicks}, Duration: {result.Duration}.");
                }
            }
            // </recognitionWithMicrophone>
        }
    }
}
