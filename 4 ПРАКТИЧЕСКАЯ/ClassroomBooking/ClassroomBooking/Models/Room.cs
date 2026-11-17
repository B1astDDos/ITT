namespace ClassroomBooking.Models
{
    /// <summary>
    /// Аудитория. IsAvailable = false означает, что аудитория недоступна
    /// для бронирования (ремонт, занятость кафедры и т. п.).
    /// </summary>
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
