using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using UnitOfWorkPractice.Models;
using UnitOfWorkPractice.Repos.Unit;

namespace UnitOfWorkPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTextController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
       
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<UserTextController> _logger;

        public UserTextController(ILogger<UserTextController> logger,
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        public async Task<IActionResult>CreateProduc([FromBody]Product model)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (model == null)
                {
                    return BadRequest("Product model cannot be null");
                }
                await _unitOfWork.ProductRepository.AddAsync(model);
                await _unitOfWork.CommitTransactionAsync();
                
                transactionScope.Complete(); // Commit the transaction scope
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
