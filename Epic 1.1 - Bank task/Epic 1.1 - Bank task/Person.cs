using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_1._1___Bank_task
{
    internal class Person
    {
        public int PersonId { get; private set; }
        public string Address { get; private set; }

        // variable for auto-increment
        protected static int personId = 0;
        public Person(string address)
        {
            PersonId = personId++;
            Address = address;
        }
    }
}
