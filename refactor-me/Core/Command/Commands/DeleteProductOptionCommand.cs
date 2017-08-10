using System;

namespace RefactorMe.Core.Command
{
    public class DeleteProductOptionCommand
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
    }
}