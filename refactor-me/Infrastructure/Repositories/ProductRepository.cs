using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace RefactorMe.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            return GetByQuery($"select * from product");
        }

        public IEnumerable<Product> GetByName(string name)
        {
            return GetByQuery($"select * from product where lower(name) like '%{name.ToLower()}%'");
        }

        public Product GetById(Guid id)
        {
            return GetByQuery($"select * from product where id = '{id}'").SingleOrDefault();
        }

        public void Create(Product product)
        {
            string statement = $"insert into product(id, name, description, price, deliveryprice) " +
                               $"values('{product.Id}', '{product.Name}', '{product.Description}', { product.Price}, { product.DeliveryPrice})";

            ExecuteCommand(statement);
        }

        public void Update(Product product)
        {
            string statement = $"update product " +
                               $"set name = '{product.Name}', " +
                               $"description = '{product.Description}', " +
                               $"price = {product.Price}, " +
                               $"deliveryprice = {product.DeliveryPrice} " +
                               $"where id = '{product.Id}'";

            ExecuteCommand(statement);
        }

        public void Delete(Guid id)
        {
            string statement = $"delete from product where id = '{id}'";

            ExecuteCommand(statement);
        }

        private IEnumerable<Product> GetByQuery(string query)
        {
            IList<Product> items = new List<Product>();
            using (var conn = Helpers.NewConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Product product = new Product()
                        {
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        };

                        PropertyInfo idProperty = typeof(Product).GetProperty("Id");
                        idProperty.SetValue(product, Guid.Parse(rdr["id"].ToString()));

                        items.Add(product);
                    }
                }
            }

            return items;
        }

        private void ExecuteCommand(string command)
        {
            using (var conn = Helpers.NewConnection())
            {
                using (var cmd = new SqlCommand(command, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}