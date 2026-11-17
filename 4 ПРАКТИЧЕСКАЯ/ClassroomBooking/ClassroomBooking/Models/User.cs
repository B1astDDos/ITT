namespace ClassroomBooking.Models
{
    /// <summary>
    /// Пользователь системы.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Group { get; set; }

        public override string ToString()
        {
            return FullName + " (" + Group + ")";
        }
    }
}
