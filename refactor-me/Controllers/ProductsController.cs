using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Infrastructure.Repositories;

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
            Core.DomainModels.Product product = ProductRepository.GetById(id);
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
        public IHttpActionResult Update(Guid id, Core.DomainModels.Product product)
        {
            Core.DomainModels.Product existingProduct = ProductRepository.GetById(id);
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
            Core.DomainModels.Product existingProduct = ProductRepository.GetById(id);
            if (existingProduct == null)
            {
                return BadRequest();
            }

            ProductRepository.Delete(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}
