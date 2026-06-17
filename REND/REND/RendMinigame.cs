using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace REND
{
    /// <summary>
    /// Minigame system for the REND chatbot based on cybersecurity knowledge.
    /// Features multiple question types, scoring, and difficulty levels.
    /// </summary>
    public class RendMinigame
    {
        private int _score;
        private int _questionsAnswered;
        private int _correctAnswers;
        private List<GameQuestion> _questions;
        private Random _random;
        private GameQuestion _currentQuestion;
        private int _currentQuestionIndex;
        private bool _isGameActive;
        private const int QuestionsPerGame = 10;

        public int Score => _score;
        public int QuestionsAnswered => _questionsAnswered;
        public int CorrectAnswers => _correctAnswers;
        public bool IsGameActive => _isGameActive;
        public GameQuestion CurrentQuestion => _currentQuestion;

        public RendMinigame()
        {
            // Use RNG.Shared for better randomization, fallback to new Random() for older .NET versions
            _random = new Random();
            _questions = InitializeQuestions();
            _score = 0;
            _questionsAnswered = 0;
            _correctAnswers = 0;
            _isGameActive = false;
            _currentQuestionIndex = 0;
        }

        /// <summary>
        /// Starts the minigame session.
        /// </summary>
        public string StartGame()
        {
            _isGameActive = true;
            _score = 0;
            _questionsAnswered = 0;
            _correctAnswers = 0;
            _currentQuestionIndex = 0;

            // Select random subset of questions and shuffle them
            SelectAndShuffleQuestions();

            return GetWelcomeMessage();
        }

        /// <summary>
        /// Gets the welcome message for the game.
        /// </summary>
        private string GetWelcomeMessage()
        {
            return "Welcome to the Cyber Security Minigame! ??\n\n" +
                   $"You'll face {QuestionsPerGame} questions covering various cyber security topics.\n" +
                   "Correct answers earn points based on difficulty:\n" +
                   "  - Easy: 10 points\n" +
                   "  - Medium: 25 points\n" +
                   "  - Hard: 50 points\n\n" +
                   "Let's start! Here's your first question...\n";
        }

        /// <summary>
        /// Gets the next question or ends the game.
        /// </summary>
        public string GetNextQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                return EndGame();
            }

            _currentQuestion = _questions[_currentQuestionIndex];
            _currentQuestionIndex++;

            return _currentQuestion.GetFormattedQuestion();
        }

        /// <summary>
        /// Processes user answer and returns feedback with next question or game end.
        /// </summary>
        public string ProcessAnswer(string userAnswer)
        {
            if (_currentQuestion == null || !_isGameActive)
            {
                return "No question to answer. Start a new game with 'Let's play a game'.";
            }

            userAnswer = userAnswer.Trim();
            bool isCorrect = _currentQuestion.CheckAnswer(userAnswer);

            _questionsAnswered++;

            string feedback;
            if (isCorrect)
            {
                _correctAnswers++;
                _score += _currentQuestion.PointsForCorrect;
                feedback = $"? Correct! You earned {_currentQuestion.PointsForCorrect} points!\n" +
                          $"Explanation: {_currentQuestion.Explanation}\n" +
                          $"Current Score: {_score}\n\n";
            }
            else
            {
                feedback = $"? Incorrect. The correct answer is: {_currentQuestion.CorrectAnswer}\n" +
                          $"Explanation: {_currentQuestion.Explanation}\n" +
                          $"Current Score: {_score}\n\n";
            }

            // Get next question or end game
            if (_currentQuestionIndex >= _questions.Count)
            {
                feedback += EndGame();
                return feedback;
            }

            feedback += GetNextQuestion();
            return feedback;
        }

        /// <summary>
        /// Ends the game and displays final statistics.
        /// </summary>
        private string EndGame()
        {
            _isGameActive = false;

            string accuracy = _questionsAnswered > 0
                ? $"{(_correctAnswers * 100) / _questionsAnswered}%"
                : "0%";

            return "?? GAME OVER! ??\n\n" +
                   $"Final Score: {_score}\n" +
                   $"Correct Answers: {_correctAnswers}/{_questionsAnswered}\n" +
                   $"Accuracy: {accuracy}\n\n" +
                   GetFinalFeedback(accuracy);
        }

        /// <summary>
        /// Generates motivational feedback based on final performance.
        /// </summary>
        private string GetFinalFeedback(string accuracy)
        {
            return _score switch
            {
                >= 400 => "Excellent! You're a cyber security expert! ??",
                >= 300 => "Great job! You know your cyber security well! ??",
                >= 200 => "Good effort! Keep learning about cyber security. ??",
                >= 100 => "Not bad! Review the topics and try again. ??",
                _ => "Keep practicing! Cyber security is a journey. ??"
            };
        }

        /// <summary>
        /// Quits the current game session.
        /// </summary>
        public string QuitGame()
        {
            if (!_isGameActive)
            {
                return "No active game to quit.";
            }

            _isGameActive = false;

            return $"Game ended early.\n" +
                   $"Final Score: {_score}\n" +
                   $"Questions Answered: {_questionsAnswered}\n" +
                   $"Correct Answers: {_correctAnswers}\n\n" +
                   "Feel free to play again anytime!";
        }

        /// <summary>
        /// Selects a random subset of questions and shuffles both the list and answer options.
        /// </summary>
        private void SelectAndShuffleQuestions()
        {
            // Get all available questions
            var allQuestions = InitializeQuestions();

            // Select random subset
            _questions = allQuestions
                .OrderBy(_ => _random.Next())
                .Take(QuestionsPerGame)
                .ToList();

            // Shuffle the final subset
            ShuffleQuestions();

            // Shuffle answer options for each multiple choice question
            foreach (var question in _questions)
            {
                if (question is MultipleChoiceQuestion mcQuestion)
                {
                    mcQuestion.ShuffleAnswers(_random);
                }
                else if (question is ScenarioQuestion scenarioQuestion)
                {
                    scenarioQuestion.ShuffleAnswers(_random);
                }
            }
        }

        /// <summary>
        /// Shuffles the question list for randomized gameplay using Fisher-Yates algorithm.
        /// </summary>
        private void ShuffleQuestions()
        {
            for (int i = _questions.Count - 1; i > 0; i--)
            {
                int randomIndex = _random.Next(i + 1);
                var temp = _questions[i];
                _questions[i] = _questions[randomIndex];
                _questions[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Initializes all game questions with various types.
        /// </summary>
        private List<GameQuestion> InitializeQuestions()
        {
            return new List<GameQuestion>
            {
                // Multiple Choice Questions
                new MultipleChoiceQuestion(
                    "What does HTTPS stand for?",
                    new[] { "Hyper Text Transfer Protocol Secure", "High Text Transfer Protocol System", "Hyper Transmission Text Protocol", "Home Text Transfer Protocol Standard" },
                    0,
                    Difficulty.Easy,
                    25,
                    "HTTPS is the secure version of HTTP that encrypts data in transit."),

                new MultipleChoiceQuestion(
                    "Which of the following is NOT a type of malware?",
                    new[] { "Antivirus", "Trojan", "Worm", "Virus" },
                    0,
                    Difficulty.Easy,
                    25,
                    "Antivirus is a protective tool, not malware. Trojans, worms, and viruses are all malicious."),

                new MultipleChoiceQuestion(
                    "What does DDoS stand for?",
                    new[] { "Distributed Denial of Service", "Direct Data Online Service", "Digital Defense Operating System", "Distributed Download Service" },
                    0,
                    Difficulty.Medium,
                    25,
                    "DDoS attacks overwhelm services by flooding them with traffic from multiple sources."),

                new MultipleChoiceQuestion(
                    "Which encryption standard is commonly used for wireless networks?",
                    new[] { "WPA2", "HTTP", "FTP", "TCP" },
                    0,
                    Difficulty.Medium,
                    25,
                    "WPA2 (Wi-Fi Protected Access 2) is the current standard for securing wireless networks."),

                new MultipleChoiceQuestion(
                    "What is a 'zero-day' vulnerability?",
                    new[] { "An exploit unknown to the software vendor before public release", "A vulnerability found on day zero of the month", "A security issue lasting less than 24 hours", "A bug found in legacy systems" },
                    0,
                    Difficulty.Hard,
                    50,
                    "Zero-day vulnerabilities are previously unknown exploits, making them particularly dangerous."),

                // True/False Questions
                new TrueFalseQuestion(
                    "A strong password should contain uppercase, lowercase, numbers, and special characters.",
                    true,
                    Difficulty.Easy,
                    10,
                    "Complex passwords with mixed character types are significantly more secure against brute force attacks."),

                new TrueFalseQuestion(
                    "Two-factor authentication eliminates all risk of account compromise.",
                    false,
                    Difficulty.Medium,
                    25,
                    "While 2FA greatly improves security, it can still be vulnerable to sophisticated attacks like SIM swapping."),

                new TrueFalseQuestion(
                    "Phishing is exclusively delivered through email.",
                    false,
                    Difficulty.Medium,
                    25,
                    "Phishing can occur via email, SMS (smishing), phone calls (vishing), and social media."),

                new TrueFalseQuestion(
                    "A VPN completely anonymizes all your online activities.",
                    false,
                    Difficulty.Hard,
                    50,
                    "While VPNs add security and hide your IP, they don't guarantee complete anonymity—your VPN provider can see your traffic."),

                // Fill-in-the-Blank Questions
                new FillInBlankQuestion(
                    "A ____ is malware disguised as legitimate software that gives attackers backdoor access.",
                    new[] { "trojan", "trojan horse" },
                    Difficulty.Easy,
                    10,
                    "Trojans trick users into installing them by appearing as legitimate applications."),

                new FillInBlankQuestion(
                    "The practice of manipulating people into revealing confidential information is called ____.",
                    new[] { "social engineering", "social engineer" },
                    Difficulty.Medium,
                    25,
                    "Social engineering is a psychological attack that exploits human trust rather than technical vulnerabilities."),

                new FillInBlankQuestion(
                    "A ____ attack sends fraudulent emails impersonating a trusted source to steal credentials.",
                    new[] { "phishing", "phishing attack" },
                    Difficulty.Easy,
                    10,
                    "Phishing is one of the most common attack vectors for credential theft and malware distribution."),

                // Scenario Questions
                new ScenarioQuestion(
                    "You receive an email from your bank asking you to 'verify your account' by clicking a link. What should you do?",
                    new[] { "do not click the link and contact your bank directly", "ignore the email", "click the link to verify your account", "forward it to friends as a warning" },
                    0,
                    Difficulty.Easy,
                    10,
                    "This is a phishing attempt. Legitimate banks never ask you to verify credentials via email links. Always contact institutions directly through official channels."),

                new ScenarioQuestion(
                    "A colleague sends you a file via email titled 'Project_Report.exe'. What's your first action?",
                    new[] { "scan it with antivirus before opening", "open it immediately", "send it to others", "delete it without scanning" },
                    0,
                    Difficulty.Medium,
                    25,
                    "Executable files (.exe) from unexpected sources are high-risk. Always scan with antivirus or verify with the sender first."),

                new ScenarioQuestion(
                    "You notice your password has been compromised in a data breach. What should you do first?",
                    new[] { "change your password immediately and check for unauthorized account activity", "continue using the old password", "ignore it as it's just one account", "change it slowly over time" },
                    0,
                    Difficulty.Medium,
                    25,
                    "Act quickly! Change compromised passwords immediately and monitor accounts for unauthorized access. Use unique passwords across all important accounts.")
            };
        }
    }

    /// <summary>
    /// Base class for game questions with different types.
    /// </summary>
    public abstract class GameQuestion
    {
        public string QuestionText { get; set; }
        public Difficulty Difficulty { get; set; }
        public int PointsForCorrect { get; set; }
        public string Explanation { get; set; }
        public string CorrectAnswer { get; set; }

        public abstract string GetFormattedQuestion();
        public abstract bool CheckAnswer(string userAnswer);
    }

    /// <summary>
    /// Multiple choice question with 4 options.
    /// </summary>
    public class MultipleChoiceQuestion : GameQuestion
    {
        private string[] _options;
        private int _correctAnswerIndex;

        public MultipleChoiceQuestion(string question, string[] options, int correctIndex, Difficulty difficulty, int points, string explanation)
        {
            QuestionText = question;
            _options = options;
            _correctAnswerIndex = correctIndex;
            Difficulty = difficulty;
            PointsForCorrect = points;
            Explanation = explanation;
            CorrectAnswer = _options[correctIndex];
        }

        public override string GetFormattedQuestion()
        {
            string formatted = $"[{Difficulty}] {QuestionText}\n";
            for (int i = 0; i < _options.Length; i++)
            {
                formatted += $"  {char.ConvertFromUtf32(65 + i)}) {_options[i]}\n";
            }
            formatted += "Type A, B, C, or D:\n";
            return formatted;
        }

        public override bool CheckAnswer(string userAnswer)
        {
            userAnswer = userAnswer.Trim().ToUpperInvariant();
            int answerIndex = userAnswer.Length == 1 ? userAnswer[0] - 65 : -1;
            return answerIndex == _correctAnswerIndex;
        }

        public void ShuffleAnswers(Random random)
        {
            _options = _options.OrderBy(_ => random.Next()).ToArray();
            _correctAnswerIndex = Array.IndexOf(_options, _options[_correctAnswerIndex]);
        }
    }

    /// <summary>
    /// True/False question type.
    /// </summary>
    public class TrueFalseQuestion : GameQuestion
    {
        private bool _correctAnswerBool;

        public TrueFalseQuestion(string question, bool correctAnswer, Difficulty difficulty, int points, string explanation)
        {
            QuestionText = question;
            _correctAnswerBool = correctAnswer;
            CorrectAnswer = correctAnswer ? "True" : "False";
            Difficulty = difficulty;
            PointsForCorrect = points;
            Explanation = explanation;
        }

        public override string GetFormattedQuestion()
        {
            return $"[{Difficulty}] {QuestionText}\n" +
                   "Type 'True' or 'False':\n";
        }

        public override bool CheckAnswer(string userAnswer)
        {
            userAnswer = userAnswer.Trim().ToLowerInvariant();
            bool answer = userAnswer == "true" || userAnswer == "t" || userAnswer == "yes" || userAnswer == "y";
            return answer == _correctAnswerBool;
        }
    }

    /// <summary>
    /// Fill-in-the-blank question type with answer variations.
    /// </summary>
    public class FillInBlankQuestion : GameQuestion
    {
        private string[] _acceptableAnswers;

        public FillInBlankQuestion(string question, string[] acceptableAnswers, Difficulty difficulty, int points, string explanation)
        {
            QuestionText = question;
            _acceptableAnswers = acceptableAnswers;
            CorrectAnswer = acceptableAnswers[0];
            Difficulty = difficulty;
            PointsForCorrect = points;
            Explanation = explanation;
        }

        public override string GetFormattedQuestion()
        {
            return $"[{Difficulty}] {QuestionText}\n" +
                   "Type your answer:\n";
        }

        public override bool CheckAnswer(string userAnswer)
        {
            userAnswer = Regex.Replace(userAnswer.Trim().ToLowerInvariant(), @"\s+", " ");
            return _acceptableAnswers.Any(answer =>
                userAnswer == answer.ToLowerInvariant() ||
                userAnswer.Equals(answer.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Scenario-based question type.
    /// </summary>
    public class ScenarioQuestion : GameQuestion
    {
        private string[] _options;
        private int _correctAnswerIndex;

        public ScenarioQuestion(string scenario, string[] options, int correctIndex, Difficulty difficulty, int points, string explanation)
        {
            QuestionText = scenario;
            _options = options;
            _correctAnswerIndex = correctIndex;
            Difficulty = difficulty;
            PointsForCorrect = points;
            Explanation = explanation;
            CorrectAnswer = _options[correctIndex];
        }

        public override string GetFormattedQuestion()
        {
            string formatted = $"[SCENARIO - {Difficulty}] {QuestionText}\n";
            for (int i = 0; i < _options.Length; i++)
            {
                formatted += $"  {char.ConvertFromUtf32(65 + i)}) {_options[i]}\n";
            }
            formatted += "Type A, B, C, or D:\n";
            return formatted;
        }

        public override bool CheckAnswer(string userAnswer)
        {
            userAnswer = userAnswer.Trim().ToUpperInvariant();
            int answerIndex = userAnswer.Length == 1 ? userAnswer[0] - 65 : -1;
            return answerIndex == _correctAnswerIndex;
        }

        public void ShuffleAnswers(Random random)
        {
            _options = _options.OrderBy(_ => random.Next()).ToArray();
            _correctAnswerIndex = Array.IndexOf(_options, _options[_correctAnswerIndex]);
        }
    }

    /// <summary>
    /// Difficulty levels for questions.
    /// </summary>
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}