using System;

namespace ClassroomBooking.Models
{
    /// <summary>
    /// Бронирование аудитории.
    /// </summary>
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoomId { get; set; }

        public string UserName { get; set; }

        public string RoomName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string DateText
        {
            get { return Date.ToString("dd.MM.yyyy"); }
        }

        public string StartText
        {
            get { return StartTime.ToString(@"hh\:mm"); }
        }

        public string EndText
        {
            get { return EndTime.ToString(@"hh\:mm"); }
        }
    }
}
