using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.Tests.Test_Classes
{
    public class Person : Animal
    {
        public Person()
        {
            this.Type = "Human";
        }
        public List<Pet> Pets { get; set; }
    }
}
