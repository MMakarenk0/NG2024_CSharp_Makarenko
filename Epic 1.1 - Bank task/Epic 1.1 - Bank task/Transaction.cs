﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace Epic_1._1___Bank_task
{
    internal class Transaction
    {
        public int Transaction_id { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }

        // variable for auto-increment
        protected static int transaction_number = 0;
        public Transaction(decimal amount)
        {
            Transaction_id = transaction_number++;
            Amount = amount;
            Timestamp = DateTime.Now;
        }
        public void RecordTransaction()
        {
            Console.WriteLine("Transaction recorded:");
            var details = GetTransactionDetails();
            Console.WriteLine(details);
        }
        public string GetTransactionDetails()
        {
            return $"Transaction ID: {Transaction_id}, Amount: {Amount}, Timestamp: {Timestamp}";
        }
    }
}
