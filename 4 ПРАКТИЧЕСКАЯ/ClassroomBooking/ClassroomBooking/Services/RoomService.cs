using System.Collections.Generic;
using ClassroomBooking.Models;

namespace ClassroomBooking.Services
{
    /// <summary>
    /// Учебный список аудиторий (хранится в памяти).
    /// Room 305 намеренно недоступна: на ней проверяется AllowUnavailableRooms.
    /// </summary>
    public class RoomService
    {
        private readonly List<Room> rooms = new List<Room>
        {
            new Room { Id = 101, Name = "Room 101", Capacity = 24, IsAvailable = true },
            new Room { Id = 202, Name = "Room 202", Capacity = 16, IsAvailable = true },
            new Room { Id = 305, Name = "Room 305", Capacity = 30, IsAvailable = false }
        };

        public List<Room> GetAll()
        {
            return rooms;
        }
    }
}
