using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query.Queries;
using RefactorMe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;

namespace RefactorMe.Core.Query
{
    //TODO: Use StructureMap instead
    public class QueryHandlerFactory
    {
        private IProductRepository productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository();
                }
                return productRepository;
            }
        }
        
        public IQueryHandler<Q, M> Create<Q, M>()
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(Q), typeof(M));
            if (handlerType.GenericTypeArguments[0] == typeof(GetProductsQuery) && handlerType.GenericTypeArguments[1] == typeof(IEnumerable<Product>))
            {
                return (IQueryHandler<Q, M>)new GetProductsQueryHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(GetProductQuery) && handlerType.GenericTypeArguments[1] == typeof(Product))
            {
                return (IQueryHandler<Q, M>)new GetProductQueryHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(GetProductOptionsQuery) && handlerType.GenericTypeArguments[1] == typeof(IEnumerable<ProductOption>))
            {
                return (IQueryHandler<Q, M>)new GetProductOptionsQueryHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(GetProductOptionQuery) && handlerType.GenericTypeArguments[1] == typeof(ProductOption))
            {
                return (IQueryHandler<Q, M>)new GetProductOptionQueryHandler(ProductRepository);
            }

            return null;
        }
    }
}