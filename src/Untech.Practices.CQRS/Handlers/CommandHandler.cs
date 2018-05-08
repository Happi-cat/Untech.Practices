namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents base command handler without a response.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	public abstract class CommandHandler<TIn> : ICommandHandler<TIn, Nothing>
		where TIn : ICommand<Nothing>
	{
		public Nothing Handle(TIn request)
		{
			HandleCore(request);

			return Nothing.AtAll;
		}

		protected abstract void HandleCore(TIn command);
	}
}