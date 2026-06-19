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
    /// Manages cybersecurity tasks with database persistence.
    /// </summary>
    public class TaskManager
    {
        // Connection String
        string connectionString = "Server=LabVM2049939\\SQLEXPRESS;Database=REND;Trusted_Connection=True;TrustServerCertificate=True;";

        public TaskManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Gets the next available TaskID from the database.
        /// </summary>
        /// <returns>The next TaskID</returns>
        private int GetNextTaskId()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ISNULL(MAX(TaskID), 0) + 1 FROM Tasks";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Saves a new task to the database. The TaskID is auto-generated.
        /// </summary>
        /// <param name="TaskName">The name of the task</param>
        /// <param name="Description">A description of the task</param>
        /// <param name="ReminderDate">The date to remind the user about the task</param>
        /// <param name="IsCompleted">Whether the task is completed</param>
        /// <returns>true if the task was saved successfully; otherwise false</returns>
        public bool SaveTask(string TaskName, string Description, DateTime ReminderDate, bool IsCompleted = false)
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                MessageBox.Show("Task name cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Task description cannot be empty.");
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int taskId = GetNextTaskId();

                    string query = "INSERT INTO Tasks (TaskID, UserId, TaskName, Description, ReminderDate, IsCompleted) " +
                                   "VALUES (@TaskID, @UserId, @TaskName, @Description, @ReminderDate, @IsCompleted)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", taskId);

                        // ✅ FIX: never send NULL into NOT NULL column
                        command.Parameters.AddWithValue("@UserId", 1);

                        command.Parameters.AddWithValue("@TaskName", TaskName);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@ReminderDate", ReminderDate);
                        command.Parameters.AddWithValue("@IsCompleted", IsCompleted);

                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Saves a new task to the database with automatic timestamp.
        /// </summary>
        /// <param name="taskName">The name of the task</param>
        /// <param name="description">A description of the task</param>
        /// <returns>true if the task was saved successfully; otherwise false</returns>
        public bool SaveTask(string taskName, string description)
        {
            return SaveTask(taskName, description, DateTime.Now, false);
        }

        /// <summary>
        /// Saves a new task with TaskID and UserID to the database.
        /// </summary>
        /// <param name="TaskID">The ID of the task</param>
        /// <param name="UserID">The ID of the user who owns the task</param>
        /// <param name="TaskName">The name of the task</param>
        /// <param name="Description">A description of the task</param>
        /// <param name="ReminderDate">The date to remind the user about the task</param>
        /// <param name="IsCompleted">Whether the task is completed</param>
        /// <returns>true if the task was saved successfully; otherwise false</returns>
        public bool SaveTaskWithIds(int TaskID, int UserID, string TaskName, string Description, DateTime ReminderDate, bool IsCompleted = false)
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                MessageBox.Show("Task name cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Task description cannot be empty.");
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Tasks (TaskID, UserId, TaskName, Description, ReminderDate, IsCompleted) " +
                                   "VALUES (@TaskID, @UserId, @TaskName, @Description, @ReminderDate, @IsCompleted)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", TaskID);
                        command.Parameters.AddWithValue("@UserId", UserID);
                        command.Parameters.AddWithValue("@TaskName", TaskName);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@ReminderDate", ReminderDate);
                        command.Parameters.AddWithValue("@IsCompleted", IsCompleted);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Saves a new task with UserID to the database.
        /// </summary>
        /// <param name="UserID">The ID of the user who owns the task</param>
        /// <param name="TaskName">The name of the task</param>
        /// <param name="Description">A description of the task</param>
        /// <param name="ReminderDate">The date to remind the user about the task</param>
        /// <param name="IsCompleted">Whether the task is completed</param>
        /// <returns>true if the task was saved successfully; otherwise false</returns>
        public bool SaveTaskWithUserId(int UserID, string TaskName, string Description, DateTime ReminderDate, bool IsCompleted = false)
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                MessageBox.Show("Task name cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Task description cannot be empty.");
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int taskId = GetNextTaskId();
                    string query = "INSERT INTO Tasks (TaskID, UserId, TaskName, Description, ReminderDate, IsCompleted) " +
                                   "VALUES (@TaskID, @UserId, @TaskName, @Description, @ReminderDate, @IsCompleted)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", taskId);
                        command.Parameters.AddWithValue("@UserId", UserID);
                        command.Parameters.AddWithValue("@TaskName", TaskName);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@ReminderDate", ReminderDate);
                        command.Parameters.AddWithValue("@IsCompleted", IsCompleted);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Saves multiple tasks with UserID in a batch operation.
        /// </summary>
        /// <param name="UserID">The ID of the user who owns the tasks</param>
        /// <param name="Tasks">A list of tuples containing (TaskName, Description) for each task</param>
        /// <returns>The number of tasks successfully saved</returns>
        public int SaveMultipleTasksWithUserId(int UserID, List<(string TaskName, string Description)> Tasks)
        {
            if (Tasks == null || Tasks.Count == 0)
            {
                MessageBox.Show("No tasks to save.");
                return 0;
            }

            int successCount = 0;

            foreach (var task in Tasks)
            {
                if (SaveTaskWithUserId(UserID, task.TaskName, task.Description, DateTime.Now, false))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Saves multiple tasks to the database in a batch operation.
        /// </summary>
        /// <param name="Tasks">A list of tuples containing (TaskName, Description) for each task</param>
        /// <returns>The number of tasks successfully saved</returns>
        public int SaveMultipleTasks(List<(string TaskName, string Description)> Tasks)
        {
            if (Tasks == null || Tasks.Count == 0)
            {
                MessageBox.Show("No tasks to save.");
                return 0;
            }

            int successCount = 0;

            foreach (var task in Tasks)
            {
                if (SaveTask(task.TaskName, task.Description))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Saves multiple tasks to the database in a batch operation with full details.
        /// </summary>
        /// <param name="Tasks">A list of tuples containing (TaskName, Description, ReminderDate, IsCompleted) for each task</param>
        /// <returns>The number of tasks successfully saved</returns>
        public int SaveMultipleTasks(List<(string TaskName, string Description, DateTime ReminderDate, bool IsCompleted)> Tasks)
        {
            if (Tasks == null || Tasks.Count == 0)
            {
                MessageBox.Show("No tasks to save.");
                return 0;
            }

            int successCount = 0;

            foreach (var task in Tasks)
            {
                if (SaveTask(task.TaskName, task.Description, task.ReminderDate, task.IsCompleted))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Retrieves all tasks from the database as a formatted list.
        /// </summary>
        /// <returns>A list of all saved tasks</returns>
        public List<string> GetAllTasks()
        {
            List<string> Tasks = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT TaskID, TaskName, Description, ReminderDate, IsCompleted FROM Tasks ORDER BY TaskID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int TaskId = reader.GetInt32(0);
                                string TaskName = reader.GetString(1);
                                string Description = reader.GetString(2);
                                DateTime ReminderDate = reader.GetDateTime(3);
                                bool IsCompleted = Convert.ToInt32(reader[4]) == 1;
                                Tasks.Add($"[ID: {TaskId}] {TaskName} - {Description} (Due: {ReminderDate:yyyy-MM-dd}) - Completed: {IsCompleted}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving tasks: {ex.Message}");
            }

            return Tasks;
        }

        /// <summary>
        /// Retrieves all tasks for a specific user from the database as a formatted list.
        /// </summary>
        /// <param name="UserId">The ID of the user</param>
        /// <returns>A list of all tasks for the specified user</returns>
        public List<string> GetTasksByUserId(int UserId)
        {
            List<string> Tasks = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT TaskID, TaskName, Description, ReminderDate, IsCompleted FROM Tasks WHERE UserId = @UserId ORDER BY TaskID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", UserId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int TaskId = reader.GetInt32(0);
                                string TaskName = reader.GetString(1);
                                string Description = reader.GetString(2);
                                DateTime ReminderDate = reader.GetDateTime(3);
                                bool IsCompleted = Convert.ToInt32(reader[4]) == 1;
                                Tasks.Add($"[ID: {TaskId}] {TaskName} - {Description} (Due: {ReminderDate:yyyy-MM-dd}) - Completed: {IsCompleted}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving tasks: {ex.Message}");
            }

            return Tasks;
        }

        /// <summary>
        /// Retrieves tasks from the database as a Dictionary with TaskID as key.
        /// Useful for programmatic access without formatting.
        /// </summary>
        /// <returns>A dictionary of TaskID and task details</returns>
        public Dictionary<int, (string name, string description, DateTime reminderDate, bool isCompleted)> GetTasksAsDictionary()
        {
            var tasks = new Dictionary<int, (string, string, DateTime, bool)>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT TaskID, TaskName, Description, ReminderDate, IsCompleted FROM Tasks ORDER BY TaskID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int taskId = reader.GetInt32(0);
                                string taskName = reader.GetString(1);
                                string description = reader.GetString(2);
                                DateTime reminderDate = reader.GetDateTime(3);
                                bool IsCompleted = Convert.ToInt32(reader[4]) == 1;

                                tasks[taskId] = (taskName, description, reminderDate, IsCompleted);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving tasks: {ex.Message}");
            }

            return tasks;
        }

        /// <summary>
        /// Deletes a task from the database by TaskID. Silent mode suppresses MessageBox.
        /// </summary>
        /// <param name="TaskID">The ID of the task to delete</param>
        /// <param name="silent">If true, suppresses MessageBox notifications</param>
        /// <returns>true if the task was deleted successfully; otherwise false</returns>
        public bool DeleteTask(int TaskID, bool silent = false)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Tasks WHERE TaskID = @TaskID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", TaskID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            if (!silent)
                                MessageBox.Show($"Task {TaskID} deleted successfully.");
                            return true;
                        }
                        else
                        {
                            if (!silent)
                                MessageBox.Show($"Task with ID {TaskID} not found.");
                            return false;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes multiple tasks from the database by their TaskIDs. Silent mode suppresses MessageBox.
        /// </summary>
        /// <param name="TaskIds">A list of TaskIDs to delete</param>
        /// <param name="silent">If true, suppresses MessageBox notifications</param>
        /// <returns>The number of tasks successfully deleted</returns>
        public int DeleteMultipleTasks(List<int> TaskIds, bool silent = false)
        {
            if (TaskIds == null || TaskIds.Count == 0)
            {
                if (!silent)
                    MessageBox.Show("No tasks to delete.");
                return 0;
            }

            int successCount = 0;

            foreach (var taskId in TaskIds)
            {
                if (DeleteTask(taskId, silent: true))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Checks if a task exists by TaskID.
        /// </summary>
        /// <param name="TaskID">The ID of the task to check</param>
        /// <returns>true if the task exists; otherwise false</returns>
        public bool TaskExists(int TaskID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Tasks WHERE TaskID = @TaskID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", TaskID);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking task: {ex.Message}");
                return false;
            }
        }
        private bool ReadBool(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            if (value is bool b)
                return b;

            return Convert.ToInt32(value) == 1;
        }
    }
}