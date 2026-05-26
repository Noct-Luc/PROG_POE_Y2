using System;
using System.IO;
using System.Text.Json;

namespace REND
{
    /// <summary>
    /// Handles saving and loading user memory (name and preferences).
    /// </summary>
    public static class UserMemory
    {
        private static readonly string _memoryFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "REND",
            "user_memory.json");

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private static UserData _currentUser;

        /// <summary>
        /// Gets the current user's name, or null if not set.
        /// </summary>
        public static string GetUserName()
        {
            if (_currentUser == null)
            {
                LoadUserMemory();
            }
            return _currentUser?.Name;
        }

        /// <summary>
        /// Saves the user's name to memory.
        /// </summary>
        public static void SaveUserName(string name)
        {
            if (_currentUser == null)
            {
                _currentUser = new UserData();
            }

            _currentUser.Name = name;
            _currentUser.LastSeen = DateTime.Now;

            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(_memoryFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serialize and save
                var json = JsonSerializer.Serialize(_currentUser, _jsonOptions);
                File.WriteAllText(_memoryFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving user memory: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads user memory from disk.
        /// </summary>
        private static void LoadUserMemory()
        {
            try
            {
                if (File.Exists(_memoryFilePath))
                {
                    var json = File.ReadAllText(_memoryFilePath);
                    _currentUser = JsonSerializer.Deserialize<UserData>(json, _jsonOptions);
                }
                else
                {
                    _currentUser = new UserData();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user memory: {ex.Message}");
                _currentUser = new UserData();
            }
        }

        /// <summary>
        /// Clears the user's memory.
        /// </summary>
        public static void ClearUserMemory()
        {
            try
            {
                if (File.Exists(_memoryFilePath))
                {
                    File.Delete(_memoryFilePath);
                }
                _currentUser = new UserData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing user memory: {ex.Message}");
            }
        }

        /// <summary>
        /// Internal class to represent user data.
        /// </summary>
        private class UserData
        {
            public string Name { get; set; }
            public DateTime LastSeen { get; set; }
        }
    }
}