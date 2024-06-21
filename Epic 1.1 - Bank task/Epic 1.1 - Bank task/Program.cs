namespace Epic_1._1___Bank_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Admin admin = new Admin( "Admin User", "123 Admin St.", Role.Administrator);

            Person client1 = new Person("John Doe", "456 Elm St.");
            admin.AddClient(client1);

            client1.Deposit(500m);
            client1.Withdraw(200m);

            Console.WriteLine($"Client1 Balance: {client1.GetBalance()}");

            Person client2 = new Person("Jane Smith", "789 Pine St.");
            admin.AddClient(client2);

            client2.Deposit(1000m);
            client2.Withdraw(300m);

            Console.WriteLine($"Client2 Balance: {client2.GetBalance()}");

            admin.GenerateReport();

            admin.RemoveClient(client1.ClientId);
        }
    }
}
