using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace REND
{
    /// <summary>
    /// Manages activity logging for tracking the last 10 significant user actions.
    /// </summary>
    public static class ActivityLog
    {
        private static readonly string _logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "REND",
            "activity_log.json");

        private const int MaxActivities = 10;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private static List<ActivityEntry> _activities = new();

        /// <summary>
        /// Logs a significant user action.
        /// </summary>
        public static void LogAction(ActionType type, string details)
        {
            try
            {
                LoadActivities();

                var entry = new ActivityEntry
                {
                    Timestamp = DateTime.Now,
                    ActionType = type,
                    Details = details
                };

                _activities.Add(entry);

                // Keep only the last 10 activities
                if (_activities.Count > MaxActivities)
                {
                    _activities = _activities.Skip(_activities.Count - MaxActivities).ToList();
                }

                SaveActivities();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging action: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the last 10 logged activities.
        /// </summary>
        public static List<ActivityEntry> GetLastActivities()
        {
            try
            {
                LoadActivities();
                return new List<ActivityEntry>(_activities);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving activities: {ex.Message}");
                return new List<ActivityEntry>();
            }
        }

        /// <summary>
        /// Clears all activity logs.
        /// </summary>
        public static void ClearActivityLog()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    File.Delete(_logFilePath);
                }
                _activities = new List<ActivityEntry>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing activity log: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats activity log for display.
        /// </summary>
        public static string GetFormattedLog()
        {
            var activities = GetLastActivities();
            if (activities.Count == 0)
            {
                return "No activities recorded yet.";
            }

            var log = new System.Text.StringBuilder();
            log.AppendLine("=== Activity Log (Last 10 Actions) ===");
            for (int i = activities.Count - 1; i >= 0; i--)
            {
                var activity = activities[i];
                log.AppendLine($"{i + 1}. [{activity.Timestamp:yyyy-MM-dd HH:mm:ss}] {activity.ActionType}: {activity.Details}");
            }
            return log.ToString();
        }

        /// <summary>
        /// Loads activities from disk.
        /// </summary>
        private static void LoadActivities()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    var json = File.ReadAllText(_logFilePath);
                    _activities = JsonSerializer.Deserialize<List<ActivityEntry>>(json, _jsonOptions) ?? new();
                }
                else
                {
                    _activities = new();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading activities: {ex.Message}");
                _activities = new();
            }
        }

        /// <summary>
        /// Saves activities to disk.
        /// </summary>
        private static void SaveActivities()
        {
            try
            {
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(_activities, _jsonOptions);
                File.WriteAllText(_logFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving activities: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Represents a single activity entry in the log.
    /// </summary>
    public class ActivityEntry
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("actionType")]
        public ActionType ActionType { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }

    /// <summary>
    /// Enum for significant user action types.
    /// </summary>
    public enum ActionType
    {
        UserNameSet,
        UserNameCleared,
        GameStarted,
        GameQuit,
        GameCompleted,
        TopicQueried,
        HelpRequested,
        SessionEnded,
        TaskAdded
    }
}