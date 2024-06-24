using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Admin : Person
    {
        public int AdminId { get; private set; }
        public Role Role { get; private set; }
        protected static List<Client> clients = new List<Client>();

        // variable for auto-increment
        protected static int adminId = 0;
        public Admin(string address, Role role) : base(address)
        {
            Role = role;
        }
        public void AddClient(Client client)
        {
            clients.Add(client);
            Console.WriteLine($"Client {client.Name} added.");
        }
        public void RemoveClient(int clientId)
        {
            var client = clients.Find(c => c.ClientId == clientId);
            if (client != null)
            {
                clients.Remove(client);
                Console.WriteLine($"Client {client.Name} removed.");
            }
            else
            {
                Console.WriteLine("Client not found.");
            }
        }
        public void ViewTransactions()
        {
            foreach (var client in clients)
            {
                var clientTransactions = client.GetTransactions();
                foreach (var transaction in clientTransactions)
                {
                    Console.WriteLine(transaction.GetTransactionDetails());
                }
            }
        }
        public void GenerateReport()
        {
            var allTransactions = clients.SelectMany(client => client.GetTransactions()).ToList();
            var totalAmount = allTransactions.Sum(t => t.Amount);
            var totalTransactions = allTransactions.Count;
            var deposits = allTransactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var withdrawals = allTransactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            var totalDeposits = allTransactions.Count(t => t.Amount > 0);
            var totalWithdrawals = allTransactions.Count(t => t.Amount < 0);

            Console.WriteLine("Transaction Report:");
            Console.WriteLine($"Total Amount Transacted: {totalAmount}");
            Console.WriteLine($"Total Transactions: {totalTransactions}");
            Console.WriteLine($"Total Deposits: {totalDeposits}, Amount: {deposits}");
            Console.WriteLine($"Total Withdrawals: {totalWithdrawals}, Amount: {withdrawals}");
        }
    }
}
