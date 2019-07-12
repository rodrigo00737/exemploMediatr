# Exemplo de utilização da biblioteca Mediatr com ASP.NET Core para implementação Design Pattern Mediator

O Mediator é um padrão de projeto Comportamental criado pelo GoF, que nos ajuda a garantir um baixo acoplamento entre os objetos de nossa aplicação. Ele permite que um objeto se comunique com outros sem saber suas estruturas. Para isso devemos definir um ponto central que irá encapsular como os objetos irão se comunicar uns com os outros.

> “Uma biblioteca tentando resolver um problema simples – desacoplando o envio interno de mensagens. Cross-platform, suportando o .NET 4.5 e o netstandard1.1.” Jimmy Bogard – Criador da biblioteca Mediatr, e da biblioteca AutoMapper.

### Funcionalidade
Basicamente temos dois componentes principais chamados de **Request** e **Handler**, que implementamos através das interfaces **IRequest** e **IRequestHandler<TRequest>** respectivamente.

**Request** → mensagem que será processada.

**Handler** → responsável por processar determinada(s) mensagen(s).

### Configuração Inicial

- Criar uma solution **TesteMediator**
- Criar um projeto **TesteMediatr.Api** do tipo Asp.Net Core Web Application
- Criar um projeto **TesteMediatr.Domain** do tipo Class Library (.Net Core)

Para utilizarmos o **Mediatr** precisamos baixar via **NUGET** os seguintes pacotes:

```dotnetcli
Install-Package MediatR
```
Instala a biblioteca principal, o CORE do Mediatr.

```dotnetcli
Install-Package MediatR.Extensions.Microsoft.DependencyInjection
```
Instala o pacote para utilização com o ASP.NET Core que inclui um método de extensão ```IServiceCollection.AddMediatR```, permitindo que você registre todos os Handlers e Pre / PostProcessors.

Dentro do projeto **TesteMediatr.Domain** criar a pasta **Models**.

Dentro da pasta Models, criar o arquivo **Product.cs**.

```csharp
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
```

Na raiz do projeto criar a pasta **Handlers**

Dentro da pasta criar o arquivo **ProductHandler.cs**.

```csharp
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
            //Faz as validações dos campos da mensagem que no caso é o Produto
            if (string.IsNullOrEmpty(message.Name) && message.Price == 0)
                return Task.FromResult("Preencher todos os campos.");

            //Depois de feita as validações do Produto, salva no repositório.
            //productRepository.Save(message);
            return Task.FromResult("Produto inserido com sucesso.");
        }
    }
}
```


Dentro do Projeto **TesteMediatr.Api**, criar a controller ProductController, conforme código abaixo:

```csharp
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
```

Dentro do arquivo **Startup.cs** adicionar dentro do método ```ConfigureServices(IServiceCollection services)``` as seguintes linhas:

```csharp
//Busca o Assembly onde estão os Handlers
var assembly = AppDomain.CurrentDomain.Load("TesteMediatr.Domain");

//Adiciona o assembly para ser monitorado pelo gestor de dependencia da implementação do Mediatr.
services.AddMediatR(assembly);
```


### Referências
http://mcamara-site.azurewebsites.net/2017/06/10/utilizando-biblioteca-mediatr-com-asp-net-core/
https://www.wellingtonjhn.com/posts/mediatr-com-asp.net-core/
