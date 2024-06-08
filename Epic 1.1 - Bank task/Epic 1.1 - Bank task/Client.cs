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
        public int Client_id { get; private set; }
        public string Name { get; private set; }
        public int Account_number { get; private set; }
        protected Balance balance;
        protected List<Transaction> transactions;

        // variables for auto-increment
        protected static int account_number = 0;
        protected static int client_number = 0;
        public Client(string name) 
        {
            Client_id = client_number++;
            Account_number = account_number++;
            Name = name;
            balance = new Balance(Client_id);
            transactions = new List<Transaction>();
        }
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance.Update_balance(amount);
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
            if (amount > 0 && amount <= balance.Get_balance())
            {
                balance.Update_balance(-amount);
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
            return balance.Get_balance();
        }
        public List<Transaction> GetTransactions()
        {
            return transactions;
        }
    }
}
