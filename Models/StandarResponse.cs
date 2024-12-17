namespace ExamenPractico.Models
{
    public class StandardResponse
    {
        public required string msg { get; set; }
        public required bool success { get; set; }
        public required object data { get; set; }
    }
}