using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.Command
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ICommandResult Execute(CreateProductCommand command)
        {
            //TODO: Use fluent validation to validate command model
            IList<string> errorMessages = Validate(command);
            if(errorMessages != null && errorMessages.Count() > 0)
            {
                return new CommandModelResult<Product>(errorMessages);
            }
            
            //TODO: Use AutoMapper to handle model mapping
            Product product = new Product();
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.DeliveryPrice = command.DeliveryPrice;
            
            productRepository.Create(product);

            return new CommandModelResult<Product>(product);
        }

        private IList<string> Validate(CreateProductCommand command)
        {
            IList<string> errorMessages = new List<string>();
            if (string.IsNullOrEmpty(command.Name))
            {
                errorMessages.Add("Name is required");
            }
            else if (command.Name.Length > 50)
            {
                errorMessages.Add("Name length more than 50 characters");
            }

            if (string.IsNullOrEmpty(command.Description) && command.Description.Length > 255)
            {
                errorMessages.Add("Description length more than 255 characters");
            }

            if (command.Price < -1)
            {
                errorMessages.Add("Price is invalid");
            }

            if (command.DeliveryPrice < -1)
            {
                errorMessages.Add("Delivery Price is invalid");
            }

            return errorMessages;
        }
    }
}