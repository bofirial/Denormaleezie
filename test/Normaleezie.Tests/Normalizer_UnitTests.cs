using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy.Configuration;
using Normaleezie.Tests.Test_Classes;
using Normaleezie;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace nEZ.Unit
{
    #region Normalize
    public class When_Calling_Normalize_With_A_Null_Object
    {
        private readonly List<List<List<object>>> normalizedForm;

        public When_Calling_Normalize_With_A_Null_Object()
        {
            Normalizer normalizer = new Normalizer();

            normalizedForm = normalizer.Normalize<List<object>>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_List()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
        }
    }

    public class When_Calling_Normalize_With_A_List
    {
        private readonly List<List<List<object>>> normalizedForm;
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<List<List<object>>> convertToNormalizedFormReturnValue;

        public When_Calling_Normalize_With_A_List()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.Normalize(A<List<Animal>>.Ignored)).CallsBaseMethod();

            convertToNormalizedFormReturnValue = new List<List<List<object>>>()
            {
                new List<List<object>>()
                {
                    new List<object>()
                    {
                        "denormaleezie"
                    }
                }
            };

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored)).Returns(convertToNormalizedFormReturnValue);

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                }
            };

            normalizedForm = normalizer.Normalize(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
        }

        [Fact]
        public void It_Should_Return_The_Value_Returned_From_ConvertToNormalizedForm()
        {
            Assert.Equal(convertToNormalizedFormReturnValue, normalizedForm);
        }

        [Fact]
        public void It_Should_Call_ConvertToNormalizedForm()
        {
            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
    #endregion

    #region ConvertToNormalizedForm
    public class When_Calling_ConvertToNormalizedForm_With_Null
    {
        private readonly Normalizer normalizer;

        public When_Calling_ConvertToNormalizedForm_With_Null()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.ConvertToNormalizedForm<object>(null));
        }
    }

    public class When_Calling_ConvertToNormalizedForm
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<List<List<object>>> normalizedForm;
        private readonly List<List<object>> normalizedDataList;
        private readonly List<List<object>> normalizedStructureList;

        public When_Calling_ConvertToNormalizedForm()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored)).CallsBaseMethod();

            normalizedDataList = new List<List<object>>()
            {
                new List<object>() { "Id"},
                new List<object>() { "Name", "Tony", "Tania", "Talia"},
                new List<object>() { "Age", 21, 1, 3},
                new List<object>() { "Type", "Tiger" }
            };

            normalizedStructureList = new List<List<object>>()
            {
                new List<object>() {1, 1, 1, 1 },
                new List<object>() {2, 2, 2, 1 },
                new List<object>() {3, 3, 3, 1 }
            };

            A.CallTo(() => normalizer.CreateNormalizedDataList(A<List<Animal>>.Ignored)).Returns(normalizedDataList);

            A.CallTo(() => normalizer.CreateNormalizedStructureList(A<List<Animal>>.Ignored, A<List<List<object>>>.Ignored))
                .Returns(normalizedStructureList);

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                }
            };

            normalizedForm = normalizer.ConvertToNormalizedForm(animals);
        }

        [Fact]
        public void It_Should_Return_The_NormalizedForm()
        {
            Assert.Equal(normalizedDataList, normalizedForm[0]);
            Assert.Equal(normalizedStructureList, normalizedForm[1]);
        }

        [Fact]
        public void It_Should_Call_CreateNormalizedDataList()
        {
            A.CallTo(() => normalizer.CreateNormalizedDataList(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Call_CreateNormalizedStructureList()
        {
            A.CallTo(() => normalizer.CreateNormalizedStructureList(A<List<Animal>>.Ignored, A<List<List<object>>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == normalizedDataList)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
    #endregion

    #region CreateNormalizedDataList
    public class When_Calling_CreateNormalizedDataList_With_Null
    {
        private readonly Normalizer normalizer;

        public When_Calling_CreateNormalizedDataList_With_Null()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedDataList<object>(null));
        }
    }

    public class When_Calling_CreateNormalizedDataList_With_A_List_Of_Flat_Objects
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<List<object>> normalizedDataList;
        private readonly List<List<List<object>>> getNormalizedDataForPropertyReturnValues;

        public When_Calling_CreateNormalizedDataList_With_A_List_Of_Flat_Objects()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.CreateNormalizedDataList(A<List<Animal>>.Ignored)).CallsBaseMethod();

            getNormalizedDataForPropertyReturnValues = new List<List<List<object>>>()
            {
                new List<List<object>>() {
                    new List<object>() {"Name" }
                },
                new List<List<object>>() {
                    new List<object>() {"AnimalId" }
                },
                new List<List<object>>() {
                    new List<object>() {"Age", 21, 12 }
                },
                new List<List<object>>() {
                    new List<object>() {"Type", "Tiger" }
                }
            };

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
                .ReturnsNextFromSequence(getNormalizedDataForPropertyReturnValues.ToArray());

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal(),
                new Animal()
            };

            normalizedDataList = normalizer.CreateNormalizedDataList(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_For_Every_Property()
        {
            Assert.Equal(typeof(Animal).GetProperties().Count(), normalizedDataList.Count);
        }

        [Fact]
        public void It_Should_Call_GetNormalizedDataForProperty_For_Each_Property_In_The_DenormalizedListItemType()
        {
            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Times(typeof(Animal).GetProperties().Count()));
        }

        [Fact]
        public void It_Should_Return_The_Data_From_GetNormalizedDataForProperty_Sorted_By_PropertyName()
        {
            Assert.Equal(new List<List<object>>()
            {
                getNormalizedDataForPropertyReturnValues[2][0],
                getNormalizedDataForPropertyReturnValues[1][0],
                getNormalizedDataForPropertyReturnValues[0][0],
                getNormalizedDataForPropertyReturnValues[3][0],
            }, normalizedDataList);
        }
    }
    #endregion

    #region GetNormalizedDataForProperty

    public class When_Calling_GetNormalizedDataForProperty_With_A_Null_Object_List
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetNormalizedDataForProperty_With_A_Null_Object_List()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedDataForProperty<object>(null, typeof(Animal).GetProperty("AnimalId")));
        }
    }
    public class When_Calling_GetNormalizedDataForProperty_With_A_Null_PropertyInfo
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetNormalizedDataForProperty_With_A_Null_PropertyInfo()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedDataForProperty(new List<Animal>(), null));
        }
    }

    public class When_Calling_GetNormalizedDataForProperty_With_A_Property_That_Has_No_Duplicate_Values
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly PropertyInfo propInfo;
        private readonly List<List<object>> normalizedDataForProperty;

        public When_Calling_GetNormalizedDataForProperty_With_A_Property_That_Has_No_Duplicate_Values()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(false);

            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .Returns(new List<object>() { 1, 2, 3 });

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal(),
                new Animal()
            };

            propInfo = typeof(Animal).GetProperty("AnimalId");

            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(animals, propInfo);
        }

        [Fact]
        public void It_Should_Call_IsSimpleType()
        {
            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
                .MustHaveHappened();
        }

        [Fact]
        public void It_Should_Call_GetUniquePropertyValues()
        {
            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && (PropertyInfo)args[1] == propInfo)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Return_Only_The_Property_Name()
        {
            Assert.Equal(new List<List<object>>()
            {
                new List<object>() {"AnimalId" }
            }, normalizedDataForProperty);
        }
    }

    public class When_Calling_GetNormalizedDataForProperty_With_A_Property_That_Has_Duplicate_Values
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly PropertyInfo propInfo;
        private readonly List<List<object>> normalizedDataForProperty;

        public When_Calling_GetNormalizedDataForProperty_With_A_Property_That_Has_Duplicate_Values()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(false);

            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .Returns(new List<object>() { "Tiger" });

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal(),
                new Animal()
            };

            propInfo = typeof(Animal).GetProperty("Type");

            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(animals, propInfo);
        }

        [Fact]
        public void It_Should_Call_IsSimpleType()
        {
            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
                .MustHaveHappened();
        }

        [Fact]
        public void It_Should_Call_GetUniquePropertyValues()
        {
            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && (PropertyInfo)args[1] == propInfo)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Return_The_Property_Name_And_The_Unique_Values()
        {
            Assert.Equal(new List<List<object>>()
            {
                new List<object>() {"Type", "Tiger" }
            }, normalizedDataForProperty);
        }
    }

    public class When_Calling_GetNormalizedDataForProperty_With_A_DateTime_Property_And_A_PropertyNamePrefix
    {
        private readonly Normalizer normalizer;
        private readonly List<Book> books;
        private readonly PropertyInfo propInfo;
        private readonly List<List<object>> normalizedDataForProperty;

        public When_Calling_GetNormalizedDataForProperty_With_A_DateTime_Property_And_A_PropertyNamePrefix()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<Book>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(false);

            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Book>>.Ignored, A<PropertyInfo>.Ignored))
                .Returns(new List<object>() { new DateTime(2015, 1, 1) });

            books = new List<Book>()
            {
                new Book()
                {
                },
                new Book(),
                new Book()
            };

            propInfo = typeof(Book).GetProperty("PublishDate");

            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(books, propInfo, "ThisIsAPrefix.");
        }

        [Fact]
        public void It_Should_Call_IsSimpleType()
        {
            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
                .MustHaveHappened();
        }

        [Fact]
        public void It_Should_NOT_Call_GetNormalizedDataForProperty_For_The_DateTimes()
        {
            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<DateTime>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void It_Should_Call_GetUniquePropertyValues()
        {
            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Book>>.Ignored, A<PropertyInfo>.Ignored))
                .WhenArgumentsMatch(args => args[0] == books && (PropertyInfo)args[1] == propInfo)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Return_The_Property_Name_And_The_Unique_Values()
        {
            Assert.Equal(new List<List<object>>()
            {
                new List<object>() { "ThisIsAPrefix.PublishDate", new DateTime(2015, 1, 1) }
            }, normalizedDataForProperty);
        }
    }

    public class When_Calling_GetNormalizedDataForProperty_With_A_Reference_Property
    {
        private readonly Normalizer normalizer;
        private readonly List<BookMark> bookMarks;
        private readonly PropertyInfo propInfo;
        private readonly List<List<object>> normalizedDataForProperty;
        private readonly List<List<List<object>>> getNormalizedDataForPropertySubPropertyReturnValues;

        public When_Calling_GetNormalizedDataForProperty_With_A_Reference_Property()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<BookMark>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(true);

            getNormalizedDataForPropertySubPropertyReturnValues = new List<List<List<object>>>()
            {
                new List<List<object>>() { new List<object>() { "Book.Title", "The Fellowship of the Ring", "The Two Towers" } },
                new List<List<object>>() { new List<object>() { "Book.Author", "J.R.R. Tolkien" } },
                new List<List<object>>() { new List<object>() { "Book.PublishDate", new DateTime(1954, 11, 11) } },
                new List<List<object>>() { new List<object>() { "Book.Series", "The Lord of the Rings" } },
                new List<List<object>>() { new List<object>() { "Book.PurchaseYear", 2000 } },
                new List<List<object>>() { new List<object>() { "Book.PurchaseLocation", "Barnes and Noble" } },
                new List<List<object>>() { new List<object>() { "Book.HasRead", true } }
            };

            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
                .ReturnsNextFromSequence(getNormalizedDataForPropertySubPropertyReturnValues.ToArray());

            bookMarks = new List<BookMark>()
            {
                new BookMark()
                {
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
                new BookMark()
                {
                    CurrentPage = 1,
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
                new BookMark()
                {
                    CurrentPage = 1,
                    Book = new Book()
                    {
                        Title = "The Two Towers",
                        Author = "J.R.R. Tolkien",
                        PublishDate = new DateTime(1954, 11, 11),
                        Series = "The Lord of the Rings",
                        PurchaseYear = 2000,
                        PurchaseLocation = "Barnes and Noble",
                        HasRead = true
                    }
                }
            };

            propInfo = typeof(BookMark).GetProperty("Book");

            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(bookMarks, propInfo, "Test.");
        }

        [Fact]
        public void It_Should_Call_IsSimpleType()
        {
            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
                .MustHaveHappened();
        }

        [Fact]
        public void It_Should_NOT_Call_GetUniquePropertyValues()
        {
            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<BookMark>>.Ignored, A<PropertyInfo>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void It_Should_Call_GetNormalizedDataForProperty()
        {
            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[2].ToString() == "Test.Book.")
                .MustHaveHappened(Repeated.Exactly.Times(typeof(Book).GetProperties().Count()));
        }

        [Fact]
        public void It_Should_Return_The_Data_From_The_SubProperty_GetNormalizedDataForProperty_Calls()
        {
            Assert.Equal(getNormalizedDataForPropertySubPropertyReturnValues.Select(i => i.First()).ToList(), normalizedDataForProperty);
        }
    }

    #endregion

    #region IsSimpleType
    public class When_Calling_IsSimpleType_With_A_Null_PropertyInfo
    {
        private readonly Normalizer normalizer;

        public When_Calling_IsSimpleType_With_A_Null_PropertyInfo()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.IsSimpleType(null));
        }
    }

    public class When_Calling_IsSimpleType_With_An_Int
    {
        private readonly bool isSimpleType;

        public When_Calling_IsSimpleType_With_An_Int()
        {
            Normalizer normalizer = new Normalizer();

            isSimpleType = normalizer.IsSimpleType(typeof(string));
        }

        [Fact]
        public void It_Should_Return_False()
        {
            Assert.False(isSimpleType);
        }
    }

    public class When_Calling_IsSimpleType_With_A_DateTime
    {
        private readonly bool isSimpleType;

        public When_Calling_IsSimpleType_With_A_DateTime()
        {
            Normalizer normalizer = new Normalizer();

            isSimpleType = normalizer.IsSimpleType(typeof(DateTime));
        }

        [Fact]
        public void It_Should_Return_False()
        {
            Assert.False(isSimpleType);
        }
    }

    public class When_Calling_IsSimpleType_With_An_Object_Property
    {
        private readonly bool isSimpleType;

        public When_Calling_IsSimpleType_With_An_Object_Property()
        {
            Normalizer normalizer = new Normalizer();

            isSimpleType = normalizer.IsSimpleType(typeof(Book));
        }

        [Fact]
        public void It_Should_Return_True()
        {
            Assert.True(isSimpleType);
        }
    }
    #endregion

    #region GetUniquePropertyValues
    public class When_Calling_GetUniquePropertyValues_With_A_Null_Object_List
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetUniquePropertyValues_With_A_Null_Object_List()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetUniquePropertyValues<object>(null, typeof(Animal).GetProperty("AnimalId")));
        }
    }
    public class When_Calling_GetUniquePropertyValues_With_A_Null_PropertyInfo
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetUniquePropertyValues_With_A_Null_PropertyInfo()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetUniquePropertyValues(new List<Animal>(), null));
        }
    }

    public class When_Calling_GetUniquePropertyValues_For_A_Property_With_No_Duplicates
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<object> uniquePropertyValues;

        public When_Calling_GetUniquePropertyValues_For_A_Property_With_No_Duplicates()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal()
                {
                    AnimalId = 2,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tania"
                },
                new Animal()
                {
                    AnimalId = 3,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tali"
                }
            };

            PropertyInfo propInfo = typeof(Animal).GetProperty("AnimalId");

            uniquePropertyValues = normalizer.GetUniquePropertyValues(animals, propInfo);
        }

        [Fact]
        public void It_Should_Return_A_String_For_Every_ListItem()
        {
            Assert.Equal(animals.Count, uniquePropertyValues.Count);
        }

        [Fact]
        public void The_List_Should_Contain_The_Property_Values()
        {
            Assert.Equal(1, (int)uniquePropertyValues[0]);
            Assert.Equal(2, (int)uniquePropertyValues[1]);
            Assert.Equal(3, (int)uniquePropertyValues[2]);
        }
    }
    public class When_Calling_GetUniquePropertyValues_For_A_Property_With_Duplicates
    {
        private readonly Normalizer normalizer;
        private readonly List<object> uniquePropertyValues;

        public When_Calling_GetUniquePropertyValues_For_A_Property_With_Duplicates()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

            List<Animal> animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal()
                {
                    AnimalId = 2,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tania"
                },
                new Animal()
                {
                    AnimalId = 3,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tali"
                }
            };

            PropertyInfo propInfo = typeof(Animal).GetProperty("Type");

            uniquePropertyValues = normalizer.GetUniquePropertyValues(animals, propInfo);
        }

        [Fact]
        public void It_Should_Return_A_String_For_Every_Unique_ListItem()
        {
            Assert.Equal(1, uniquePropertyValues.Count);
        }

        [Fact]
        public void The_List_Should_Contain_Only_The_Unique_Property_Values()
        {
            Assert.Equal("Tiger", (string)uniquePropertyValues[0]);
        }
    }
    #endregion

    #region CreateNormalizedStructureList
    public class When_Calling_CreateNormalizedStructureList_With_A_Null_DenormalizedList
    {
        private readonly Normalizer normalizer;

        public When_Calling_CreateNormalizedStructureList_With_A_Null_DenormalizedList()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureList<object>(null, new List<List<object>>()));
        }
    }
    public class When_Calling_CreateNormalizedStructureList_With_A_Null_NormalizedDataList
    {
        private readonly Normalizer normalizer;

        public When_Calling_CreateNormalizedStructureList_With_A_Null_NormalizedDataList()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureList(new List<Animal>(), null));
        }
    }

    public class When_Calling_CreateNormalizedStructureList
    {
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<List<object>> normalizedStructureList;
        private readonly List<List<object>> createNormalizedStructureItemReturnValues;

        public When_Calling_CreateNormalizedStructureList()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.CreateNormalizedStructureList(A<List<Animal>>.Ignored, A<List<List<object>>>.Ignored)).CallsBaseMethod();

            createNormalizedStructureItemReturnValues = new List<List<object>>()
            {
                new List<object>() { 1, 2, 3, 4 },
                new List<object>() { 5, 6, 7, 8 },
                new List<object>() { 9, 9, 9, 9 }
            };

            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored))
                .ReturnsNextFromSequence(createNormalizedStructureItemReturnValues.ToArray());

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal()
                {
                    AnimalId = 2,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tania"
                },
                new Animal()
                {
                    AnimalId = 3,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tali"
                }
            };

            List<List<object>> normalizedDataList = new List<List<object>>()
            {
                new List<object>() { "AnimalId" },
                new List<object>() { "Type" },
                new List<object>() { "Age" },
                new List<object>() { "Name" }
            };

            normalizedStructureList = normalizer.CreateNormalizedStructureList(animals, normalizedDataList);
        }

        [Fact]
        public void It_Should_Call_CreateNormalizedStructureItem_Once_For_Each_Item_In_The_NormalizedDataList()
        {
            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(animals.Count));
        }

        [Fact]
        public void It_Should_Return_The_Data_From_CreateNormalizedStructureItem()
        {
            foreach (var createNormalizedStructureItemReturnValue in createNormalizedStructureItemReturnValues)
            {
                Assert.Contains(normalizedStructureList, dsl => dsl == createNormalizedStructureItemReturnValue);
            }
        }
    }
    #endregion

    #region CreateNormalizedStructureItem
    public class When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedList
    {
        private readonly Normalizer normalizer;

        public When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedList()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureItem<object>(null, new List<List<object>>()));
        }
    }
    public class When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedData
    {
        private readonly Normalizer normalizer;

        public When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedData()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureItem(new Animal(), null));
        }
    }

    public class When_Calling_CreateNormalizedStructureItem
    {
        private readonly Normalizer normalizer;
        private readonly List<List<object>> normalizedDataList;
        private readonly List<object> normalizedStructureItem;
        private readonly List<object> getNormalizedItemPropertyObjectReturnValues;

        public When_Calling_CreateNormalizedStructureItem()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored)).CallsBaseMethod();

            getNormalizedItemPropertyObjectReturnValues = new List<object>()
            {
                101, 1, 1, "Tony"
            };

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored))
                .ReturnsNextFromSequence(getNormalizedItemPropertyObjectReturnValues.ToArray());

            Animal animal = new Animal()
            {
                AnimalId = 101,
                Type = "Tiger",
                Age = 21,
                Name = "Tony"
            };

            normalizedDataList = new List<List<object>>()
            {
                new List<object>() { "AnimalId" },
                new List<object>() { "Type", "Tiger", "Hippo" },
                new List<object>() { "Age", 21 },
                new List<object>() { "Name"}
            };

            normalizedStructureItem = normalizer.CreateNormalizedStructureItem(animal, normalizedDataList);
        }

        [Fact]
        public void It_Should_Call_GetNormalizedItemPropertyObject_Once_For_Each_Item_In_The_DenormalizedData()
        {
            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(normalizedDataList.Count));
        }

        [Fact]
        public void It_Should_Return_The_Data_From_GetNormalizedItemPropertyObject()
        {
            foreach (var getNormalizedItemPropertyObjectReturnValue in getNormalizedItemPropertyObjectReturnValues)
            {
                Assert.Contains(normalizedStructureItem, dataStructureObject => dataStructureObject == getNormalizedItemPropertyObjectReturnValue);
            }
        }
    }
    #endregion

    #region GetNormalizedItemPropertyObject
    public class When_Calling_GetNormalizedItemPropertyObject_With_A_Null_DenormalizedItem
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetNormalizedItemPropertyObject_With_A_Null_DenormalizedItem()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedItemPropertyObject<object>(null, new List<object>()));
        }
    }
    public class When_Calling_GetNormalizedItemPropertyObject_With_A_Null_NormalizedPropertyData
    {
        private readonly Normalizer normalizer;

        public When_Calling_GetNormalizedItemPropertyObject_With_A_Null_NormalizedPropertyData()
        {
            normalizer = new Normalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedItemPropertyObject(new Animal(), null));
        }
    }

    public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_No_NormalizedPropertyData_Other_Than_The_PropertyName
    {
        private readonly Normalizer normalizer;
        private readonly object dataStructureObject;

        public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_No_NormalizedPropertyData_Other_Than_The_PropertyName()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

            Animal animal = new Animal()
            {
                AnimalId = 101,
                Type = "Tiger",
                Age = 21,
                Name = "Tony"
            };

            List<object> normalizedPropertyData = new List<object>() { "Name" };

            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData);
        }

        [Fact]
        public void It_Should_Return_The_Property_Value()
        {
            Assert.Equal("Tony", (string)dataStructureObject);
        }
    }

    public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_Missing_In_The_NormalizedPropertyData
    {
        private readonly Normalizer normalizer;
        private readonly Animal animal;
        private readonly List<object> normalizedPropertyData;

        public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_Missing_In_The_NormalizedPropertyData()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

            animal = new Animal()
            {
                AnimalId = 101,
                Type = "Tiger",
                Age = 21,
                Name = "Tony"
            };

            normalizedPropertyData = new List<object>() { "Type", "Hippo" };
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(InvalidOperationException), () => normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData));
        }
    }

    public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_In_The_NormalizedPropertyData
    {
        private readonly Normalizer normalizer;
        private readonly object dataStructureObject;

        public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_In_The_NormalizedPropertyData()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

            Animal animal = new Animal()
            {
                AnimalId = 101,
                Type = "Tiger",
                Age = 21,
                Name = "Tony"
            };

            List<object> normalizedPropertyData = new List<object>() { "Type", "Tiger", "Hippo" };

            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData);
        }

        [Fact]
        public void It_Should_Return_The_Index_Of_The_Property_Value()
        {
            Assert.Equal(1, (int)dataStructureObject);
        }
    }

    public class When_Calling_GetNormalizedItemPropertyObject_For_A_SubProperty_From_Another_Property
    {
        private readonly Normalizer normalizer;
        private readonly Book book;
        private readonly object dataStructureObject;

        public When_Calling_GetNormalizedItemPropertyObject_For_A_SubProperty_From_Another_Property()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<BookMark>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<object>.Ignored, A<List<object>>.Ignored))
                .Returns(2);

            book = new Book()
            {
                Title = "Changes",
                Author = "Jim Butcher",
                PublishDate = new DateTime(2010, 4, 6),
                Series = "The Dresden Files",
                PurchaseYear = 2016,
                PurchaseLocation = "Amazon",
                HasRead = false
            };

            BookMark bookMark = new BookMark()
            {
                CurrentPage = 300,
                Book = book
            };

            List<object> normalizedPropertyData = new List<object>() { "Book.Title.Test" };

            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(bookMark, normalizedPropertyData);
        }

        [Fact]
        public void It_Should_Call_GetNormalizedItemPropertyObject_For_The_Subproperty()
        {
            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<object>.Ignored, A<List<object>>.Ignored))
                .WhenArgumentsMatch(args =>  args[0] == book && ((List<object>)args[1])[0].ToString() == "Title.Test")
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Return_The_Value_From_Calling_GetNormalizedItemPropertyObject()
        {
            Assert.Equal(2, dataStructureObject);
        }
    }
    #endregion
}
