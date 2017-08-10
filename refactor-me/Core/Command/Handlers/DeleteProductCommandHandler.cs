using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.Command
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ICommandResult Execute(DeleteProductCommand command)
        {
            //TODO: Use fluent validation to validate command model
            IList<string> errorMessages = Validate(command);
            if(errorMessages != null && errorMessages.Count() > 0)
            {
                return new CommandResult(errorMessages);
            }
            
            productRepository.Delete(command.Id);

            return new CommandResult();
        }

        private IList<string> Validate(DeleteProductCommand command)
        {
            IList<string> errorMessages = new List<string>();
            Product product = productRepository.GetById(command.Id);
            if (product == null)
            {
                errorMessages.Add("Product not found");
                return errorMessages;
            }

            return errorMessages;
        }
    }
}