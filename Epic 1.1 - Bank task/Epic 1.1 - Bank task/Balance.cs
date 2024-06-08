using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Balance
    {
        public int Balance_id {  get; private set; }
        public int Client_id { get; private set; }
        public decimal Balance_amount { get; private set; }

        // variable for auto-increment
        private static int balance_number = 0;
        public Balance(int client_id)
        {
            Balance_id = balance_number++;
            Client_id = client_id;
            Balance_amount = 0m;
        }   
        public decimal Get_balance(){ return Balance_amount; }
        public void Update_balance(decimal amount) { Balance_amount += amount; }
    }
}
