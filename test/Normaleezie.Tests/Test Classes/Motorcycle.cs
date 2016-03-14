using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.Tests.Test_Classes
{
    public class Motorcycle : IVehicle
    {
        public int Tires { get; set; }
        public string Color { get; set; }
        public string HandleBarType { get; set; }
    }
}
