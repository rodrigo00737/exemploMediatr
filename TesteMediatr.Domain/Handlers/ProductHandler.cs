using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TesteMediatr.Domain.Models;

namespace TesteMediatr.Domain.Handlers
{
    public class ProductHandler : IRequestHandler<Product, string>
    {
        public Task<string> Handle(Product message, CancellationToken cancellationToken)
        {
            //Faço as validações dos campos da mensagem que no caso é o Produto
            if (string.IsNullOrEmpty(message.Name) && message.Price == 0)
                return Task.FromResult("Preencher todos os campos.");

            //Depois de feito as validações do Produto, agora podemos salva-lo em nosso repositório.
            //productRepository.Save(message);
            return Task.FromResult("Produto inserido com sucesso.");
        }
    }
}
