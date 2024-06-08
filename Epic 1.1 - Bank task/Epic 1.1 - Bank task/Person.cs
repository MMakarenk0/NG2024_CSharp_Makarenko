using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Person : Client
    {
        public int Person_id { get; private set; }
        public string Address { get; private set; }

        // variable for auto-increment
        protected static int person_id = 0;
        public Person(string name, string address) : base(name)
        {
            Person_id = person_id++;
            Address = address;
        }
    }
}
