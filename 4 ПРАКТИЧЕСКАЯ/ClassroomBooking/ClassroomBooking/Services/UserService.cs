using System.Collections.Generic;
using ClassroomBooking.Models;

namespace ClassroomBooking.Services
{
    /// <summary>
    /// Учебный список пользователей (хранится в памяти).
    /// </summary>
    public class UserService
    {
        private readonly List<User> users = new List<User>
        {
            new User { Id = 1, FullName = "Иванов Иван Иванович", Group = "ИС-21" },
            new User { Id = 2, FullName = "Петрова Анна Сергеевна", Group = "ИС-21" },
            new User { Id = 3, FullName = "Сидоров Пётр Алексеевич", Group = "ИС-22" }
        };

        public List<User> GetAll()
        {
            return users;
        }
    }
}
