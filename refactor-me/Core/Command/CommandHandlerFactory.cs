using RefactorMe.Core.External.Repositories;
using RefactorMe.Infrastructure.Repositories;
using System;

namespace RefactorMe.Core.Command
{
    //TODO: Use StructureMap instead
    public class CommandHandlerFactory
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

        public ICommandHandler<C> Create<C>()
        {
            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(C));
            if (handlerType.GenericTypeArguments[0] == typeof(CreateProductCommand))
            {
                return (ICommandHandler<C>)new CreateProductCommandHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(UpdateProductCommand))
            {
                return (ICommandHandler<C>)new UpdateProductCommandHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(DeleteProductCommand))
            {
                return (ICommandHandler<C>)new DeleteProductCommandHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(CreateProductOptionCommand))
            {
                return (ICommandHandler<C>)new CreateProductOptionCommandHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(UpdateProductOptionCommand))
            {
                return (ICommandHandler<C>)new UpdateProductOptionCommandHandler(ProductRepository);
            }
            else if (handlerType.GenericTypeArguments[0] == typeof(DeleteProductOptionCommand))
            {
                return (ICommandHandler<C>)new DeleteProductOptionCommandHandler(ProductRepository);
            }

            return null;
        }
    }
}