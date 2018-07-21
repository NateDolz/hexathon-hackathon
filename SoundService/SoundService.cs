using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;


namespace SoundService
{
    public class SoundService : IDisposable
    {

        #region Listener Fields

        private static SpeechFactory factory;
        private static readonly SpeechFactory Factory = factory ?? 
                                                        (factory = SpeechFactory.FromSubscription(Constants.APIKey1, Constants.Region));

        private static SpeechRecognizer recognizer;
        private static readonly SpeechRecognizer Recognizer = recognizer ?? 
                                                              (recognizer = Factory.CreateSpeechRecognizer());

        #endregion

        #region Listener Methods

        public void KickOffListener()
        {
            Recognizer.RecognitionErrorRaised += RecognitionErrorRaised;
            Recognizer.FinalResultReceived += FinalResultRecieved;
            #pragma warning disable CS4014 //Intent Is To leave this open in the background
            Recognizer.StartContinuousRecognitionAsync();
            #pragma warning restore CS4014 //Intent Is to leave this open in the background
        }

        private void FinalResultRecieved(object sender, SpeechRecognitionResultEventArgs e) =>
            ProcessSpeachResult(e.Result.Text);

        private void RecognitionErrorRaised(object sender, RecognitionErrorEventArgs e)
        {
            
        }

        #endregion

        #region Command Delegates
        
        public delegate void ShowItemDelegate(string item);
        public ShowItemDelegate ShowItem { get; set; }

        public delegate void RemoveItemDelegate(string item);

        public RemoveItemDelegate RemoveItem { get; set; }

        public delegate void TurnItemDelegate(string item, Direction direction);
        public TurnItemDelegate TurnItem { get; set; }

        public delegate void MoveItemDelegate(string item, Direction direction);
        public MoveItemDelegate MoveItem { get; set; }

        public delegate void ExpandItemDelegate(string item);
        public ExpandItemDelegate ExpandItem { get; set; }

        public delegate void CollapseItemDelegate(string item);
        public CollapseItemDelegate CollapseItem { get; set; }

        public delegate void StopDelegate();
        public StopDelegate Stop { get; set; }

        public delegate void FasterDelegate();
        public FasterDelegate Faster { get; set; }

        public delegate void SlowerDelegate();
        public SlowerDelegate Slower { get; set; }

        #endregion

        #region  Speech Result Processing

        private void ProcessSpeachResult(string speech)
        {
            speech = speech.Replace("?", "");
            speech = speech.Replace(".", "");
            speech = speech.Replace(",", "");            
            speech = speech.ToLower();

            var showRegex = new Regex(@"(?:^|\W)show(?:$|\W)");
            var removeRegex = new Regex(@"(?:^|\W)remove(?:$|\W)");
            var expandRegex = new Regex(@"(?:^|\W)expand(?:$|\W)");
            var turnRegex = new Regex(@"(?:^|\W)turn(?:$|\W)");
            var moveRegex = new Regex(@"(?:^|\W)move(?:$|\W)");
            var collapseRegex = new Regex(@"(?:^|\W)collapse(?:$|\W)");
            var stopRegex = new Regex(@"(?:^|\W)stop(?:$|\W)");
            var fasterRegex = new Regex(@"(?:^|\W)faster(?:$|\W)");
            var slowerRegex = new Regex(@"(?:^|\W)slower(?:$|\W)");
            
            var showMatch = showRegex.Match(speech);
            var removeMatch = removeRegex.Match(speech);
            var expandMatch = expandRegex.Match(speech);
            var turnMatch = turnRegex.Match(speech);
            var moveMatch = moveRegex.Match(speech);
            var collapseMatch = collapseRegex.Match(speech);
            var stopMatch = stopRegex.Match(speech);
            var fasterMatch = fasterRegex.Match(speech);
            var slowerMatch = slowerRegex.Match(speech);

            if (showMatch.Success) ShowMatch(speech.Substring(showMatch.Index + showMatch.Length).Trim());
            if (removeMatch.Success) RemoveMatch(speech.Substring(removeMatch.Index + removeMatch.Length).Trim());
            if (expandMatch.Success) ExpandMatch(speech.Substring(expandMatch.Index + expandMatch.Length).Trim());
            if (turnMatch.Success) TurnMatch(speech.Substring(turnMatch.Index + turnMatch.Length).Trim());
            if (moveMatch.Success) MoveMatch(speech.Substring(moveMatch.Index + moveMatch.Length).Trim());
            if (collapseMatch.Success) CollapseMatch(speech.Substring(collapseMatch.Index + collapseMatch.Length).Trim());
            if (stopMatch.Success) Stop();
            if (fasterMatch.Success) Faster();
            if (slowerMatch.Success) Slower();
        }

