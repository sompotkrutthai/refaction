using System;

namespace RefactorMe.Core.DomainModels
{
    public class ProductOption
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductOption(Product product)
        {
            if (product == null) { throw new ArgumentNullException("Product is null"); }

            Id = Guid.NewGuid();
            ProductId = product.Id;
        }
    }
}