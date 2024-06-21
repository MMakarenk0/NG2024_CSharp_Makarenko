using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Balance
    {
        public int BalanceId {  get; private set; }
        public int ClientId { get; private set; }
        public decimal BalanceAmount { get; private set; }

        // variable for auto-increment
        private static int balanceNumber = 0;
        public Balance(int clientId)
        {
            BalanceId = balanceNumber++;
            ClientId = clientId;
            BalanceAmount = 0m;
        }   
        public decimal GetBalance(){ return BalanceAmount; }
        public void UpdateBalance(decimal amount) { BalanceAmount += amount; }
    }
}
