namespace LibraryApi.Models
{
    public class Book
    {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string AuthorName { get; set; } = string.Empty;
            public string Genre { get; set; } = string.Empty;
            public bool IsBorrowed { get; set; } = false;
            public string? BorrowerName { get; set; }
       
    }
}
