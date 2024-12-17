namespace ExamenPractico.Models;

public class CustomerItem
{

    public required string customerId { get; set; }
    public required string email { get; set; }
    public required string phoneMobile { get; set; }
    
    public required string firstName { get; set; }
    public required string lastName { get; set; }
    public required string birthday { get; set; }
    public required List<Addresse> addresses { get; set; }


}