using RefactorMe.Core.DomainModels;
using System;
using System.Collections.Generic;

namespace RefactorMe.Core.External.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByName(string name);
        Product GetById(Guid id);
        void Create(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}
