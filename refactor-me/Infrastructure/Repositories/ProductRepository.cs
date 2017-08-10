using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace RefactorMe.Infrastructure.Repositories
{
    //TODO: use Entity Framework instead
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            return GetProductsByQuery($"select * from product");
        }

        public IEnumerable<Product> GetByName(string name)
        {
            return GetProductsByQuery($"select * from product where lower(name) like '%{name.ToLower()}%'");
        }

        public Product GetById(Guid id, bool includeOptions = false)
        {
            Product product = GetProductsByQuery($"select * from product where id = '{id}'").SingleOrDefault();
            if(product != null && includeOptions)
            {
                IEnumerable<ProductOption> productOptions = GetProductOptionsByQuery($"select * from productoption where productid = '{product.Id}'");
                if(productOptions.Count() > 0)
                {
                    productOptions.ToList().ForEach(x => product.AddOption(x));
                }
            }

            return product;
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

            ManageProductOptions(product);
        }

        public void Delete(Guid id)
        {
            string statement = $"delete from product where id = '{id}'";

            ExecuteCommand(statement);
        }

        private IEnumerable<Product> GetProductsByQuery(string query)
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

        private IEnumerable<ProductOption> GetProductOptionsByQuery(string query)
        {
            IList<ProductOption> items = new List<ProductOption>();
            using (var conn = Helpers.NewConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductOption productOption = new ProductOption()
                        {
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
                        };

                        PropertyInfo idProperty = typeof(ProductOption).GetProperty("Id");
                        idProperty.SetValue(productOption, Guid.Parse(rdr["Id"].ToString()));

                        PropertyInfo productIdProperty = typeof(ProductOption).GetProperty("ProductId");
                        productIdProperty.SetValue(productOption, Guid.Parse(rdr["ProductId"].ToString()));

                        items.Add(productOption);
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

        private void ManageProductOptions(Product product)
        {
            if (product.Options != null && product.Options.Any())
            {
                foreach (ProductOption option in product.Options)
                {
                    string statement = null;
                    //delete option, if option has set IsDeleted = true
                    if (option.IsDeleted)
                    {
                        statement = $"delete from productoption where id = '{option.Id}'";
                    }
                    else
                    {
                        IEnumerable<ProductOption> options = GetProductOptionsByQuery($"select * from productoption where id = '{option.Id}'");
                        if (options == null || (options.Count() == 0))
                        {
                            //if it's not exists, insert
                            statement = $"insert into productoption (id, productid, name, description) " +
                                        $"values ('{option.Id}', '{option.ProductId}', '{option.Name}', '{option.Description}')";
                        }
                        else
                        {
                            //if it's exists and name/description are being changed, update it
                            ProductOption existingOption = options.Single();
                            if ((option.Name != null && !option.Name.Equals(existingOption.Name))
                                || option.Description != null && !option.Description.Equals(existingOption.Description))
                            {
                                string optionName = option.Name != null && !option.Name.Equals(existingOption.Name) ? option.Name : existingOption.Name;
                                string optionDescription = option.Description != null && !option.Description.Equals(existingOption.Description) ? option.Description : existingOption.Description;
                                statement = $"update productoption set name = '{optionName}', description = '{optionDescription}' where id = '{option.Id}'";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(statement))
                    {
                        ExecuteCommand(statement);
                    }
                }
            }
        }
    }
}