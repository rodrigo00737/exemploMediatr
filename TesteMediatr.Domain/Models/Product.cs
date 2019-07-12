using MediatR;

namespace TesteMediatr.Domain.Models
{
    public class Product : IRequest<string>
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
