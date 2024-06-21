using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Epic_1._1___Bank_task
{
    internal class Client
    {
        public int ClientId { get; private set; }
        public string Name { get; private set; }
        public int AccountNumber { get; private set; }
        protected Balance balance;
        protected List<Transaction> transactions;

        // variables for auto-increment
        protected static int accountNumber = 0;
        protected static int clientNumber = 0;
        public Client(string name) 
        {
            ClientId = clientNumber++;
            AccountNumber = accountNumber++;
            Name = name;
            balance = new Balance(ClientId);
            transactions = new List<Transaction>();
        }
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance.UpdateBalance(amount);
                var transaction = new Transaction(amount);
                transaction.RecordTransaction();
            }
            else
            {
                Console.WriteLine("Deposit amount must be positive.");
            }
        }
        public void Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= balance.GetBalance())
            {
                balance.UpdateBalance(-amount);
                var transaction = new Transaction(-amount);
                transaction.RecordTransaction();
            }
            else
            {
                Console.WriteLine("Insufficient balance or invalid amount.");
            }
        }
        public decimal GetBalance()
        {
            return balance.GetBalance();
        }
        public List<Transaction> GetTransactions()
        {
            return transactions;
        }
    }
}
