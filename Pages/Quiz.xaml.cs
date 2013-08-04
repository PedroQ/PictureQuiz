using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PictureQuiz.Resources;
using System.Windows.Media.Imaging;
using Nokia.Graphics.Imaging;
using System.IO;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;


namespace PictureQuiz
{
    public partial class MainPage : PhoneApplicationPage
    {
        private QuizViewModel _viewModel;
        private Random _rnd;
        private bool _hideHint = false;

        private ImageProcessor _imgProcessor;
        private ImageProcessor.Difficulty _difficulty = ImageProcessor.Difficulty.Easy;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _imgProcessor = new ImageProcessor();
            _rnd = new Random();
            _viewModel = new QuizViewModel();
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            this.DataContext = _viewModel;

            LoadQuestionsAndReset();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            //Apply game settings
            if (IsolatedStorageSettings.ApplicationSettings.Contains("HidePictureDescription"))
            {
                _hideHint = (bool)IsolatedStorageSettings.ApplicationSettings["HidePictureDescription"];
                if (_hideHint)
                    tbQuestion.Visibility = System.Windows.Visibility.Collapsed;
            }

            HideImageStoryboard.Completed += HideImageStoryboard_Completed;
            RevealImageStoryboard.Completed += RevealImageStoryboard_Completed;

            base.OnNavigatedTo(e);
        }

        void HideImageStoryboard_Completed(object sender, EventArgs e)
        {
            //set original picture
            questionImage.Source = new BitmapImage(new Uri("/" + _viewModel.CurrentQuestion.Picture, UriKind.Relative)) { CreateOptions = BitmapCreateOptions.None };

            RevealImageStoryboard.Begin();
        }

        void RevealImageStoryboard_Completed(object sender, EventArgs e)
        {
            if (_viewModel.Round < 5) //five rounds per game
            {
                _viewModel.Round++;
                _viewModel.NextQuestion();
            }
            else
            {
                // Display message asking the user if he/she wants to repeat. If not, go back to the menu
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "Game over!",
                    Message = "Your score: " + _viewModel.Score + Environment.NewLine + "Play again?",
                    LeftButtonContent = "yes",
                    RightButtonContent = "no"
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    if (e1.Result == CustomMessageBoxResult.LeftButton)
                        LoadQuestionsAndReset();
                    else
                        NavigationService.GoBack();
                };

                messageBox.Show();
            }
        }

        private void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentQuestion")
            {
                LoadQuestion();
            }
        }

        private async void LoadQuestion()
        {
            //Randomize answers and populate the buttons
            string[] answers = new string[4];
            _viewModel.CurrentQuestion.Answers.CopyTo(answers, 0);
            answers[3] = _viewModel.CurrentQuestion.CorrectAnswer;

            answers = answers.OrderBy(x => _rnd.Next()).ToArray();

            btnAnswer1.Content = answers[0];
            btnAnswer2.Content = answers[1];
            btnAnswer3.Content = answers[2];
            btnAnswer4.Content = answers[3];


            //Load the picture and apply some filters to obfuscate it
            Uri questionUri = new Uri(_viewModel.CurrentQuestion.Picture, UriKind.Relative);
            Stream questionImageStream = Application.GetResourceStream(questionUri).Stream;
            await _imgProcessor.ApplyDifficultyToImage(questionImageStream, _difficulty, questionImage);

            LoadQuestionStoryboard.Begin();
        }

        private void AnswerButton(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if ((string)btn.Content == _viewModel.CurrentQuestion.CorrectAnswer)
            {
                // Points are incremented according to the current difficulty
                // Easy = 1, Moderate = 2, Hard = 3, and so on
                // Double points if playing without hints

                if (_hideHint)
                    _viewModel.Score += (int)_difficulty * 2;
                else
                    _viewModel.Score += (int)_difficulty;

                IncreaseDifficulty();
            }

            HideImageStoryboard.Begin();
        }

        private void LoadQuestionsAndReset()
        {
            _viewModel.Score = 0;
            _viewModel.Round = 1;
            _difficulty = ImageProcessor.Difficulty.Easy;
            _viewModel.LoadQuestions();
        }

        private void IncreaseDifficulty()
        {
            if (_difficulty < ImageProcessor.Difficulty.ReallyHard)
                _difficulty++;
        }
    }
}