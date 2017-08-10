using System.Collections.Generic;

namespace RefactorMe.Core.Command
{
    public class CommandResult: ICommandResult
    {
        public bool IsCompleted { get; set; }
        public IList<string> ErrorMessages { get; set; }

        public CommandResult()
        {
            IsCompleted = true;
        }
        public CommandResult(IList<string> errorMessages)
        {
            IsCompleted = false;
            ErrorMessages = errorMessages;
        }
    }

    public class CommandModelResult<T> : CommandResult
    {
        public T Model { get; set; }

        public CommandModelResult(T model)
        {
            IsCompleted = true;
            Model = model;
        }
        public CommandModelResult(IList<string> errorMessages)
            : base(errorMessages)
        {
            IsCompleted = false;
            ErrorMessages = errorMessages;
        }
    }
}