namespace ExamenPractico.Models;

public class Addresse
{

    public required string address1 { get; set; }
    public  required string city { get; set; }
    public required string stateCode { get; set; }
    public required string postalCode { get; set; }

    public required string countryCode { get; set; }
    
    public required string creationDate { get; set; }

    public required bool preferred { get; set; }
  



}