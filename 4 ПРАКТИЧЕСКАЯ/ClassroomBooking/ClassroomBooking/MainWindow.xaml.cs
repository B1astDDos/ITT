using System;
using System.Windows;
using ClassroomBooking.Configuration;
using ClassroomBooking.Models;
using ClassroomBooking.Services;

namespace ClassroomBooking
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ConfigService configService = new ConfigService();

        private AppSettings settings;

        private readonly UserService userService = new UserService();
        private readonly RoomService roomService = new RoomService();
        private readonly BookingService bookingService = new BookingService();

        public MainWindow()
        {
            InitializeComponent();

            settings = configService.Load();

            Title = settings.AppTitle;
            StatusTextBlock.Text = settings.StatusMessage;

            if (settings.AdminMode)
                StatusTextBlock.Text = "Режим администратора";

            SupportLogger.Info("Запуск приложения. Режим: " + settings.StatusMessage + "; Время: " + settings.WorkDayStart + "-" + settings.WorkDayEnd);

            LoadData();
        }

        private void LoadData()
        {
            UserComboBox.ItemsSource = userService.GetAll();
            RoomComboBox.ItemsSource = roomService.GetAll();
            DatePicker.SelectedDate = DateTime.Today;
            StartTimeTextBox.Text = "10:00";
            EndTimeTextBox.Text = "11:00";

            RefreshBookings();
        }

        private void RefreshBookings()
        {
            BookingsGrid.ItemsSource = null;
            BookingsGrid.ItemsSource = bookingService.GetAll();
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            SupportLogger.Info("Пользователь начал создание бронирования.");

            if (!(UserComboBox.SelectedItem is User user))
            {
                SupportLogger.Warning("Бронирование отклонено: пользователь не выбран.");
                StatusTextBlock.Text = "Выберите пользователя.";
                return;
            }

            if (!(RoomComboBox.SelectedItem is Room room))
            {
                SupportLogger.Warning("Бронирование отклонено: аудитория не выбрана.");
                StatusTextBlock.Text = "Выберите аудиторию.";
                return;
            }

            if (!DatePicker.SelectedDate.HasValue)
            {
                SupportLogger.Warning("Бронирование отклонено: дата не выбрана.");
                StatusTextBlock.Text = "Выберите дату.";
                return;
            }

            DateTime bookingDate = DatePicker.SelectedDate.Value.Date;

            if (!settings.AllowPastDates && bookingDate < DateTime.Today)
            {
                SupportLogger.Warning("Бронирование отклонено: указана прошедшая дата.");
                StatusTextBlock.Text = "Нельзя бронировать прошедшую дату.";
                return;
            }

            TimeSpan startTime;
            TimeSpan endTime;

            if (!TimeSpan.TryParse(StartTimeTextBox.Text, out startTime) ||
                !TimeSpan.TryParse(EndTimeTextBox.Text, out endTime))
            {
                SupportLogger.Warning("Бронирование отклонено: неверный формат времени.");
                StatusTextBlock.Text = "Проверьте формат времени (ЧЧ:ММ).";
                return;
            }

            if (endTime <= startTime)
            {
                SupportLogger.Warning("Бронирование отклонено: время окончания раньше времени начала.");
                StatusTextBlock.Text = "Время окончания должно быть позже времени начала.";
                return;
            }

            if (!settings.AllowUnavailableRooms && !room.IsAvailable)
            {
                SupportLogger.Warning("Бронирование отклонено: аудитория недоступна. RoomId=" + room.Id);
                StatusTextBlock.Text = "Аудитория недоступна для бронирования.";
                return;
            }

            TimeSpan workStart;
            TimeSpan workEnd;

            if (!TimeSpan.TryParse(settings.WorkDayStart, out workStart) ||
                !TimeSpan.TryParse(settings.WorkDayEnd, out workEnd))
            {
                SupportLogger.Warning("Ошибка конфигурации рабочего времени. WorkDayStart=" + settings.WorkDayStart + "; WorkDayEnd=" + settings.WorkDayEnd);
                StatusTextBlock.Text =
                    "Ошибка конфигурации рабочего времени.";
                return;
            }

            if (startTime < workStart || endTime > workEnd)
            {
                SupportLogger.Warning("Бронирование отклонено: время вне рабочего периода.");
                StatusTextBlock.Text =
                    "Бронирование доступно с " +
                    settings.WorkDayStart + " до " +
                    settings.WorkDayEnd + ".";
                return;
            }

            double duration =
                (endTime - startTime).TotalMinutes;

            if (duration < settings.MinBookingMinutes)
            {
                SupportLogger.Warning("Бронирование отклонено: слишком короткая длительность.");
                StatusTextBlock.Text =
                    "Минимальная длительность — " +
                    settings.MinBookingMinutes + " минут.";
                return;
            }

            if (duration > settings.MaxBookingHours * 60)
            {
                SupportLogger.Warning("Бронирование отклонено: превышена максимальная длительность.");
                StatusTextBlock.Text =
                    "Максимальная длительность — " +
                    settings.MaxBookingHours + " ч.";
                return;
            }

            Booking booking = new Booking
            {
                UserId = user.Id,
                RoomId = room.Id,
                UserName = user.FullName,
                RoomName = room.Name,
                Date = bookingDate,
                StartTime = startTime,
                EndTime = endTime
            };

            try
            {
                bool added = bookingService.AddBooking(booking);

                if (!added)
                {
                    SupportLogger.Warning(
                        "Бронирование отклонено: обнаружено пересечение.");

                    StatusTextBlock.Text =
                        "На это время аудитория уже забронирована.";
                    return;
                }

                SupportLogger.Info(
                    "Бронирование успешно создано. UserId=" +
                    user.Id + "; RoomId=" + room.Id);

                StatusTextBlock.Text = "Бронирование создано.";
                RefreshBookings();
            }
            catch (Exception ex)
            {
                SupportLogger.Error(
                    "Непредвиденная ошибка при создании бронирования.", ex);

                StatusTextBlock.Text =
                    "Не удалось выполнить операцию. Обратитесь в поддержку.";
                return;
            }
        }

        private void SupportButton_Click(object sender, RoutedEventArgs e)
        {
            SupportRequestWindow window =
                new SupportRequestWindow(
                    "1.0.0",
                    SupportLogger.LogFilePath);

            window.Owner = this;
            window.ShowDialog();
        }
    }
}
