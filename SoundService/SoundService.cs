using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;


namespace SoundService
{
    public class SoundService
    {
        private static SpeechFactory factory;
        private static SpeechFactory Factory = factory ?? (factory = SpeechFactory.FromSubscription(Constants.APIKey1, Constants.Region));

        public void KickOffListener()
        {
            var recognizer = Factory.CreateSpeechRecognizer();

            recognizer.RecognitionErrorRaised += RecognitionErrorRaised;
            recognizer.FinalResultReceived += FinalResultRecieved;
            #pragma warning disable CS4014 //Intent Is To leave this open in the background
            recognizer.StartContinuousRecognitionAsync();
            #pragma warning restore CS4014 //Intent Is to leave this open in the background
        }

        private void FinalResultRecieved(object sender, SpeechRecognitionResultEventArgs e)
        {
            var result = e.Result.Text;
            KeywordRecognized(result);        
        }

        private void RecognitionErrorRaised(object sender, RecognitionErrorEventArgs e)
        {
            
        }

        public delegate void KeywordRecognizedDelegate(string keyWord);
        public KeywordRecognizedDelegate KeywordRecognized { get; set; }

        private KeyValuePair<string, string> ProcessSpeachResult(string speech)
        {
            speech = speech.Replace("?", "");
            speech = speech.Replace(".", "");
            speech = speech.Replace(",", "");
            speech = speech.Replace(" ", "");
            speech = speech.ToLower();

            var showExpression = new Regex(@"(?:^|\W)showme(?:$|\W)");
            var expandRegex = new Regex(@"(?:^|\W)expand(?:$|\W)");
            
        }


    }
}
