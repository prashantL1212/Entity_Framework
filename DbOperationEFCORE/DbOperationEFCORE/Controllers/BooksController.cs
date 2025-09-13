using DbOperationEFCORE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbOperationEFCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await appDbContext.Books.Include(x=>x.Author).ToListAsync();

            return Ok(books);
        }

        [HttpGet("FromSQL")]
        public async Task<IActionResult> GetAllBooksSQL()
        {
            //var books = await appDbContext.Books.FromSql($"select * from Books").ToListAsync();

            //var books = await appDbContext.Books.FromSql($"GetALLBook").ToListAsync();
            var param = new SqlParameter("@Id", 10);
            var books = await appDbContext.Books.FromSql($"GetALLBookByID {param}").ToListAsync();
            
            return Ok(books);
        }

        [HttpGet("AllLanguage")]
        public async Task<IActionResult> GetAllLanguage()
        {
            var books = await appDbContext.Languages.Include(x => x.Books).ToListAsync();

            return Ok(books);
        }

        [HttpPost("addBook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book)
        {
            try
            {
                appDbContext.Books.Add(book);
                await appDbContext.SaveChangesAsync();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addMultipleBook")]
        public async Task<IActionResult> AddMultipleBook([FromBody] List<Book> book)
        {
            appDbContext.Books.AddRange(book);
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] Book model)
        {
            var book = appDbContext.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;
            book.NoOfPages = model.NoOfPages;

            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateBookwithSingleHit([FromBody] Book model)
        {

            try
            {
                appDbContext.Books.Update(model);

                await appDbContext.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookwithSingleHit_Bulk()
        {

            try
            {
                await appDbContext.Books.Where(x => x.AuthorId == null)
                    .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.AuthorId, x => 5)
                // .SetProperty(x => x.Description, x => "New Description")
                );

                //await appDbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("{Bookid}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int Bookid)
        {
            //var book = appDbContext.Books.FirstOrDefault(x => x.Id == Bookid);

            //if (book == null)
            //{
            //    return NotFound();
            //}
            //appDbContext.Books.Remove(book);
            var book = new Book() { Id = Bookid };
            appDbContext.Entry(book).State = EntityState.Deleted;
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpDelete("BulkDelete")]
        public async Task<IActionResult> DeleteBulkBook()
        {

            var book = await appDbContext.Books.Where(x => x.Id < 2).ToListAsync();
            if (book == null)
            {
                return NotFound();
            }
            appDbContext.Books.RemoveRange(book);
            await appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("BulkDelete_1HIT")]
        public async Task<IActionResult> DeleteBulkBookSingleHit()
        {

            await appDbContext.Books.Where(x => x.Id < 4).ExecuteDeleteAsync();

            return Ok();
        }
    }
}
