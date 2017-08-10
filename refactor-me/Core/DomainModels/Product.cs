using Newtonsoft.Json;
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

        private IList<ProductOption> options { get; set; }

        [JsonIgnore]
        public IEnumerable<ProductOption> Options { get { return (options != null) ? options : new List<ProductOption>(); } }

        public Product()
        {
            Id = Guid.NewGuid();
            options = new List<ProductOption>();
        }

        public void AddOption(ProductOption option)
        {
            if (!options.Any(x => x.Id == option.Id))
            {
                options.Add(option);
            }
        }

        public void RemoveOption(ProductOption option)
        {
            if (options != null && options.Any(x => x.Id == option.Id))
            {
                ProductOption exOption = options.Single(x => x.Id == option.Id);
                exOption.MarkDeleted();
            }
        }
    }
}