using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Person : Client
    {
        public int PersonId { get; private set; }
        public string Address { get; private set; }

        // variable for auto-increment
        protected static int personId = 0;
        public Person(string name, string address) : base(name)
        {
            PersonId = personId++;
            Address = address;
        }
    }
}
