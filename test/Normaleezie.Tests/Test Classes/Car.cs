using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.Tests.Test_Classes
{
    public class Car : IVehicle
    {
        public int Seats { get; set; }
        public int Tires { get; set; }
        public string Color { get; set; }
    }
}
