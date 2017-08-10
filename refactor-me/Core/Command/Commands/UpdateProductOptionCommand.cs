using System;

namespace RefactorMe.Core.Command
{
    public class UpdateProductOptionCommand
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}