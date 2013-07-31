using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureQuiz
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private Random _rnd;

        public List<Question> Questions { get; set; }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                NotifyPropertyChanged("CurrentQuestion");
            }
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }

        private int _round;
        public int Round
        {
            get { return _round; }
            set
            {
                _round = value;
                NotifyPropertyChanged("Round");
            }
        }



        public QuizViewModel()
        {
            _rnd = new Random();
        }

        public void LoadQuestions()
        {
            Questions = new List<Question>();

            Questions.Add(new Question("This city is known as \"the city that never sleeps\".", "newyork.jpg",
                "New York", "Las Vegas", "Los Angeles", "Atlanta",
                "Patrick Briggs", "http://www.flickr.com/photos/87241965@N00/4379712440/"));

            Questions.Add(new Question("This city is known for its traditional architecture, canals, shopping, and many coffeeshops.", "amsterdam.jpg",
                "Amsterdam", "Venice", "Leiden", "Rotterdam",
                "Christian Lendl", "http://www.flickr.com/photos/_dchris/7329055596/"));

            Questions.Add(new Question("This city is known for its beautiful harbor, along with the Harbour Bridge.", "sydney.jpg",
                "Sydney", "Melbourne", "Brisbane", "Perth",
                "Jimmy Harris", "http://www.flickr.com/photos/jimmyharris/114538159/"));

            Questions.Add(new Question("This is one of the oldest cities in Europe.", "lisbon.jpg",
                "Lisbon", "London", "Paris", "Madrid",
                "Terence S. Jones ", "http://www.flickr.com/photos/terence_s_jones/6684473529/"));

            Questions.Add(new Question("This is the capital and largest urban area of both England and the United Kingdom.", "london.jpg",
                "London", "Manchester", "York", "Leeds",
                "Tim Morris", "http://www.flickr.com/photos/timmorris/3103896345/"));

            Questions.Add(new Question("This city is known for being the fashion capital of the world.", "paris.jpg",
                "Paris", "Milan", "New York", "London",
                "Terrazzo", "http://www.flickr.com/photos/terrazzo/3958413757/"));

            Questions.Add(new Question("This is the only city in the world to contain in its interior a whole state.", "rome.jpg",
                "Rome", "Naples", "Milan", "Florence",
                "roamancing", "http://www.flickr.com/photos/roamancing/5795269117/"));

            Questions.Add(new Question("This city is the business and trade hub of United Arab Emirates.", "dubai.jpg",
                "Dubai", "Abu Dhabi", "Sharjah", "Al Ain",
                "Richard Schneider", "http://www.flickr.com/photos/picturecorrect/7623566780/"));

            Questions.Add(new Question("This city is referred to as the birthplace of democracy.", "athens.jpg",
                "Athens", "Rome", "Thessaloniki", "Naples",
                "Ronny Siegel", "http://www.flickr.com/photos/47309201@N02/9259286435/"));

            Questions.Add(new Question("This city has the second largest community of billionaires in the world.", "moscow.jpg",
                "Moscow", "New York", "London", "Hong Kong",
                "Lori Branham", "http://www.flickr.com/photos/kjunstorm/8337228066/"));
           
            NextQuestion();
        }

        public void NextQuestion()
        {
            if (Questions.Any())
            {
                CurrentQuestion = Questions[_rnd.Next(Questions.Count)];
                Questions.Remove(_currentQuestion);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        } 
    }
}