        private void ShowMatch(string speech) => ShowItem(speech.Replace(" ", ""));

        private void RemoveMatch(string speech) => RemoveItem(speech.Replace(" ", ""));

        private void ExpandMatch(string speech) => ExpandItem(speech.Replace(" ", ""));

        private void CollapseMatch(string speech) => CollapseItem(speech.Replace(" ", ""));

        private void TurnMatch(string speech)
        {
            var upRegex = new Regex(@"(?:^|\W)up(?:$|\W)");
            var downRegex = new Regex(@"(?:^|\W)down(?:$|\W)");
            var forwardRegex = new Regex(@"(?:^|\W)forward(?:$|\W)");
            var backRegex = new Regex(@"(?:^|\W)back(?:$|\W)");
            var leftRegex = new Regex(@"(?:^|\W)left(?:$|\W)");
            var rightRegex = new Regex(@"(?:^|\W)right(?:$|\W)");

            var upMatch = upRegex.Match(speech);
            var downMatch = downRegex.Match(speech);
            var forwardMatch = forwardRegex.Match(speech);
            var backMatch = backRegex.Match(speech);
            var leftMatch = leftRegex.Match(speech);
            var rightMatch = rightRegex.Match(speech);

            if (upMatch.Success) TurnItem(speech.Substring(0, upMatch.Index).Trim(), Direction.Up);
            if (downMatch.Success) TurnItem(speech.Substring(0, downMatch.Index).Trim(), Direction.Down);
            if (forwardMatch.Success) TurnItem(speech.Substring(0, forwardMatch.Index).Trim(), Direction.Forward);
            if (backMatch.Success) TurnItem(speech.Substring(0, backMatch.Index).Trim(), Direction.Back);
            if (leftMatch.Success) TurnItem(speech.Substring(0, leftMatch.Index).Trim(), Direction.Left);
            if (rightMatch.Success) TurnItem(speech.Substring(0, rightMatch.Index).Trim(), Direction.Right);
        }

        private void MoveMatch(string speech)
        {
            var upRegex = new Regex(@"(?:^|\W)up(?:$|\W)");
            var downRegex = new Regex(@"(?:^|\W)down(?:$|\W)");
            var forwardRegex = new Regex(@"(?:^|\W)forward(?:$|\W)");
            var backRegex = new Regex(@"(?:^|\W)back(?:$|\W)");
            var leftRegex = new Regex(@"(?:^|\W)left(?:$|\W)");
            var rightRegex = new Regex(@"(?:^|\W)right(?:$|\W)");

            var upMatch = upRegex.Match(speech);
            var downMatch = downRegex.Match(speech);
            var forwardMatch = forwardRegex.Match(speech);
            var backMatch = backRegex.Match(speech);
            var leftMatch = leftRegex.Match(speech);
            var rightMatch = rightRegex.Match(speech);

            if (upMatch.Success) MoveItem(speech.Substring(0, upMatch.Index).Trim(), Direction.Up);
            if (downMatch.Success) MoveItem(speech.Substring(0, downMatch.Index).Trim(), Direction.Down);
            if (forwardMatch.Success) MoveItem(speech.Substring(0, forwardMatch.Index).Trim(), Direction.Forward);
            if (backMatch.Success) MoveItem(speech.Substring(0, backMatch.Index).Trim(), Direction.Back);
            if (leftMatch.Success) MoveItem(speech.Substring(0, leftMatch.Index).Trim(), Direction.Left);
            if (rightMatch.Success) MoveItem(speech.Substring(0, rightMatch.Index).Trim(), Direction.Right);
        }

        #endregion

        #region Logging

        private void LogError(string description, Exception e = null)
        {
            //TODO:
        }

        #endregion

        #region IDisposable

        public void Dispose() => Recognizer.Dispose();

        #endregion
    }

    public enum Direction
    {
        Forward,
        Back,
        Up,
        Down,
        Left,
        Right,
    }

}
