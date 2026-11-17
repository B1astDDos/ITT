using System;
using System.IO;
using System.Text;
using System.Windows;
using ClassroomBooking.Services;

namespace ClassroomBooking
{
    /// <summary>
    /// Окно отправки обращения пользователя.
    /// Обращение сохраняется в %LocalAppData%\ClassroomBooking\SupportRequests.
    /// </summary>
    public partial class SupportRequestWindow : Window
    {
        private readonly string appVersion;
        private readonly string logPath;

        public SupportRequestWindow(string version, string logFilePath)
        {
            InitializeComponent();
            appVersion = version;
            logPath = logFilePath;
        }

        private void SubmitButton_Click(
            object sender, RoutedEventArgs e)
        {
            string subject = SubjectTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(subject) ||
                string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show(
                    "Заполните тему и описание проблемы.",
                    "Не все поля заполнены",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string folder = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "ClassroomBooking",
                    "SupportRequests");

                Directory.CreateDirectory(folder);

                string ticketId =
                    "SUP-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");

                string filePath = Path.Combine(
                    folder, ticketId + ".txt");

                string report =
                    "ID: " + ticketId + Environment.NewLine +
                    "Дата: " + DateTime.Now + Environment.NewLine +
                    "Версия: " + appVersion + Environment.NewLine +
                    "Тема: " + subject + Environment.NewLine +
                    "Описание: " + description + Environment.NewLine +
                    "Журнал: " + logPath + Environment.NewLine;

                File.WriteAllText(filePath, report, Encoding.UTF8);

                SupportLogger.Info(
                    "Создано обращение пользователя: " + ticketId);

                MessageBox.Show(
                    "Обращение сохранено: " + ticketId,
                    "Служба поддержки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                SupportLogger.Error(
                    "Не удалось сохранить обращение пользователя.", ex);

                MessageBox.Show(
                    "Не удалось сохранить обращение. Обратитесь в поддержку.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(
            object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
