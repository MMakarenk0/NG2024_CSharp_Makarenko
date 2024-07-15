namespace ReportApp.Models;

public class Admin
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PreferedName { get; set; }
    public string Pronouns { get; set; }
    public string City { get; set; }
    public bool isAdmin { get; set; } = true;

    // Which Clients this Admin should manage
    public List<Client> Clients { get; set; }
}
