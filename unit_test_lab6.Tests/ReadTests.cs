namespace unit_test_lab6.Tests;

[TestClass]
public class ReadTests
{

    LibraryService lib = new LibraryService();
    string BooksFile = "../../../../Data/Books.csv";
    string UsersFile = "../../../../Data/Users.csv";
    string BorrowedFile = "../../../../Data/Borrowed.csv";


    [TestMethod]
    public async Task TestReadBooks()
    {
        File.WriteAllText(BooksFile, "1,title1,author1,123\n2,title2,author2,456");

        await lib.ReadBooks();

        Assert.AreEqual(2, lib.books.Count);
        Assert.AreEqual("title1", lib.books[0].Title);
        Assert.AreEqual("author1", lib.books[0].Author);
    }

    [TestMethod]
    public async Task TestReadBooks_Empty()
    {
        File.WriteAllText(BooksFile, "");

        await lib.ReadBooks();

        Assert.AreEqual(0, lib.books.Count);
    }

    [TestMethod]
    public async Task TestReadBooks_BadData()
    {
        File.WriteAllText(BooksFile, "1 , title1, author1");

        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
        {
            await lib.ReadBooks();
            var _ = lib.books[0];
        });
    }

    [TestMethod]
    public async Task TestReadBooks_Spaces()
    {
        File.WriteAllText(BooksFile, "1 , title1, author1 , 1234 ");

        await lib.ReadBooks();

        Assert.AreEqual("title1", lib.books[0].Title);
    }

    [TestMethod]
    public async Task TestReadUser()
    {
        File.WriteAllText(UsersFile, "1,user1,user1@gmail.com\n2,user2,user2@gmail.com");

        await lib.ReadUsers();

        Assert.AreEqual(2, lib.users.Count);
        Assert.AreEqual("user1", lib.users[0].Name);
        Assert.AreEqual("user1@gmail.com", lib.users[0].Email);
    }

    [TestMethod]
    public async Task TestReadUser_Empty()
    {
        File.WriteAllText(UsersFile, "");

        await lib.ReadUsers();

        Assert.AreEqual(0, lib.users.Count);
    }

    [TestMethod]
    public async Task TestReadUsers_BadData()
    {
        File.WriteAllText(UsersFile, "1 , user1");

        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
        {
            await lib.ReadBooks();
            var _ = lib.users[0];
        });
    }

    [TestMethod]
    public async Task TestReadUser_Spaces()
    {
        File.WriteAllText(UsersFile, "1 , user1 , user1@gmail.com ");

        await lib.ReadUsers();

        Assert.AreEqual("user1", lib.users[0].Name);
    }

    [TestMethod]
    public async Task TestReadBorrowedBooks()
    {
        File.WriteAllText(BorrowedFile, "1,user1,user1@gmail.com\n1,4,title4,author4,456\n1,3,title3,author3,123");
        File.WriteAllText(UsersFile, "1,user1,user1@gmail.com");

        await lib.ReadUsers();
        await lib.ReadBorrowedBooksFromCsv();

        Assert.AreEqual(2, lib.borrowedBooks[lib.users[0]].Count);
        Assert.AreEqual("title4", lib.borrowedBooks[lib.users[0]][0].Title);
        Assert.AreEqual("author4", lib.borrowedBooks[lib.users[0]][0].Author);
    }

    [TestMethod]
    public async Task TestReadBorrowedBooks_Empty()
    {
        File.WriteAllText(BorrowedFile, "");
        File.WriteAllText(UsersFile, "1,user1,user1@gmail.com");

        await lib.ReadUsers();
        await lib.ReadBooks();

        Assert.IsFalse(lib.borrowedBooks.ContainsKey(lib.users[0]));
    }

    [TestMethod]
    public async Task TestReadBorrowedBooks_BadData()
    {
        File.WriteAllText(BorrowedFile, "1,user1,user1@gmail.com\n1,4,title4,author4");
        File.WriteAllText(UsersFile, "1,user1,user1@gmail.com");
        await lib.ReadUsers();

        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
        {
            await lib.ReadBorrowedBooksFromCsv();
            var _ = lib.borrowedBooks[lib.users[0]][0];
        });
    }

    [TestMethod]
    public async Task TestReadBorrowedBooks_Spaces()
    {
        File.WriteAllText(BorrowedFile, "1,user1, user1@gmail.com\n1 ,4, title4, author4 , 1234 \n1, 3, title3, author3, 4567");
        File.WriteAllText(UsersFile, "1,user1,user1@gmail.com");
        await lib.ReadUsers();
        await lib.ReadBorrowedBooksFromCsv();

        Assert.AreEqual(2, lib.borrowedBooks[lib.users[0]].Count);
        Assert.AreEqual("title4", lib.borrowedBooks[lib.users[0]][0].Title);
    }
}
