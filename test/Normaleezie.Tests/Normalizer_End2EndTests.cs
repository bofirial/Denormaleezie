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

namespace E2E
{
    public class Normalize_With_A_Null_Object
    {
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Null_Object()
        {
            Normalizer normalizer = new Normalizer();

            normalizedForm = normalizer.Normalize<object>(null);
        }

        [Fact]
        public void Should_Return_An_Empty_List()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
        }
    }

    public class Normalize_With_A_Type_Containing_Only_Simple_Properties
    {
        private readonly List<Animal> animals;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Type_Containing_Only_Simple_Properties()
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
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal(new List<object>() { "Age", 10, 11, 2, 15 }, normalizedForm[0][0]);
            Assert.Equal(new List<object>() { "AnimalId" }, normalizedForm[0][1]);
            Assert.Equal(new List<object>() { "Name", "Tony", "Lenny", "John", "Garry", "Zachary" }, normalizedForm[0][2]);
            Assert.Equal(new List<object>() { "Type", "Tiger", "Giraffe", "Zebra" }, normalizedForm[0][3]);

            Assert.Equal(new List<object>() { 1, 101, 1, 1 }, normalizedForm[1][0]);
            Assert.Equal(new List<object>() { 2, 102, 2, 1 }, normalizedForm[1][1]);
            Assert.Equal(new List<object>() { 3, 103, 3, 1 }, normalizedForm[1][2]);
            Assert.Equal(new List<object>() { 4, 104, 1, 2 }, normalizedForm[1][3]);
            Assert.Equal(new List<object>() { 1, 105, 4, 2 }, normalizedForm[1][4]);
            Assert.Equal(new List<object>() { 1, 106, 5, 3 }, normalizedForm[1][5]);
        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            Assert.True(JsonConvert.SerializeObject(normalizedForm).Length < JsonConvert.SerializeObject(animals).Length);
        }
    }


    public class Normalize_With_A_Type_Containing_A_DateTime
    {
        private readonly List<Book> books;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Type_Containing_A_DateTime()
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
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal(new List<object>() { "Author", "J.R.R. Tolkien", "Jim Butcher" }, normalizedForm[0][0]);
            Assert.Equal(new List<object>() { "HasRead", true, false }, normalizedForm[0][1]);
            Assert.Equal(new List<object>() { "PublishDate" }, normalizedForm[0][2]);
            Assert.Equal(new List<object>() { "PurchaseLocation", "Barnes and Noble", "Amazon" }, normalizedForm[0][3]);
            Assert.Equal(new List<object>() { "PurchaseYear", 2000, 2015, 2016 }, normalizedForm[0][4]);
            Assert.Equal(new List<object>() { "Series", "The Lord of the Rings", "The Dresden Files" }, normalizedForm[0][5]);
            Assert.Equal(new List<object>() { "Title" }, normalizedForm[0][6]);

            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 7, 29), 1, 1, 1, "The Fellowship of the Ring" }, normalizedForm[1][0]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 11, 11), 1, 1, 1, "The Two Towers" }, normalizedForm[1][1]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1955, 10, 20), 1, 1, 1, "The Return of the King" }, normalizedForm[1][2]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2000, 4, 1), 2, 2, 2, "Storm Front" }, normalizedForm[1][3]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2001, 1, 1), 2, 2, 2, "Fool Moon" }, normalizedForm[1][4]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2001, 9, 1), 2, 2, 2, "Grave Peril" }, normalizedForm[1][5]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2002, 2, 2), 2, 2, 2, "Summer Knight" }, normalizedForm[1][6]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2003, 8, 5), 2, 2, 2, "Death Masks" }, normalizedForm[1][7]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2004, 8, 2), 2, 2, 2, "Blood Rites" }, normalizedForm[1][8]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2005, 5, 3), 2, 2, 2, "Dead Beat" }, normalizedForm[1][9]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2006, 5, 2), 2, 2, 2, "Proven Guilty" }, normalizedForm[1][10]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2007, 4, 3), 2, 2, 2, "White Night" }, normalizedForm[1][11]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2008, 4, 1), 2, 3, 2, "Small Favor" }, normalizedForm[1][12]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2009, 4, 7), 2, 3, 2, "Turn Coat" }, normalizedForm[1][13]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2010, 4, 6), 2, 3, 2, "Changes" }, normalizedForm[1][14]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2011, 4, 26), 2, 3, 2, "Ghost Story" }, normalizedForm[1][15]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2012, 11, 27), 2, 3, 2, "Cold Days" }, normalizedForm[1][16]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2014, 5, 27), 2, 3, 2, "Skin Game" }, normalizedForm[1][17]);
        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            Assert.True(JsonConvert.SerializeObject(normalizedForm).Length < JsonConvert.SerializeObject(books).Length);
        }
    }

    public class Normalize_With_A_Type_That_Contains_A_Complex_Property
    {
        private readonly List<BookMark> bookMarks;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Type_That_Contains_A_Complex_Property()
        {
            Normalizer normalizer = new Normalizer();

            bookMarks = new List<BookMark>()
            {
                new BookMark() {
                    CurrentPage = 101,
                    Book = new Book()
                    {
                        Title = "The Fellowship of the Ring",
                        Author = "J.R.R. Tolkien",
                        PublishDate = new DateTime(1954, 7, 29),
                        Series = "The Lord of the Rings",
                        PurchaseYear = 2000,
                        PurchaseLocation = "Barnes and Noble",
                        HasRead = true
                    }
                },
                 new BookMark() {
                    CurrentPage = 1,
                    Book = new Book() {
                        Title = "The Two Towers",
                        Author = "J.R.R. Tolkien",
                        PublishDate = new DateTime(1954, 11, 11),
                        Series = "The Lord of the Rings",
                        PurchaseYear = 2000,
                        PurchaseLocation = "Barnes and Noble",
                        HasRead = true
                    }
                 },
                 new BookMark() {
                    CurrentPage = 1,
                    Book =new Book() {
                        Title = "The Return of the King",
                        Author = "J.R.R. Tolkien",
                        PublishDate = new DateTime(1955, 10, 20),
                        Series = "The Lord of the Rings",
                        PurchaseYear = 2000,
                        PurchaseLocation = "Barnes and Noble",
                        HasRead = true
                    }
                 },
                 new BookMark() {
                    CurrentPage = 300,
                    Book = new Book() {
                        Title = "Changes",
                        Author = "Jim Butcher",
                        PublishDate = new DateTime(2010, 4, 6),
                        Series = "The Dresden Files",
                        PurchaseYear = 2016,
                        PurchaseLocation = "Amazon",
                        HasRead = false
                    }
                 },
                 new BookMark() {
                    CurrentPage = 1,
                    Book = new Book() {
                        Title = "Ghost Story",
                        Author = "Jim Butcher",
                        PublishDate = new DateTime(2011, 4, 26),
                        Series = "The Dresden Files",
                        PurchaseYear = 2016,
                        PurchaseLocation = "Amazon",
                        HasRead = false
                    }
                 },
                 new BookMark() {
                    CurrentPage = 1,
                    Book = new Book() {
                        Title = "Cold Days",
                        Author = "Jim Butcher",
                        PublishDate = new DateTime(2012, 11, 27),
                        Series = "The Dresden Files",
                        PurchaseYear = 2016,
                        PurchaseLocation = "Amazon",
                        HasRead = false
                    }
                 },
                 new BookMark() {
                    CurrentPage = 1,
                    Book = new Book() {
                        Title = "Skin Game",
                        Author = "Jim Butcher",
                        PublishDate = new DateTime(2014, 5, 27),
                        Series = "The Dresden Files",
                        PurchaseYear = 2016,
                        PurchaseLocation = "Amazon",
                        HasRead = false
                    }
                 },
            };

            normalizedForm = normalizer.Normalize(bookMarks);
        }

        [Fact]
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof (List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal("Book.", normalizedForm[0][0][0]);
            Assert.Equal(new List<object>() { "Author", "J.R.R. Tolkien", "Jim Butcher" }, normalizedForm[0][0][1]);
            Assert.Equal(new List<object>() { "HasRead", true, false }, normalizedForm[0][0][2]);
            Assert.Equal(new List<object>() { "PublishDate" }, normalizedForm[0][0][3]);
            Assert.Equal(new List<object>() { "PurchaseLocation", "Barnes and Noble", "Amazon" }, normalizedForm[0][0][4]);
            Assert.Equal(new List<object>() { "PurchaseYear", 2000, 2016 }, normalizedForm[0][0][5]);
            Assert.Equal(new List<object>() { "Series", "The Lord of the Rings", "The Dresden Files" }, normalizedForm[0][0][6]);
            Assert.Equal(new List<object>() { "Title" }, normalizedForm[0][0][7]);

            Assert.Equal(new List<object>() {"CurrentPage", 101, 1, 300}, normalizedForm[0][1]);

            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 7, 29), 1, 1, 1, "The Fellowship of the Ring" }, normalizedForm[1][0][0]);
            Assert.Equal(1, normalizedForm[1][0][1]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 11, 11), 1, 1, 1, "The Two Towers" }, normalizedForm[1][1][0]);
            Assert.Equal(2, normalizedForm[1][1][1]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1955, 10, 20), 1, 1, 1, "The Return of the King" }, normalizedForm[1][2][0]);
            Assert.Equal(2, normalizedForm[1][2][1]);

            Assert.Equal(new List<object>() { 2, 2, new DateTime(2010, 4, 6), 2, 2, 2, "Changes" }, normalizedForm[1][3][0]);
            Assert.Equal(3, normalizedForm[1][3][1]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2011, 4, 26), 2, 2, 2, "Ghost Story" }, normalizedForm[1][4][0]);
            Assert.Equal(2, normalizedForm[1][4][1]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2012, 11, 27), 2, 2, 2, "Cold Days" }, normalizedForm[1][5][0]);
            Assert.Equal(2, normalizedForm[1][5][1]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2014, 5, 27), 2, 2, 2, "Skin Game" }, normalizedForm[1][6][0]);
            Assert.Equal(2, normalizedForm[1][6][1]);

        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(bookMarks);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }

    public class Normalize_With_A_Type_That_Contains_A_List_Property
    {
        private readonly List<Zoo> zoos;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Type_That_Contains_A_List_Property()
        {
            Normalizer normalizer = new Normalizer();

            zoos = new List<Zoo>()
            {
                new Zoo()
                {
                    Name = "Columbus Zoo and Aquarium",
                    Animals = new List<Animal>()
                    {
                        new Animal() {Age = 20, AnimalId = 101, Name = "Tony", Type = "Tiger"},
                        new Animal() {Age = 20, AnimalId = 102, Name = "Tania", Type = "Tiger"},
                        new Animal() {Age = 3, AnimalId = 103, Name = "Zachary", Type = "Zebra"}
                    }
                },
                new Zoo()
                {
                    Name = "Cincinnati Zoo and Botanical Garden",
                    Animals = new List<Animal>()
                    {
                        new Animal() {Age = 16, AnimalId = 104, Name = "Tony", Type = "Tiger"},
                    }
                }
            };

            normalizedForm = normalizer.Normalize(zoos);
        }

        [Fact]
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
            
            Assert.Equal("Animals~", normalizedForm[0][0][0]);
            Assert.Equal(new List<object>() { "Age", 20, 3, 16 }, normalizedForm[0][0][1]);
            Assert.Equal(new List<object>() { "AnimalId" }, normalizedForm[0][0][2]);
            Assert.Equal(new List<object>() { "Name", "Tony", "Tania", "Zachary" }, normalizedForm[0][0][3]);
            Assert.Equal(new List<object>() { "Type", "Tiger", "Zebra" }, normalizedForm[0][0][4]);

            Assert.Equal(new List<object>() { "Name" }, normalizedForm[0][1]);

            Assert.Equal(new List<object>() { 1, 101, 1, 1 }, ((List<List<object>>)normalizedForm[1][0][0])[0]);
            Assert.Equal(new List<object>() { 1, 102, 2, 1 }, ((List<List<object>>)normalizedForm[1][0][0])[1]);
            Assert.Equal(new List<object>() { 2, 103, 3, 2 }, ((List<List<object>>)normalizedForm[1][0][0])[2]);
            Assert.Equal("Columbus Zoo and Aquarium", normalizedForm[1][0][1]);


            Assert.Equal(new List<object>() { 3, 104, 1, 1 }, ((List<List<object>>)normalizedForm[1][1][0])[0]);
            Assert.Equal("Cincinnati Zoo and Botanical Garden", normalizedForm[1][1][1]);
        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(zoos);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }

    public class Normalize_With_A_Type_That_Is_A_List
    {
        private readonly List<List<Book>> libraries;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_With_A_Type_That_Is_A_List()
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
                    new Book() {Title = "The Return of the King", Author = "J.R.R. Tolkien", PublishDate = new DateTime(1955, 10, 20),
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

        [Fact]
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof (List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal("~", normalizedForm[0][0][0]);
            Assert.Equal(new List<object>() { "Author", "J.R.R. Tolkien", "Jim Butcher" }, normalizedForm[0][0][1]);
            Assert.Equal(new List<object>() { "HasRead", true, false }, normalizedForm[0][0][2]);
            Assert.Equal(new List<object>() { "PublishDate" }, normalizedForm[0][0][3]);
            Assert.Equal(new List<object>() { "PurchaseLocation", "Barnes and Noble", "Amazon" }, normalizedForm[0][0][4]);
            Assert.Equal(new List<object>() { "PurchaseYear", 2000, 2015, 2016 }, normalizedForm[0][0][5]);
            Assert.Equal(new List<object>() { "Series", "The Lord of the Rings", "The Dresden Files" }, normalizedForm[0][0][6]);
            Assert.Equal(new List<object>() { "Title" }, normalizedForm[0][0][7]);

            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 7, 29), 1, 1, 1, "The Fellowship of the Ring" }, ((List<List<object>>)normalizedForm[1][0][0])[0]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1954, 11, 11), 1, 1, 1, "The Two Towers" }, ((List<List<object>>)normalizedForm[1][0][0])[1]);
            Assert.Equal(new List<object>() { 1, 1, new DateTime(1955, 10, 20), 1, 1, 1, "The Return of the King" }, ((List<List<object>>)normalizedForm[1][0][0])[2]);

            Assert.Equal(new List<object>() { 2, 1, new DateTime(2000, 4, 1), 2, 2, 2, "Storm Front" }, ((List<List<object>>)normalizedForm[1][1][0])[0]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2001, 1, 1), 2, 2, 2, "Fool Moon" }, ((List<List<object>>)normalizedForm[1][1][0])[1]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2001, 9, 1), 2, 2, 2, "Grave Peril" }, ((List<List<object>>)normalizedForm[1][1][0])[2]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2002, 2, 2), 2, 2, 2, "Summer Knight" }, ((List<List<object>>)normalizedForm[1][1][0])[3]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2003, 8, 5), 2, 2, 2, "Death Masks" }, ((List<List<object>>)normalizedForm[1][1][0])[4]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2004, 8, 2), 2, 2, 2, "Blood Rites" }, ((List<List<object>>)normalizedForm[1][1][0])[5]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2005, 5, 3), 2, 2, 2, "Dead Beat" }, ((List<List<object>>)normalizedForm[1][1][0])[6]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2006, 5, 2), 2, 2, 2, "Proven Guilty" }, ((List<List<object>>)normalizedForm[1][1][0])[7]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2007, 4, 3), 2, 2, 2, "White Night" }, ((List<List<object>>)normalizedForm[1][1][0])[8]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2008, 4, 1), 2, 3, 2, "Small Favor" }, ((List<List<object>>)normalizedForm[1][1][0])[9]);
            Assert.Equal(new List<object>() { 2, 1, new DateTime(2009, 4, 7), 2, 3, 2, "Turn Coat" }, ((List<List<object>>)normalizedForm[1][1][0])[10]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2010, 4, 6), 2, 3, 2, "Changes" }, ((List<List<object>>)normalizedForm[1][1][0])[11]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2011, 4, 26), 2, 3, 2, "Ghost Story" }, ((List<List<object>>)normalizedForm[1][1][0])[12]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2012, 11, 27), 2, 3, 2, "Cold Days" }, ((List<List<object>>)normalizedForm[1][1][0])[13]);
            Assert.Equal(new List<object>() { 2, 2, new DateTime(2014, 5, 27), 2, 3, 2, "Skin Game" }, ((List<List<object>>)normalizedForm[1][1][0])[14]);
            
        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(libraries);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }

    public class Normalize_With_A_Type_Containing_A_Circular_Reference
    {
        private readonly List<Person> people;
        private readonly Normalizer normalizer;

        public Normalize_With_A_Type_Containing_A_Circular_Reference()
        {
            normalizer = new Normalizer();

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
        }

        [Fact]
        public void Should_Throw_An_Exception()
        {
            Exception exception = Assert.ThrowsAny<Exception>(() => normalizer.Normalize(people));

            Assert.Equal("Circular Reference Detected in object.", exception.Message);
        }
    }

    public class Normalize_An_Object_With_A_List_Of_An_Interface_That_Contains_Multiple_Types
    {
        private readonly List<Garage> garages;
        private readonly List<List<List<object>>> normalizedForm;

        public Normalize_An_Object_With_A_List_Of_An_Interface_That_Contains_Multiple_Types()
        {
            Normalizer normalizer = new Normalizer();

            garages = new List<Garage>()
            {
                new Garage() {Stall1 = new Car() {Color = "Red", Seats = 4, Tires = 4}, Stall2 = new Motorcycle() {Color = "Green", Tires = 2, HandleBarType = "Leather"}},
                new Garage() {Stall1 = new Motorcycle() {Color = "Black", HandleBarType = "Plastic", Tires = 2}, Stall2 = new Motorcycle() {Color = "Green", Tires = 2, HandleBarType = "Plastic"}},
                new Garage() {Stall1 = new Car() {Color = "Green", Seats = 4, Tires = 4}, Stall2 = new Motorcycle() {Color = "Pink", Tires = 2, HandleBarType = "Tasselled"}}
            };

            normalizedForm = normalizer.Normalize(garages);
        }

        [Fact]
        public void Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);

            Assert.Equal("Stall1.", normalizedForm[0][0][0]);
            Assert.Equal(new List<object>() { "Color" }, normalizedForm[0][0][1]);
            Assert.Equal(new List<object>() { "Tires", 4, 2 }, normalizedForm[0][0][2]);

            Assert.Equal("Stall2.", normalizedForm[0][1][0]);
            Assert.Equal(new List<object>() { "Color", "Green", "Pink" }, normalizedForm[0][1][1]);
            Assert.Equal(new List<object>() { "Tires", 2 }, normalizedForm[0][1][2]);

            Assert.Equal(new List<object>() { "Red", 1 }, normalizedForm[1][0][0]);
            Assert.Equal(new List<object>() { 1, 1 }, normalizedForm[1][0][1]);
            Assert.Equal(new List<object>() { "Black", 2 }, normalizedForm[1][1][0]);
            Assert.Equal(new List<object>() { 1, 1 }, normalizedForm[1][1][1]);
            Assert.Equal(new List<object>() { "Green", 1 }, normalizedForm[1][2][0]);
            Assert.Equal(new List<object>() { 2, 1 }, normalizedForm[1][2][1]);
        }

        [Fact]
        public void Should_Reduce_The_String_Length_When_Serialized()
        {
            string normalizedJson = JsonConvert.SerializeObject(normalizedForm);
            string serializedJson = JsonConvert.SerializeObject(garages);

            Assert.True(normalizedJson.Length < serializedJson.Length);
        }
    }
}
