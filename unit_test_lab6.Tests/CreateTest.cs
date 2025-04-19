namespace unit_test_lab6.Tests;

[TestClass]
public class CreateTests
{
    LibraryService lib = new();
    string BooksFile = "../../../../Data/Books.csv";
    string UsersFile = "../../../../Data/Users.csv";
    [TestInitialize]
    public void Setup()
    {
        File.WriteAllText(BooksFile, "");
        File.WriteAllText(UsersFile, "");
    }

    [TestMethod]
    public async Task CreateBook()
    {
        var result = await lib.AddBook("title1", "author1", "1234");
        await lib.AddBook("title2", "author2", "4567");
        Assert.IsTrue(result);
        Assert.AreEqual(2, lib.books.Count);
        Assert.AreEqual("title1", lib.books[0].Title);
    }

    [TestMethod]
    public async Task CreateBook_BadInput()
    {
        var result = await lib.AddBook("", "Author", "123");
        Assert.IsFalse(result);

        result = await lib.AddBook("Title", "", "123");
        Assert.IsFalse(result);

        result = await lib.AddBook("Title", "Author", "");
        Assert.IsFalse(result);

        Assert.AreEqual(0, lib.books.Count);
    }

    [TestMethod]
    public async Task CreateUser()
    {
        var result = await lib.AddUser("user1", "user1@gmail.com");
        await lib.AddUser("user2", "user2@gmail.com");

        Assert.IsTrue(result);
        Assert.AreEqual(2, lib.users.Count);
        Assert.AreEqual("user1", lib.users[0].Name);
    }

    [TestMethod]
    public async Task CreateUser_BadInput()
    {
        var result = await lib.AddUser("", "user1@gmail.com");
        Assert.IsFalse(result);

        result = await lib.AddUser("user1", "");
        Assert.IsFalse(result);

        Assert.AreEqual(0, lib.users.Count);
    }
}
