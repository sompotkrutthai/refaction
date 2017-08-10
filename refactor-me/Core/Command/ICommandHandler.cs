namespace RefactorMe.Core.Command
{
    public interface ICommandHandler<C>
    {
        ICommandResult Execute(C command);
    }
}
