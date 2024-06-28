using Epic_2._3___PhoneBook.Model;
using System.Text.RegularExpressions;

namespace Epic_2._3___PhoneBook;
public class PhoneBook
{
    public Dictionary<string, Person> People { get; set; } = new Dictionary<string, Person>();

    public void AddPerson()
    {
        Console.WriteLine("Enter new person`s phone number: ");
        string? phoneNumber = Console.ReadLine();

        if (!IsValidPhoneNumber(phoneNumber))
            return;

        Console.WriteLine("Enter new person`s Full name: ");
        string? fullName = Console.ReadLine();

        if (!IsValidFullName(fullName))
            return;

        if (isPhoneNumberUsed(phoneNumber))
        {
            return;
        }
        People.Add(phoneNumber, new Person
        {
            FullName = fullName,
            PhoneNumber = phoneNumber,
        });
        Console.WriteLine("Person added successfully.");
    }
    public void GetAll()
    {
        var sortedPeople = People.Values.OrderBy(x => x.FullName).ToList();
        if (sortedPeople.Count == 0)
        {
            Console.WriteLine("Error. No people have been created.");
        }
        sortedPeople.ForEach(p => Console.WriteLine($"Full Name: {p.FullName}, Phone Number: {p.PhoneNumber}"));
    }
    public void GetByPhone()
    {
        Console.WriteLine("Enter phone number: ");
        string? phoneNumber = Console.ReadLine();

        if (!IsValidPhoneNumber(phoneNumber))
            return;

        if (People.TryGetValue(phoneNumber, out Person person))
        {
            Console.WriteLine($"Full Name: {person.FullName}, Phone Number: {person.PhoneNumber}");
        }
        else
        {
            Console.WriteLine("Error: No person with the specified phone number.");
        }
    }
    public void GetByFullName()
    {
        Console.Write("Enter full name: ");
        string? fullName = Console.ReadLine();

        if (!IsValidFullName(fullName))
            return;

        var results = People.Values.Where(p => p.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)).ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("Error: No persons with the specified full name.");
        }
        else
        {
            results.ForEach(p => Console.WriteLine(p.FullName));
        }
    }
    public void DeleteByPhone()
    {
        Console.Write("Enter phone number: ");
        string? phoneNumber = Console.ReadLine();

        if (!IsValidPhoneNumber(phoneNumber))
            return;

        if (People.Remove(phoneNumber))
        {
            Console.WriteLine("Person deleted successfully.");
        }
        else
        {
            Console.WriteLine("Error: No person with the specified phone number.");
        }
    }
    public bool isPhoneNumberUsed(string phoneNumber)
    {
        return People.ContainsKey(phoneNumber);
    }
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.WriteLine("Error: Phone number cannot be null or empty.");
            return false;
        }

        if (phoneNumber.Length > 10)
        {
            Console.WriteLine("Error: Phone number max length is 10 digits.");
            return false;
        }

        if (!Regex.IsMatch(phoneNumber, @"^\d+$"))
        {
            Console.WriteLine("Error: Phone number can contain only digits.");
            return false;
        }

        return true;
    }
    private bool IsValidFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Error: Full name cannot be null or empty.");
            return false;
        }
        if (fullName.Length > 10)
        {
            Console.WriteLine("Error: Full name max lenght is 10 letters.");
            return false;
        }
        return true;
    }
}
