namespace ClassroomBooking.Configuration
{
    /// <summary>
    /// Параметры приложения, которые вынесены во внешний файл appsettings.json.
    /// Имена свойств должны совпадать с именами ключей JSON.
    /// </summary>
    public class AppSettings
    {
        public string AppTitle { get; set; } = "Classroom Booking";

        public string WorkDayStart { get; set; } = "08:00";

        public string WorkDayEnd { get; set; } = "20:00";

        public int MinBookingMinutes { get; set; } = 30;

        public int MaxBookingHours { get; set; } = 4;

        public bool AllowUnavailableRooms { get; set; } = false;

        public bool AllowPastDates { get; set; } = false;

        public bool AdminMode { get; set; } = false;

        public string StatusMessage { get; set; } = "Система готова к работе";
    }
}
