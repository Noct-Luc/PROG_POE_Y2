using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace REND
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _userName;
        private const int CenteredWidth = 50; // Width for centering ASCII art
        private RendMinigame _minigame;
        private TaskManager _taskManager;
        private int _nextTaskId = 1;

        // Task creation state management
        private bool _isAddingTask = false;
        private string _currentTaskName = null;
        private string _currentTaskDescription = null;

        // Task deletion state management
        private bool _isDeletingTask = false;
        private List<int> _taskIdsToDelete = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _minigame = new RendMinigame();

            // Initialize TaskManager with connection string
            string connectionString = "Server=LabVM2049939\\SQLEXPRESS;Database=REND;Trusted_Connection=True;TrustServerCertificate=True;";
            _taskManager = new TaskManager(connectionString);
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
                RendWriteLine($"Welcome back, {_userName}! I'm Rend, your cyber security guide. Ask me anything about cyber security. Say 'Help' for topics, 'Add a Task' to create a new task, or 'Bye' to exit.");
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

            // Check if user is adding a task
            if (_isAddingTask)
            {
                HandleTaskCreationInput(input);
                return;
            }

            // Check if user is deleting a task
            if (_isDeletingTask)
            {
                HandleTaskDeletionInput(input);
                return;
            }

            // Check if user is playing minigame
            if (_minigame.IsGameActive)
            {
                HandleMinigameInput(input);
                return;
            }

            // If user name not set, try to capture it
            if (string.IsNullOrEmpty(_userName))
            {
                if (!IsCommonGreeting(input) && !IsCyberSecurityKeyword(input) && !IsTaskCommand(input) && !IsDeleteTaskCommand(input))
                {
                    _userName = input;
                    UserMemory.SaveUserName(_userName);
                    ActivityLog.LogAction(ActionType.UserNameSet, $"User identified as '{_userName}'");
                    RendWriteLine($"Nice to meet you, {_userName}! I'm Rend, your cyber security guide. Ask me anything about cyber security. Say 'Help' for topics, 'Add a Task' to create a new task, or 'Bye' to exit.");
                    return;
                }
            }

            // Check for task-related commands (case-insensitive)
            // Check for task-related commands (case-insensitive)
            if (IsTaskCommand(input))
            {
                HandleTaskCommand(input);
                return;
            }

            // Show tasks command
            if (input.Contains("show tasks", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("show task", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("view tasks", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("display tasks", StringComparison.OrdinalIgnoreCase))
            {
                HandleShowTasksCommand();
                return;
            }

            // Check for delete task commands (case-insensitive)
            if (IsDeleteTaskCommand(input))
            {
                HandleDeleteTaskCommand(input);
                return;
            }

            // Check for minigame activation (case-insensitive, ignore punctuation)
            if (IsMinigameActivation(input))
            {
                ActivityLog.LogAction(ActionType.GameStarted, "User started the minigame");
                string gameStartMessage = _minigame.StartGame();
                RendWriteLine(gameStartMessage);
                RendWriteLine(_minigame.GetNextQuestion());
                return;
            }

            // Exit conditions
            if (input.Equals("bye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("goodbye", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                ActivityLog.LogAction(ActionType.SessionEnded, $"Session ended for user '{_userName}'");
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
                ActivityLog.LogAction(ActionType.UserNameCleared, "User cleared memory");
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

            // Handle activity log display
            if (input.Equals("show log", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("activity log", StringComparison.OrdinalIgnoreCase))
            {
                var logContent = ActivityLog.GetFormattedLog();
                RendWriteLine(logContent);
                return;
            }

            // Exact match (case-insensitive)
            var response = RendKnowledgeBase.GetRandomResponse(input);
            if (response != null)
            {
                ActivityLog.LogAction(ActionType.TopicQueried, $"User queried about '{input}'");
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
                ActivityLog.LogAction(ActionType.TopicQueried, $"User queried about '{foundKey}'");
                var fallbackResponse = RendKnowledgeBase.GetRandomResponse(foundKey);
                if (fallbackResponse != null)
                {
                    RendWriteLine(fallbackResponse);
                }
            }
            else
            {
                RendWriteLine("I'm not sure I understand. Try asking about \"Cyber Security\", \"Types\", \"Safe Browsing\", \"Add a Task\", \"Delete Task\", or say \"Hello\".");
            }
        }

        /// <summary>
        /// Checks if the input contains a task-related command.
        /// </summary>
        private bool IsTaskCommand(string input)
        {
            return input.Contains("add a task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("add task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("create task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("new task", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the input contains a delete task command.
        /// </summary>
        private bool IsDeleteTaskCommand(string input)
        {
            return input.Contains("delete task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("remove task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("delete a task", StringComparison.OrdinalIgnoreCase) ||
                   input.Contains("remove a task", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Handles task-related commands.
        /// </summary>
        private void HandleTaskCommand(string input)
        {
            if (input.Contains("add a task", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("add task", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("create task", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("new task", StringComparison.OrdinalIgnoreCase))
            {
                StartTaskCreation();
            }
        }

        /// <summary>
        /// Handles delete task commands.
        /// </summary>
        private void HandleDeleteTaskCommand(string input)
        {
            if (IsDeleteTaskCommand(input))
            {
                StartTaskDeletion();
            }
        }

        /// <summary>
        /// Initiates the task creation workflow.
        /// </summary>
        private void StartTaskCreation()
        {
            _isAddingTask = true;
            _currentTaskName = null;
            _currentTaskDescription = null;

            ActivityLog.LogAction(ActionType.TaskAdded, "User started creating a new task");
            RendWriteLine("Great! Let's create a new security task. What would you like to name this task? (e.g., 'Update Security Protocols')");
        }

        /// <summary>
        /// Handles input during the task creation process.
        /// </summary>
        private void HandleTaskCreationInput(string input)
        {
            // Step 1: Get task name
            if (_currentTaskName == null)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    RendWriteLine("Please provide a task name.");
                    return;
                }

                _currentTaskName = input;
                RendWriteLine($"Got it! The task name is '{_currentTaskName}'. Now, please provide a description for this task. What should be done?");
                return;
            }

            // Step 2: Get task description
            if (_currentTaskDescription == null)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    RendWriteLine("Please provide a task description.");
                    return;
                }

                _currentTaskDescription = input;

                // Step 3: Confirm and save the task
                RendWriteLine($"Perfect! Let me save your task:\n- Task Name: {_currentTaskName}\n- Description: {_currentTaskDescription}");
                SaveTaskToDatabase(_currentTaskName, _currentTaskDescription);

                // Reset task creation state
                _isAddingTask = false;
                _currentTaskName = null;
                _currentTaskDescription = null;

                RendWriteLine("Your task has been successfully saved to the database! Is there anything else you'd like to do?");
                return;
            }
        }

        /// <summary>
        /// Initiates the task deletion workflow.
        /// </summary>
        private void StartTaskDeletion()
        {
            _isDeletingTask = true;
            _taskIdsToDelete.Clear();

            // Display all tasks for user to choose from
            List<string> allTasks = _taskManager.GetAllTasks();

            if (allTasks.Count == 0)
            {
                RendWriteLine("You don't have any tasks to delete.");
                _isDeletingTask = false;
                return;
            }

            ActivityLog.LogAction(ActionType.TaskAdded, "User started deleting a task");
            RendWriteLine("Here are your current tasks:\n" + string.Join("\n", allTasks));
            RendWriteLine("\nWhich task would you like to delete? Please provide the TaskID number (or type 'cancel' to go back).");
        }

        /// <summary>
        /// Handles input during the task deletion process.
        /// </summary>
        private void HandleTaskDeletionInput(string input)
        {
            if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
            {
                _isDeletingTask = false;
                _taskIdsToDelete.Clear();
                RendWriteLine("Task deletion cancelled. Is there anything else you'd like to do?");
                return;
            }

            if (int.TryParse(input, out int taskId))
            {
                if (_taskManager.TaskExists(taskId))
                {
                    _taskIdsToDelete.Add(taskId);
                    RendWriteLine($"Task {taskId} marked for deletion. Add another TaskID or type 'confirm' to delete.");
                }
                else
                {
                    RendWriteLine($"Task {taskId} not found. Please enter a valid TaskID.");
                }
            }
            else if (input.Equals("confirm", StringComparison.OrdinalIgnoreCase))
            {
                if (_taskIdsToDelete.Count == 0)
                {
                    RendWriteLine("No tasks selected. Please select at least one TaskID.");
                    return;
                }

                int deletedCount = _taskManager.DeleteMultipleTasks(_taskIdsToDelete, silent: true);
                RendWriteLine($"Successfully deleted {deletedCount} task(s)!");

                _isDeletingTask = false;
                _taskIdsToDelete.Clear();
            }
            else
            {
                RendWriteLine("Enter a TaskID, 'confirm' to delete, or 'cancel' to go back.");
            }
        }

        /// <summary>
        /// Saves a task to the database using the TaskManager.
        /// </summary>
        /// <param name="taskName">The name of the task</param>
        /// <param name="description">The description of the task</param>
        private void SaveTaskToDatabase(string taskName, string description)
        {
            try
            {
                bool success = _taskManager.SaveTask(taskName, description, DateTime.Now, false);

                if (success)
                {
                    _nextTaskId++;
                    ActivityLog.LogAction(ActionType.TaskAdded, $"Task '{taskName}' successfully saved to database");
                }
                else
                {
                    RendWriteLine("There was an issue saving your task. Please try again.");
                }
            }
            catch (Exception ex)
            {
                RendWriteLine($"Error saving task: {ex.Message}");
                ActivityLog.LogAction(ActionType.TaskAdded, $"Error saving task '{taskName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Handles input while the minigame is active.
        /// </summary>
        private void HandleMinigameInput(string input)
        {
            // Check for quit commands
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit game", StringComparison.OrdinalIgnoreCase))
            {
                ActivityLog.LogAction(ActionType.GameQuit, "User quit the minigame");
                string quitMessage = _minigame.QuitGame();
                RendWriteLine(quitMessage);
                return;
            }

            // Process answer
            string resultMessage = _minigame.ProcessAnswer(input);
            RendWriteLine(resultMessage);

            // Check if game is completed
            if (!_minigame.IsGameActive)
            {
                ActivityLog.LogAction(ActionType.GameCompleted, "User completed the minigame");
            }
        }

        /// <summary>
        /// Checks if the input is a minigame activation command.
        /// Case-insensitive and ignores punctuation.
        /// </summary>
        private bool IsMinigameActivation(string input)
        {
            // Remove punctuation and convert to lowercase
            string normalized = Regex.Replace(input.ToLowerInvariant(), @"[^\w\s]", "");
            return normalized == "lets play a game" || normalized == "let's play a game";
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

        private void HandleShowTasksCommand()
        {
            try
            {
                List<string> tasks = _taskManager.GetAllTasks();

                if (tasks == null || tasks.Count == 0)
                {
                    RendWriteLine("You don't have any tasks saved yet.");
                    return;
                }

                RendWriteLine("Here are your current tasks:");

                foreach (var task in tasks)
                {
                    AppendRendLine(task);
                }
            }
            catch (Exception ex)
            {
                RendWriteLine($"Error retrieving tasks: {ex.Message}");
            }
        }
    }
}