﻿using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.Command
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ICommandResult Execute(UpdateProductCommand command)
        {
            //TODO: Use fluent validation to validate command model
            IList<string> errorMessages = Validate(command);
            if(errorMessages != null && errorMessages.Count() > 0)
            {
                return new CommandModelResult<Product>(errorMessages);
            }

            //TODO: Use AutoMapper to handle model mapping
            Product product = productRepository.GetById(command.Id);
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.DeliveryPrice = command.DeliveryPrice;
            
            productRepository.Update(product);

            return new CommandModelResult<Product>(product);
        }

        private IList<string> Validate(UpdateProductCommand command)
        {
            IList<string> errorMessages = new List<string>();
            Product product = productRepository.GetById(command.Id);
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