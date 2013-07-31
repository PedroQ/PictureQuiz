using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PictureQuiz
{
    public class Question
    {
        private const string PICTURES_PATH = "Assets/Questions/";

        public string QuestionText { get; set; }
        public string Picture { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] Answers { get; set; }
        public string Attribution { get; set; }
        public string AttributionUrl { get; set; }

        public Question(string questionText, string pictureLocation,
            string correctAnswer,
            string wrongAnswer1,
            string wrongAnswer2,
            string wrongAnswer3,
            string author, string flickrPage)
        {
            QuestionText = questionText;
            Picture = PICTURES_PATH + pictureLocation;
            CorrectAnswer = correctAnswer;
            Answers = new string[3];
            Answers[0] = wrongAnswer1;
            Answers[1] = wrongAnswer2;
            Answers[2] = wrongAnswer3;
            Attribution = author;
            AttributionUrl = flickrPage;
        }
    }
}
