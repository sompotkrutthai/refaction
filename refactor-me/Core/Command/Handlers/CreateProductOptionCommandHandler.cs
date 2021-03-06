﻿using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.Command
{
    public class CreateProductOptionCommandHandler : ICommandHandler<CreateProductOptionCommand>
    {
        private readonly IProductRepository productRepository;

        public CreateProductOptionCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ICommandResult Execute(CreateProductOptionCommand command)
        {
            //TODO: Use fluent validation to validate command model
            IList<string> errorMessages = Validate(command);
            if (errorMessages != null && errorMessages.Count() > 0)
            {
                return new CommandModelResult<Product>(errorMessages);
            }

            Product product = productRepository.GetById(command.ProductId, includeOptions: true);
            //TODO: Use AutoMapper to handle model mapping
            ProductOption productOption = new ProductOption(product)
            {
                Name = command.Name,
                Description = command.Description
            };
            product.AddOption(productOption);

            productRepository.Update(product);

            return new CommandModelResult<ProductOption>(productOption);
        }

        private IList<string> Validate(CreateProductOptionCommand command)
        {
            IList<string> errorMessages = new List<string>();
            Product product = productRepository.GetById(command.ProductId);
            if (product == null)
            {
                errorMessages.Add("Product not found");
                return errorMessages;
            }

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

            return errorMessages;
        }
    }
}