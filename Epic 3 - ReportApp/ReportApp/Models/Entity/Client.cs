namespace ReportApp.Models.Entity;

public class Client
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PreferedName { get; set; }
    public string Pronouns { get; set; }
    public string City { get; set; }

    // Which Admin is a manager of Client
    public Admin Admin { get; set; }
}
