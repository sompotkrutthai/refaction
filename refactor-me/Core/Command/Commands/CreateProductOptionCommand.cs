using System;

namespace RefactorMe.Core.Command
{
    public class CreateProductOptionCommand
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}