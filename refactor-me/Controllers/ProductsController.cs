using System;
using System.Net;
using System.Web.Http;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Infrastructure.Repositories;
using System.Linq;
using RefactorMe.Core.DomainModels;

namespace RefactorMe.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
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

        [Route]
        [HttpGet]
        public object GetAll()
        {
            return new { Items = ProductRepository.GetAll() };
        }

        [Route]
        [HttpGet]
        public object SearchByName(string name)
        {
            return new { Items = ProductRepository.GetByName(name) };
        }

        [Route("{id}")]
        [HttpGet]
        public object GetProduct(Guid id)
        {
            Product product = ProductRepository.GetById(id);
            if (product == null)
                return NotFound();

            return product;
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Core.DomainModels.Product product)
        {
            ProductRepository.Create(product);

            return Created("", product);
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            Product existingProduct = ProductRepository.GetById(id);
            if(existingProduct == null)
            {
                return BadRequest();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.DeliveryPrice = product.DeliveryPrice;

            ProductRepository.Update(existingProduct);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            Product existingProduct = ProductRepository.GetById(id);
            if (existingProduct == null)
            {
                return BadRequest();
            }

            ProductRepository.Delete(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public object GetOptions(Guid productId)
        {
            Product product = ProductRepository.GetById(productId, includeOptions: true);
            if (product == null)
                return NotFound();

            return product.Options;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public object GetOption(Guid productId, Guid id)
        {
            Product product = ProductRepository.GetById(productId, includeOptions: true);
            if (product == null || (product != null && !product.Options.Any(x => x.Id == id)))
                return NotFound();

            return product.Options.Single(x => x.Id == id);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            Product existingProduct = ProductRepository.GetById(productId, includeOptions: true);
            if (existingProduct == null)
            {
                return BadRequest();
            }

            ProductOption newOption = new ProductOption(existingProduct)
            {
                Name = option.Name,
                Description = option.Description
            };
            existingProduct.AddOption(newOption);

            ProductRepository.Update(existingProduct);

            return Created("", option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            Product existingProduct = ProductRepository.GetById(productId, includeOptions: true);
            if (existingProduct == null || (existingProduct != null && !existingProduct.Options.Any(x => x.Id == id)))
            {
                return BadRequest();
            }
            ProductOption existingProductOption = existingProduct.Options.Single(x => x.Id == id);
            existingProductOption.Name = option.Name;
            existingProductOption.Description = option.Description;

            ProductRepository.Update(existingProduct);

            return Ok();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            Product existingProduct = ProductRepository.GetById(productId, includeOptions: true);
            if (existingProduct == null || (existingProduct != null && !existingProduct.Options.Any(x => x.Id == id)))
            {
                return BadRequest();
            }

            ProductOption existingProductOption = existingProduct.Options.Single(x => x.Id == id);
            existingProduct.RemoveOption(existingProductOption);

            ProductRepository.Update(existingProduct);

            return Ok();
        }
    }
}
