using MediatR;
using Microsoft.AspNetCore.Mvc;
using TesteMediatr.Domain.Models;

namespace TesteMediatr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public ProductController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok("Retorno");
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product message)
        {
            var response = _mediatr.Send(message);
            return Ok(response);
        }
    }
}