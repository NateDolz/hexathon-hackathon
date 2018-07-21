using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;


namespace SoundService
{
    public class SoundService
    {
        private static SpeechFactory factory;
        private static SpeechFactory Factory = factory ?? (factory = SpeechFactory.FromSubscription(Constants.APIKey1, Constants.Region));

        public async Task KickOffListener()
        {
            var recognizer = Factory.CreateSpeechRecognizer();

            recognizer.RecognitionErrorRaised += RecognitionErrorRaised;=
            recognizer.IntermediateResultReceived += IntermediateResultReceived;
            #pragma warning disable CS4014 //Intent Is To leave this open in the background
            recognizer.StartContinuousRecognitionAsync();
            #pragma warning restore CS4014 //Intent Is to leave this open in the background



        }

        private void IntermediateResultReceived(object sender, SpeechRecognitionResultEventArgs e)
        {            
            var result = e.Result.Text;
            KeywordRecognized(result);
        }

        private void OnSpeechDetectedEvent(object sender, RecognitionEventArgs e)
        {
            
        }

        private void RecognitionErrorRaised(object sender, RecognitionErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public delegate void KeywordRecognizedDelegate(string keyWord);
        public KeywordRecognizedDelegate KeywordRecognized { get; set; }
        
    }
}
