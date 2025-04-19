namespace unit_test_lab6.Tests;

[TestClass]
public class UpdateTests
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
    public async Task UpdateBook()
    {
        await lib.AddBook("Old Title", "Old Author", "111");
        int id = lib.books[0].Id;

        var result = await lib.EditBook(id, "New Title", "New Author", "222");

        Assert.IsTrue(result);
        Assert.AreEqual("New Title", lib.books[0].Title);
        Assert.AreEqual("New Author", lib.books[0].Author);
        Assert.AreEqual("222", lib.books[0].ISBN);
    }

    [TestMethod]
    public async Task UpdateBook_BadId()
    {
        var result = await lib.EditBook(999, "Title", "Author", "123");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task UpdateUser()
    {
        await lib.AddUser("Old Name", "old@gmail.com");
        int id = lib.users[0].Id;

        var result = await lib.EditUser(id, "New Name", "new@gmail.com");

        Assert.IsTrue(result);
        Assert.AreEqual("New Name", lib.users[0].Name);
        Assert.AreEqual("new@gmail.com", lib.users[0].Email);
    }

    [TestMethod]
    public async Task UpdateUser_BadId()
    {
        var result = await lib.EditUser(999, "user2", "user2@gmail.com");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task UpdateBorrowedBooks()
    {
        await lib.AddUser("user1", "user1@gmail.com");
        await lib.AddBook("title1", "title1", "123");
        int userId = lib.users[0].Id;
        int bookId = lib.books[0].Id;

        var result = await lib.BorrowBook(userId, bookId);

        Assert.IsTrue(result);
        Assert.AreEqual(0, lib.books.Count);
        Assert.IsTrue(lib.borrowedBooks.ContainsKey(lib.users[0]));
        Assert.AreEqual(1, lib.borrowedBooks[lib.users[0]].Count);
    }

    [TestMethod]
    public async Task ReturnBorrowedBook()
    {
        await lib.AddUser("user1", "user1@gmail.com");
        await lib.AddBook("title1", "author1", "123");
        int userId = lib.users[0].Id;
        int bookId = lib.books[0].Id;

        await lib.BorrowBook(userId, bookId);
        var result = await lib.ReturnBook(userId, bookId);

        Assert.IsTrue(result);
        Assert.AreEqual(1, lib.books.Count);
        Assert.IsFalse(lib.borrowedBooks[lib.users[0]].Any());
    }

    [TestMethod]
    public async Task ReturnBorrowedBook_BadBookId()
    {
        await lib.AddUser("user1", "user1@gmail.com");
        int userId = lib.users[0].Id;

        var result = await lib.ReturnBook(userId, 999);
        Assert.IsFalse(result);
    }
}
