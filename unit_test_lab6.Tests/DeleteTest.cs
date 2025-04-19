namespace unit_test_lab6.Tests;

[TestClass]
public class DeleteTests
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
    public async Task DeleteBook()
    {
        await lib.AddBook("To Delete", "Author", "999");
        int bookId = lib.books[0].Id;

        var result = await lib.DeleteBook(bookId);

        Assert.IsTrue(result);
        Assert.AreEqual(0, lib.books.Count);
        Assert.IsFalse(File.ReadAllText(BooksFile).Contains("To Delete"));
    }

    [TestMethod]
    public async Task DeleteBook_BadId()
    {
        var result = await lib.DeleteBook(999);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task DeleteUser()
    {
        await lib.AddUser("Remove Me", "remove@example.com");
        int userId = lib.users[0].Id;

        var result = await lib.DeleteUser(userId);

        Assert.IsTrue(result);
        Assert.AreEqual(0, lib.users.Count);
        Assert.IsFalse(File.ReadAllText(UsersFile).Contains("Remove Me"));
    }

    [TestMethod]
    public async Task DeleteUser_BadId()
    {
        var result = await lib.DeleteUser(999);
        Assert.IsFalse(result);
    }
}
