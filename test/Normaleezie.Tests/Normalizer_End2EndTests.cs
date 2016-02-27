using Normaleezie;
using Normaleezie.Tests.Test_Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace nEZ.E2E
{
    public class When_Calling_Normalize_With_A_Null_Object
    {
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_Null_Object()
        {
            Normalizer normalizer = new Normalizer();

            normalizedForm = normalizer.Normalize<object>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_List()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
        }
    }
    public class When_Calling_Normalize_With_A_List_Of_Animals
    {
        private readonly List<Animal> animals;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_Of_Animals()
        {
            Normalizer normalizer = new Normalizer();

            animals = new List<Animal>()
            {
                new Animal() {AnimalId = 101, Age = 10, Name = "Tony", Type = "Tiger" },
                new Animal() {AnimalId = 102, Age = 11, Name = "Lenny", Type = "Tiger" },
                new Animal() {AnimalId = 103, Age = 2, Name = "John", Type = "Tiger" },
                new Animal() {AnimalId = 104, Age = 15, Name = "Tony", Type = "Giraffe" },
                new Animal() {AnimalId = 105, Age = 10, Name = "Garry", Type = "Giraffe" },
                new Animal() {AnimalId = 106, Age = 10, Name = "Zachary", Type = "Zebra" },
            };

            normalizedForm = normalizer.Normalize(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal(new List<object>() { "AnimalId" }, normalizedForm[0][0]);
            Assert.Equal(new List<object>() { "Age", 10, 11, 2, 15 }, normalizedForm[0][1]);
            Assert.Equal(new List<object>() { "Name", "Tony", "Lenny", "John", "Garry", "Zachary" }, normalizedForm[0][2]);
            Assert.Equal(new List<object>() { "Type", "Tiger", "Giraffe", "Zebra" }, normalizedForm[0][3]);

            Assert.Equal(new List<object>() { 101, 1, 1, 1 }, normalizedForm[1][0]);
            Assert.Equal(new List<object>() { 102, 2, 2, 1 }, normalizedForm[1][1]);
            Assert.Equal(new List<object>() { 103, 3, 3, 1 }, normalizedForm[1][2]);
            Assert.Equal(new List<object>() { 104, 4, 1, 2 }, normalizedForm[1][3]);
            Assert.Equal(new List<object>() { 105, 1, 4, 2 }, normalizedForm[1][4]);
            Assert.Equal(new List<object>() { 106, 1, 5, 3 }, normalizedForm[1][5]);
        }

        [Fact]
        public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        {
            Assert.True(JsonConvert.SerializeObject(normalizedForm).Length < JsonConvert.SerializeObject(animals).Length);
        }
    }


    public class When_Calling_Normalize_With_A_List_Of_Books
    {
        private readonly List<Book> books;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_Of_Books()
        {
            Normalizer normalizer = new Normalizer();

            books = new List<Book>()
            {
                new Book() {Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 7, 29),
                    Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                new Book() {Title = "The Two Towers", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 11, 11),
                    Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                new Book() {Title = "The Return of the King", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1955, 10, 20),
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

            normalizedForm = normalizer.Normalize(books);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal(new List<object>() { "Title" }, normalizedForm[0][0]);
            Assert.Equal(new List<object>() { "Author", "J.R.R. Tolkien", "Jim Butcher" }, normalizedForm[0][1]);
            Assert.Equal(new List<object>() { "PublishDate" }, normalizedForm[0][2]);
            Assert.Equal(new List<object>() { "Series", "The Lord of the Rings", "The Dresden Files" }, normalizedForm[0][3]);
            Assert.Equal(new List<object>() { "PurchaseLocation", "Barnes and Noble", "Amazon" }, normalizedForm[0][4]);
            Assert.Equal(new List<object>() { "PurchaseYear", 2000, 2015, 2016 }, normalizedForm[0][5]);
            Assert.Equal(new List<object>() { "HasRead", true, false }, normalizedForm[0][6]);

            Assert.Equal(new List<object>() { "The Fellowship of the Ring", 1, new DateTime(1954, 7, 29), 1, 1, 1, 1 }, normalizedForm[1][0]);
            Assert.Equal(new List<object>() { "The Two Towers", 1, new DateTime(1954, 11, 11), 1, 1, 1, 1 }, normalizedForm[1][1]);
            Assert.Equal(new List<object>() { "The Return of the King", 1, new DateTime(1955, 10, 20), 1, 1, 1, 1 }, normalizedForm[1][2]);
            Assert.Equal(new List<object>() { "Storm Front", 2, new DateTime(2000, 4, 1), 2, 2, 2, 1 }, normalizedForm[1][3]);
            Assert.Equal(new List<object>() { "Fool Moon", 2, new DateTime(2001, 1, 1), 2, 2, 2, 1 }, normalizedForm[1][4]);
            Assert.Equal(new List<object>() { "Grave Peril", 2, new DateTime(2001, 9, 1), 2, 2, 2, 1 }, normalizedForm[1][5]);
            Assert.Equal(new List<object>() { "Summer Knight", 2, new DateTime(2002, 2, 2), 2, 2, 2, 1 }, normalizedForm[1][6]);
            Assert.Equal(new List<object>() { "Death Masks", 2, new DateTime(2003, 8, 5), 2, 2, 2, 1 }, normalizedForm[1][7]);
            Assert.Equal(new List<object>() { "Blood Rites", 2, new DateTime(2004, 8, 2), 2, 2, 2, 1 }, normalizedForm[1][8]);
            Assert.Equal(new List<object>() { "Dead Beat", 2, new DateTime(2005, 5, 3), 2, 2, 2, 1 }, normalizedForm[1][9]);
            Assert.Equal(new List<object>() { "Proven Guilty", 2, new DateTime(2006, 5, 2), 2, 2, 2, 1 }, normalizedForm[1][10]);
            Assert.Equal(new List<object>() { "White Night", 2, new DateTime(2007, 4, 3), 2, 2, 2, 1 }, normalizedForm[1][11]);
            Assert.Equal(new List<object>() { "Small Favor", 2, new DateTime(2008, 4, 1), 2, 2, 3, 1 }, normalizedForm[1][12]);
            Assert.Equal(new List<object>() { "Turn Coat", 2, new DateTime(2009, 4, 7), 2, 2, 3, 1 }, normalizedForm[1][13]);
            Assert.Equal(new List<object>() { "Changes", 2, new DateTime(2010, 4, 6), 2, 2, 3, 2 }, normalizedForm[1][14]);
            Assert.Equal(new List<object>() { "Ghost Story", 2, new DateTime(2011, 4, 26), 2, 2, 3, 2 }, normalizedForm[1][15]);
            Assert.Equal(new List<object>() { "Cold Days", 2, new DateTime(2012, 11, 27), 2, 2, 3, 2 }, normalizedForm[1][16]);
            Assert.Equal(new List<object>() { "Skin Game", 2, new DateTime(2014, 5, 27), 2, 2, 3, 2 }, normalizedForm[1][17]);
        }

        [Fact]
        public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        {
            Assert.True(JsonConvert.SerializeObject(normalizedForm).Length < JsonConvert.SerializeObject(books).Length);
        }
    }

    public class When_Calling_Normalize_With_A_List_List_Of_Books
    {
        private readonly List<List<Book>> libraries;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_List_Of_Books()
        {
            Normalizer normalizer = new Normalizer();

            libraries = new List<List<Book>>()
            {
                new List<Book>()
                {
                    new Book() {Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 7, 29),
                        Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                    new Book() {Title = "The Two Towers", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1954, 11, 11),
                        Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                    new Book() {Title = "The The Return of the King", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1955, 10, 20),
                        Series = "The Lord of the Rings", PurchaseYear = 2000, PurchaseLocation = "Barnes and Noble", HasRead = true},
                },
                new List<Book>()
                {
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
                }
            };

            normalizedForm = normalizer.Normalize(libraries);
        }

        //[Fact]
        //public void It_Should_Return_A_List_In_Normalized_Form()
        //{
        //    Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
        //    Assert.NotEmpty(normalizedForm);

        //    Assert.Equal(new List<object>() { "Title" }, normalizedForm[0][0]);
        //    Assert.Equal(new List<object>() { "Author", "J.R.R. Tolkien", "Jim Butcher" }, normalizedForm[0][1]);
        //    Assert.Equal(new List<object>() { "PublishDate" }, normalizedForm[0][2]);
        //    Assert.Equal(new List<object>() { "Series", "The Lord of the Rings", "The Dresden Files" }, normalizedForm[0][3]);
        //    Assert.Equal(new List<object>() { "PurchaseLocation", "Barnes and Noble", "Amazon" }, normalizedForm[0][4]);
        //    Assert.Equal(new List<object>() { "PurchaseYear", 2000, 2015, 2016 }, normalizedForm[0][5]);
        //    Assert.Equal(new List<object>() { "HasRead", true, false }, normalizedForm[0][6]);

        //    Assert.Equal(new List<object>() { "The Fellowship of the Ring", 1, new DateTime(1954, 7, 29), 1, 1, 1, 1 }, normalizedForm[1][0]);
        //    Assert.Equal(new List<object>() { "The Two Towers", 1, new DateTime(1954, 11, 11), 1, 1, 1, 1 }, normalizedForm[1][1]);
        //    Assert.Equal(new List<object>() { "The Return of the King", 1, new DateTime(1955, 10, 20), 1, 1, 1, 1 }, normalizedForm[1][2]);
        //    Assert.Equal(new List<object>() { "Storm Front", 2, new DateTime(2000, 4, 1), 2, 2, 2, 1 }, normalizedForm[1][3]);
        //    Assert.Equal(new List<object>() { "Fool Moon", 2, new DateTime(2001, 1, 1), 2, 2, 2, 1 }, normalizedForm[1][4]);
        //    Assert.Equal(new List<object>() { "Grave Peril", 2, new DateTime(2001, 9, 1), 2, 2, 2, 1 }, normalizedForm[1][5]);
        //    Assert.Equal(new List<object>() { "Summer Knight", 2, new DateTime(2002, 2, 2), 2, 2, 2, 1 }, normalizedForm[1][6]);
        //    Assert.Equal(new List<object>() { "Death Masks", 2, new DateTime(2003, 8, 5), 2, 2, 2, 1 }, normalizedForm[1][7]);
        //    Assert.Equal(new List<object>() { "Blood Rites", 2, new DateTime(2004, 8, 2), 2, 2, 2, 1 }, normalizedForm[1][8]);
        //    Assert.Equal(new List<object>() { "Dead Beat", 2, new DateTime(2005, 5, 3), 2, 2, 2, 1 }, normalizedForm[1][9]);
        //    Assert.Equal(new List<object>() { "Proven Guilty", 2, new DateTime(2006, 5, 2), 2, 2, 2, 1 }, normalizedForm[1][10]);
        //    Assert.Equal(new List<object>() { "White Night", 2, new DateTime(2007, 4, 3), 2, 2, 2, 1 }, normalizedForm[1][11]);
        //    Assert.Equal(new List<object>() { "Small Favor", 2, new DateTime(2008, 4, 1), 2, 2, 3, 1 }, normalizedForm[1][12]);
        //    Assert.Equal(new List<object>() { "Turn Coat", 2, new DateTime(2009, 4, 7), 2, 2, 3, 1 }, normalizedForm[1][13]);
        //    Assert.Equal(new List<object>() { "Changes", 2, new DateTime(2010, 4, 6), 2, 2, 3, 2 }, normalizedForm[1][14]);
        //    Assert.Equal(new List<object>() { "Ghost Story", 2, new DateTime(2011, 4, 26), 2, 2, 3, 2 }, normalizedForm[1][15]);
        //    Assert.Equal(new List<object>() { "Cold Days", 2, new DateTime(2012, 11, 27), 2, 2, 3, 2 }, normalizedForm[1][16]);
        //    Assert.Equal(new List<object>() { "Skin Game", 2, new DateTime(2014, 5, 27), 2, 2, 3, 2 }, normalizedForm[1][17]);
        //}

        //[Fact]
        //public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        //{
        //    string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
        //    string serializedJson = JsonConvert.SerializeObject(libraries);

        //    Assert.True(normalizedJson.Length < serializedJson.Length);
        //}
    }

    public class When_Calling_Normalize_With_A_List_Of_Pets
    {
        private readonly List<Pet> pets;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_Of_Pets()
        {
            Normalizer normalizer = new Normalizer();

            pets = new List<Pet>()
            {
                new Pet() {AnimalId = 10, Age = 15, Name = "Ayla", Type = "Cat", Owner = new Person() {Name = "Jennifer"} },
                new Pet() {AnimalId = 11, Age = 15, Name = "Cookie", Type = "Cat", Owner = new Person() {Name = "Jennifer"} },
                new Pet() {AnimalId = 12, Age = 8, Name = "Bob", Type = "Cat", Owner = new Person() {Name = "Jennifer"} },
                new Pet() {AnimalId = 13, Age = 2, Name = "Ellie", Type = "Dog", Owner = new Person() {Name = "James"} },
            };

            normalizedForm = normalizer.Normalize(pets);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
        }

        [Fact]
        public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(pets);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }

    public class When_Calling_Normalize_With_A_List_Of_People
    {
        private readonly List<Person> people;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_Of_People()
        {
            Normalizer normalizer = new Normalizer();

            people = new List<Person>()
            {
                new Person() {Name = "Jennifer", Pets = new List<Pet>()
                {
                    new Pet() {AnimalId = 10, Age = 15, Name = "Ayla", Type = "Cat" },
                    new Pet() {AnimalId = 11, Age = 15, Name = "Cookie", Type = "Cat" },
                    new Pet() {AnimalId = 12, Age = 8, Name = "Bob", Type = "Cat"}
                }},

                new Person() {Name = "James", Pets = new List<Pet>()
                {
                    new Pet() {AnimalId = 12, Age = 8, Name = "Bob", Type = "Cat"},
                    new Pet() {AnimalId = 14, Age = 12, Name = "Tiger", Type = "Cat"}
                }}
            };

            normalizedForm = normalizer.Normalize(people);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
        }

        [Fact]
        public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(people);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }

    public class When_Calling_Normalize_With_A_List_Of_People_Containing_A_Circular_Reference
    {
        private readonly List<Person> people;
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_List_Of_People_Containing_A_Circular_Reference()
        {
            Normalizer normalizer = new Normalizer();

            Person jennifer = new Person()
            {
                Name = "Jennifer",
                Pets = new List<Pet>()
                {
                    new Pet() {AnimalId = 10, Age = 15, Name = "Ayla", Type = "Cat"},
                    new Pet() {AnimalId = 11, Age = 15, Name = "Cookie", Type = "Cat"},
                    new Pet() {AnimalId = 12, Age = 8, Name = "Bob", Type = "Cat"}
                }
            };

            foreach (var pet in jennifer.Pets)
            {
                pet.Owner = jennifer;
            }

            people = new List<Person>()
            {
                jennifer
            };

            normalizedForm = normalizer.Normalize(people);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
        }

        //[Fact]
        //public void The_Normalized_Form_Should_Reduce_The_String_Length_When_Serialized()
        //{
        //    string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
        //    string serializedJson = JsonConvert.SerializeObject(people);

        //    Assert.True(normalizedJson.Length < serializedJson.Length);
        //}
    }
}
