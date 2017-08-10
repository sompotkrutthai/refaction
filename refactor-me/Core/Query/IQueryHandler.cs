namespace RefactorMe.Core.Query
{
    public interface IQueryHandler<Q, T>
    {
        T Handle(Q query);
    }
}
