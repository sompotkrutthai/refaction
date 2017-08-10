using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.Command
{
    public class DeleteProductOptionCommandHandler : ICommandHandler<DeleteProductOptionCommand>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductOptionCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ICommandResult Execute(DeleteProductOptionCommand command)
        {
            //TODO: Use fluent validation to validate command model
            IList<string> errorMessages = Validate(command);
            if(errorMessages != null && errorMessages.Count() > 0)
            {
                return new CommandResult(errorMessages);
            }

            Product product = productRepository.GetById(command.ProductId, includeOptions: true);
            ProductOption productOption = product.Options.Single(x => x.Id == command.Id);
            product.RemoveOption(productOption);

            productRepository.Update(product);

            return new CommandResult();
        }

        private IList<string> Validate(DeleteProductOptionCommand command)
        {
            IList<string> errorMessages = new List<string>();
            Product product = productRepository.GetById(command.ProductId, includeOptions: true);
            if (product == null)
            {
                errorMessages.Add("Product not found");
                return errorMessages;
            }
            if (!product.Options.Any(x => x.Id == command.Id))
            {
                errorMessages.Add("Product option not found");
                return errorMessages;
            }

            return errorMessages;
        }
    }
}