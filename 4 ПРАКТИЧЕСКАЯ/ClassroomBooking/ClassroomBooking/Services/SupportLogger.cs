using System;
using System.IO;

namespace ClassroomBooking.Services
{
    /// <summary>
    /// Простой журнал событий приложения.
    /// Файл хранится в %LocalAppData%\ClassroomBooking\Logs\application.log,
    /// потому что обычный пользователь имеет право писать в свой профиль.
    /// </summary>
    public static class SupportLogger
    {
        private static string LogDirectory
        {
            get
            {
                string root = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(root, "ClassroomBooking", "Logs");
            }
        }

        public static string LogFilePath
        {
            get { return Path.Combine(LogDirectory, "application.log"); }
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Warning(string message)
        {
            Write("WARNING", message);
        }

        public static void Error(string message, Exception ex)
        {
            Write("ERROR", message + Environment.NewLine + ex.ToString());
        }

        private static void Write(string level, string message)
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);
                string line = string.Format(
                    "{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}{3}",
                    DateTime.Now, level, message, Environment.NewLine);
                File.AppendAllText(LogFilePath, line);
            }
            catch
            {
                // Ошибка записи журнала не должна закрывать приложение.
            }
        }
    }
}
