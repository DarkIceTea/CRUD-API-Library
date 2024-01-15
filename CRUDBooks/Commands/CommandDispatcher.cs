namespace CRUDBooks.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null) throw new Exception(typeof(TCommand).ToString());

            handler.Execute(command);
        }
    }

}
