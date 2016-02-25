using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denormaleezie.Tests.Test_Classes
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Series { get; set; }
        public string PurchaseLocation { get; set; }
        public int PurchaseYear { get; set; }
        public bool HasRead { get; set; }
    }
}
