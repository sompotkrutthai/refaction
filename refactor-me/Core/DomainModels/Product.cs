using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorMe.Core.DomainModels
{
    public class Product
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public IEnumerable<ProductOption> Options { get; private set; }

        public Product()
        {
            Id = Guid.NewGuid();
            Options = new List<ProductOption>();
        }

        public void AddOption(ProductOption option)
        {
            if(!Options.Any(x => x.Id == option.Id))
            {
                IList<ProductOption> options = Options == null ? new List<ProductOption>() : Options.ToList();
                options.Add(option);

                Options = options.AsEnumerable();
            }
        }

        public void RemoveOption(ProductOption option)
        {
            if (Options != null && Options.Any(x => x.Id == option.Id))
            {
                IList<ProductOption> options = Options.ToList();
                options.Remove(option);

                Options = options.AsEnumerable();
            }
        }
    }
}