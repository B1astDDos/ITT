using System.Collections.Generic;
using System.Linq;
using ClassroomBooking.Models;

namespace ClassroomBooking.Services
{
    /// <summary>
    /// Сервис бронирований. Данные учебные и хранятся в памяти процесса.
    /// AddBooking возвращает false, если время пересекается с существующей бронью
    /// этой же аудитории на эту же дату.
    /// </summary>
    public class BookingService
    {
        private readonly List<Booking> bookings = new List<Booking>();
        private int nextId = 1;

        public List<Booking> GetAll()
        {
            return bookings;
        }

        public bool AddBooking(Booking booking)
        {
            bool hasConflict = bookings.Any(b =>
                b.RoomId == booking.RoomId &&
                b.Date.Date == booking.Date.Date &&
                booking.StartTime < b.EndTime &&
                b.StartTime < booking.EndTime);

            if (hasConflict)
                return false;

            booking.Id = nextId++;
            bookings.Add(booking);
            return true;
        }
    }
}
