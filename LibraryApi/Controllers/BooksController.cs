using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }
        // GET : api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET : api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            return book;
        }
        // POST : api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        // PUT : api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest();

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(b => b.Id == id);
        }

        // DELETE : api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //POST : api/Books/Borrow/5
        [HttpPost("Borrow/{id}")]
        public async Task<IActionResult> BorrowBook(int id, [FromBody] string borrowerName)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();
            if (book.IsBorrowed)
                return BadRequest("Book is already borrowed.");
            book.IsBorrowed = true;
            book.BorrowerName = borrowerName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Post : api/Books/Return/5
        [HttpPost("Return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();
            if (!book.IsBorrowed)
                return BadRequest("Book is not borrowed.");
            book.IsBorrowed = false;
            book.BorrowerName = null;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}