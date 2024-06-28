using Epic_2._3___PhoneBook;

Console.WriteLine("Welcome to Phone Book");
PhoneBook phoneBook = new PhoneBook();
while (true)
{
    Console.WriteLine("Select an option: Q - exit, A - add, GP - Get by phone, GN - Get by name, GA - Get all, D - delete by phone");
    string option = Console.ReadLine().ToLower();
    switch (option)
    {
        case "q":
            return;
        case "a":
            phoneBook.AddPerson();
            break;
        case "gp":
            phoneBook.GetByPhone();
            break;
        case "gn":
            phoneBook.GetByFullName();
            break;
        case "ga":
            phoneBook.GetAll();
            break;
        case "d":
            phoneBook.DeleteByPhone();
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}