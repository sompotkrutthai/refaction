using RefactorMe.Core.Command;
using RefactorMe.Core.DomainModels;
using RefactorMe.Core.Query;
using RefactorMe.Core.Query.Queries;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace RefactorMe.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private QueryHandlerFactory queryHandlerFactory;
        private QueryHandlerFactory QueryHandlerFactory
        {
            get
            {
                if (queryHandlerFactory == null)
                {
                    queryHandlerFactory = new QueryHandlerFactory();
                }
                return queryHandlerFactory;
            }
        }

        private CommandHandlerFactory commandHandlerFactory;
        private CommandHandlerFactory CommandHandlerFactory
        {
            get
            {
                if (commandHandlerFactory == null)
                {
                    commandHandlerFactory = new CommandHandlerFactory();
                }
                return commandHandlerFactory;
            }
        }

        [Route]
        [HttpGet]
        public object GetAll()
        {
            IQueryHandler<GetProductsQuery, IEnumerable<Product>> queryHandler = QueryHandlerFactory.Create<GetProductsQuery, IEnumerable<Product>>();
            return new { Items = queryHandler.Handle(new GetProductsQuery()) };
        }

        [Route]
        [HttpGet]
        public object SearchByName(string name)
        {
            IQueryHandler<GetProductsQuery, IEnumerable<Product>> queryHandler = QueryHandlerFactory.Create<GetProductsQuery, IEnumerable<Product>>();
            return new { Items = queryHandler.Handle(new GetProductsQuery() { Name = name }) };
        }

        [Route("{id}")]
        [HttpGet]
        public object GetProduct(Guid id)
        {
            IQueryHandler<GetProductQuery, Product> queryHandler = QueryHandlerFactory.Create<GetProductQuery, Product>();
            Product product = queryHandler.Handle(new GetProductQuery { Id = id });
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(CreateProductCommand command)
        {
            ICommandHandler<CreateProductCommand> commandHandler = CommandHandlerFactory.Create<CreateProductCommand>();
            ICommandResult commandResult = commandHandler.Execute(command);
            if (commandResult.IsCompleted)
            {
                return Created("", (commandResult as CommandModelResult<Product>).Model);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, UpdateProductCommand command)
        {
            ICommandHandler<UpdateProductCommand> commandHandler = CommandHandlerFactory.Create<UpdateProductCommand>();
            command.Id = id;
            ICommandResult commandResult = commandHandler.Execute(command);
            if (commandResult.IsCompleted)
            {
                return Ok();
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            ICommandHandler<DeleteProductCommand> commandHandler = CommandHandlerFactory.Create<DeleteProductCommand>();
            ICommandResult commandResult = commandHandler.Execute(new DeleteProductCommand { Id = id });
            if (commandResult.IsCompleted)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        public object GetOptions(Guid productId)
        {
            IQueryHandler<GetProductOptionsQuery, IEnumerable<ProductOption>> queryHandler = QueryHandlerFactory.Create<GetProductOptionsQuery, IEnumerable<ProductOption>>();
            return new { Items = queryHandler.Handle(new GetProductOptionsQuery { ProductId = productId }) };
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public object GetOption(Guid productId, Guid id)
        {
            IQueryHandler<GetProductOptionQuery, ProductOption> queryHandler = QueryHandlerFactory.Create<GetProductOptionQuery, ProductOption>();
            ProductOption productOption = queryHandler.Handle(new GetProductOptionQuery { ProductId = productId, Id = id });
            if (productOption == null)
            {
                return NotFound();
            }

            return productOption;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, CreateProductOptionCommand command)
        {
            ICommandHandler<CreateProductOptionCommand> commandHandler = CommandHandlerFactory.Create<CreateProductOptionCommand>();
            command.ProductId = productId;
            ICommandResult commandResult = commandHandler.Execute(command);
            if (commandResult.IsCompleted)
            {
                return Created("", (commandResult as CommandModelResult<ProductOption>).Model);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, UpdateProductOptionCommand command)
        {
            ICommandHandler<UpdateProductOptionCommand> commandHandler = CommandHandlerFactory.Create<UpdateProductOptionCommand>();
            command.ProductId = productId;
            command.Id = id;
            ICommandResult commandResult = commandHandler.Execute(command);
            if (commandResult.IsCompleted)
            {
                return Ok();
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            ICommandHandler<DeleteProductOptionCommand> commandHandler = CommandHandlerFactory.Create<DeleteProductOptionCommand>();
            ICommandResult commandResult = commandHandler.Execute(new DeleteProductOptionCommand { ProductId = productId, Id = id });
            if (commandResult.IsCompleted)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, commandResult.ErrorMessages);
            }
        }
    }
}
