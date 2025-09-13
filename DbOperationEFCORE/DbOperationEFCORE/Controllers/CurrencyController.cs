using DbOperationEFCORE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationEFCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        //to fetch all data from Currency table
        [HttpGet("GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
           //var result=appDbContext.Currencies.ToList();
           //var result =from currency in appDbContext.Currencies select currency;

            var result = await appDbContext.Currencies.ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyById([FromRoute] int id)
        {
            
            var result = await appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByName([FromRoute] string name)
        {
           
            //var result = await appDbContext.Currencies.Where(x => x.Title == name).FirstOrDefaultAsync();//if data not found it will not throw exception and it will return null but multiple records found it will not throw exception it will return first record
           
            //var result = await appDbContext.Currencies.Where(x => x.Title == name).FirstAsync(); //if data not found it will throw exception
          
            //var result =await appDbContext.Currencies.Where(x => x.Title == name).SingleOrDefaultAsync(); //if data not found it will not throw exception and it will return null but if multiple records found it will throw exception
            
            //var result = await appDbContext.Currencies.Where(x => x.Title == name).SingleAsync(); //if data not found it will throw exception but if multiple records found it will throw exception
            
            var result = await appDbContext.Currencies.FirstOrDefaultAsync(x => x.Title == name); //if data not found it will not throw exception and it will return null but multiple records found it will not throw exception it will return first record and it is more optimized if data found at first position it will not check remaining records and it will return first record
            
            return Ok(result);
        }

        [HttpGet("{name}/{desc}")]
        public async Task<IActionResult> GetCurrencyByNameandByDescription([FromRoute] string name, [FromRoute] string desc)
        {


            //var result = await appDbContext.Currencies.FirstOrDefaultAsync(x => x.Title == name && x.Description==desc); //if data not found it will not throw exception and it will return null but multiple records found it will not throw exception it will return first record and it is more optimized if data found at first position it will not check remaining records and it will return first record
            var result = await appDbContext.Currencies.Where(x=>x.Title==name && x.Description==desc).ToListAsync(); //if data not found it will not throw exception and it will return empty list but multiple records found it will not throw exception it will return all records matched
            return Ok(result);
        }

        [HttpGet("linq/{name}")]
        public async Task<IActionResult> GetCurrencyByNameandByDescriptionLINQ([FromRoute] string name, [FromQuery] string? desc)
        {
            var result = await appDbContext.Currencies.FirstOrDefaultAsync(x =>
            x.Title == name && (string.IsNullOrEmpty(desc) || x.Description == desc));
            return Ok(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetCurrencyByMultipleID([FromBody] List<int> ids)
        {
           // var ids = new List<int> { 1, 2, 3 };

            var result = await appDbContext.Currencies.Where(x => ids.Contains(x.Id)).ToListAsync();
            return Ok(result);
        }

        [HttpPost("SpecificColumns")]
        public async Task<IActionResult> GetSpecificColumnsCurrency([FromBody] List<int> ids)
        {
            // var ids = new List<int> { 1, 2, 3 };

            var result = await appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x=>new Currency()
                {
                    Id=x.Id,
                    Title=x.Title
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
