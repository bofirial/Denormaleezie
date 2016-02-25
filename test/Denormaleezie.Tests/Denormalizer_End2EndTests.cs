using Denormaleezie;
using Denormaleezie.Tests.Test_Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dnz.E2E
{
    public class When_Calling_DenormalizeToJSON_With_A_Null_Object
    {
        private string json;

        public When_Calling_DenormalizeToJSON_With_A_Null_Object()
        {
            Denormalizer denormalizer = new Denormalizer();

            json = denormalizer.DenormalizeToJSON<object>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_String()
        {
            Assert.Equal(string.Empty, json);
        }
    }
    public class When_Calling_DenormalizeToJSON_With_A_List_Of_Flat_Objects
    {
        private List<Animal> animals;
        private string json;

        public When_Calling_DenormalizeToJSON_With_A_List_Of_Flat_Objects()
        {
            Denormalizer denormalizer = new Denormalizer();

            animals = new List<Animal>()
            {
                new Animal() {AnimalId = 101, Age = 10, Name = "Tony", Type = "Tiger" },
                new Animal() {AnimalId = 102, Age = 11, Name = "Lenny", Type = "Tiger" },
                new Animal() {AnimalId = 103, Age = 2, Name = "John", Type = "Tiger" },
                new Animal() {AnimalId = 104, Age = 15, Name = "Tony", Type = "Giraffe" },
                new Animal() {AnimalId = 105, Age = 10, Name = "Garry", Type = "Giraffe" },
                new Animal() {AnimalId = 106, Age = 10, Name = "Zachary", Type = "Zebra" },
            };

            string json = new Denormaleezie.Denormalizer().DenormalizeToJSON(animals);
        }
        
        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Reduce_The_Size_Vs_JSON_Serialization()
        {
            Assert.True(json.Length < JsonConvert.SerializeObject(animals).Length);
        }
    }


    public class When_Calling_DenormalizeToJSON_With_A_List_Of_Books
    {
        private List<Book> books;
        private string denormalizedJson;
        private string serializedJson;

        public When_Calling_DenormalizeToJSON_With_A_List_Of_Books()
        {
            Denormalizer denormalizer = new Denormalizer();

            books = new List<Book>()
            {
                new Book() {Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 7, 29),
                    Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                new Book() {Title = "The Two Towers", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 11, 11),
                    Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                new Book() {Title = "The The Return of the King", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1955, 10, 20),
                    Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                new Book() {Title = "Storm Front", Author = "Jim Butcher", PublishDate = new DateTime(2000, 4, 1),

                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Fool Moon", Author = "Jim Butcher", PublishDate = new DateTime(2001, 1, 1),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Grave Peril", Author = "Jim Butcher", PublishDate = new DateTime(2001, 9, 1),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Summer Knight", Author = "Jim Butcher", PublishDate = new DateTime(2002, 2, 2),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Death Masks", Author = "Jim Butcher", PublishDate = new DateTime(2003, 8, 5),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Blood Rites", Author = "Jim Butcher", PublishDate = new DateTime(2004, 8, 2),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Dead Beat", Author = "Jim Butcher", PublishDate = new DateTime(2005, 5, 3),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Proven Guilty", Author = "Jim Butcher", PublishDate = new DateTime(2006, 5, 2),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "White Night", Author = "Jim Butcher", PublishDate = new DateTime(2007, 4, 3),
                    Series = "The Dresden Files", PurchaseYear = 2015, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Small Favor", Author = "Jim Butcher", PublishDate = new DateTime(2008, 4, 1),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Turn Coat", Author = "Jim Butcher", PublishDate = new DateTime(2009, 4, 7),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = true},
                new Book() {Title = "Changes", Author = "Jim Butcher", PublishDate = new DateTime(2010, 4, 6),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = false},
                new Book() {Title = "Ghost Story", Author = "Jim Butcher", PublishDate = new DateTime(2011, 4, 26),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = false},
                new Book() {Title = "Cold Days", Author = "Jim Butcher", PublishDate = new DateTime(2012, 11, 27),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = false},
                new Book() {Title = "Skin Game", Author = "Jim Butcher", PublishDate = new DateTime(2014, 5, 27),
                    Series = "The Dresden Files", PurchaseYear = 2016, PurchaseLocation = "Amazon", HasRead = false},
            };

            denormalizedJson = denormalizer.DenormalizeToJSON(books);
            serializedJson = JsonConvert.SerializeObject(books);
        }

        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(denormalizedJson);
        }

        [Fact]
        public void It_Should_Reduce_The_Size_Vs_JSON_Serialization()
        {
            Assert.True(denormalizedJson.Length < serializedJson.Length);
        }
    }
}
