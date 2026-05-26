using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace REND
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _userName;
        private const int CenteredWidth = 50; // Width for centering ASCII art

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load saved user name from memory
            _userName = UserMemory.GetUserName();

            // ASCII-art header
            var header = new[]
            {
                "  ____  _____ _   _ ____  ",
                " |  _ \\| ____| \\ | |  _ \\ ",
                " | |_) |  _| |  \\| | | | |",
                " |  _ <| |___| |\\  | |_| |",
                " |_| \\_\\_____|_| \\_|____/ ",
                "",
                "REND - Cyber Security Guide",
                "",
                "Created by: J",
                ""
            };

            // Center and display ASCII art
            foreach (var line in header)
            {
                var centeredLine = CenterText(line);
                AppendRendLine(centeredLine);
            }

            // Intro: personalized if user name is known
            if (!string.IsNullOrEmpty(_userName))
            {
                RendWriteLine($"Welcome back, {_userName}! I'm Rend, your cyber security guide. Ask me anything about cyber security. Say 'Help' for topics or 'Bye' to exit.");
            }
            else
            {
                RendWriteLine("Hello! I'm Rend, your cyber security guide. What's your name?");
            }
        }

        /// <summary>
        /// Centers text by adding padding to the left.
        /// </summary>
        private string CenterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            int padding = Math.Max(0, (CenteredWidth - text.Length) / 2);
            return new string(' ', padding) + text;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput(txtInput.Text);
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ProcessInput(txtInput.Text);
            }
        }

        private void ProcessInput(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return;
            }

            var input = raw.Trim();
            AppendUserLine(input);
            txtInput.Clear();

            // If user name not set, try to capture it
            if (string.IsNullOrEmpty(_userName))
            {
                if (!IsCommonGreeting(input) && !IsCyberSecurityKeyword(input))
                {
                    _userName = input;
                    UserMemory.SaveUserName(_userName);
                    RendWriteLine($"Nice to meet you, {_userName}! I'm Rend, your cyber security guide. Ask me anything about cyber security. Say 'Help' for topics or 'Bye' to exit.");
                    return;
                }
            }

            // Exit conditions
            if (input.Equals("bye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("goodbye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                var byeResponse = RendKnowledgeBase.GetRandomResponse("Bye");
                if (byeResponse != null && !string.IsNullOrEmpty(_userName))
                {
                    RendWriteLine($"{byeResponse} Have a great day, {_userName}!");
                }
                else if (byeResponse != null)
                {
                    RendWriteLine(byeResponse);
                }
                txtInput.IsEnabled = false;
                btnSend.IsEnabled = false;
                return;
            }

            // Handle memory-related commands
            if (input.Equals("forget me", StringComparison.OrdinalIgnoreCase))
            {
                UserMemory.ClearUserMemory();
                _userName = null;
                RendWriteLine("I've cleared your information from my memory. Feel free to introduce yourself again anytime!");
                return;
            }

            if (input.Equals("who am I", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("who am i", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(_userName))
                {
                    RendWriteLine($"You're {_userName}! I remember you from our previous conversation.");
                }
                else
                {
                    RendWriteLine("I don't have your name saved yet. What should I call you?");
                }
                return;
            }

            // Exact match (case-insensitive)
            var response = RendKnowledgeBase.GetRandomResponse(input);
            if (response != null)
            {
                RendWriteLine(response);
                return;
            }

            // Fallback: try to find any key appearing inside the input (prefer longer keys first)
            string foundKey = null;
            foreach (var key in RendKnowledgeBase.GetKeys().OrderByDescending(k => k.Length))
            {
                if (!string.IsNullOrWhiteSpace(key) && input.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundKey = key;
                    break;
                }
            }

            if (foundKey != null)
            {
                var fallbackResponse = RendKnowledgeBase.GetRandomResponse(foundKey);
                if (fallbackResponse != null)
                {
                    RendWriteLine(fallbackResponse);
                }
            }
            else
            {
                RendWriteLine("I'm not sure I understand. Try asking about \"Cyber Security\", \"Types\", \"Safe Browsing\", or say \"Hello\".");
            }
        }

        /// <summary>
        /// Checks if the input is a common greeting.
        /// </summary>
        private bool IsCommonGreeting(string input)
        {
            var greetings = new[] { "hi", "hello", "hey", "greetings", "howdy", "help", "thanks" };
            return greetings.Any(g => input.Equals(g, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the input is a known cyber security keyword.
        /// </summary>
        private bool IsCyberSecurityKeyword(string input)
        {
            var keywords = RendKnowledgeBase.GetKeys().ToList();
            return keywords.Any(k => input.Equals(k, StringComparison.OrdinalIgnoreCase));
        }

        // Helper to append Rend lines with "Rend: " prefix
        private void RendWriteLine(string message)
        {
            AppendRendLine("Rend: " + message);
        }

        private void AppendRendLine(string text)
        {
            tbConversation.AppendText(text + Environment.NewLine);
            tbConversation.ScrollToEnd();
        }

        private void AppendUserLine(string text)
        {
            tbConversation.AppendText("You: " + text + Environment.NewLine);
            tbConversation.ScrollToEnd();
        }
    }
}