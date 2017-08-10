using System.Collections.Generic;

namespace RefactorMe.Core.Command
{
    public interface ICommandResult
    {
        bool IsCompleted { get; set; }
        IList<string> ErrorMessages { get; set; }
    }
}
