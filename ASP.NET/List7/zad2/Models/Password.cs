namespace zad2.Models
{
    public class Password
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public int Iterations { get; set; }
    }
}
