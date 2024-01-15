namespace CRUDBooks.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();
            if (handler == null)
            {
                throw new Exception(typeof(TQuery).ToString());
            }

            return handler.Execute(query);
        }
    }

}
