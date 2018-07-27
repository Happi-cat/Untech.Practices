namespace Untech.Practices.Notifications.Mail
{
	/// <summary>
	///     Used as a marker for mail arguments.
	/// </summary>
	public interface IMailTemplateArguments
	{
		/// <summary>
		///     Gets key that can be used for template selection or other purpose.
		/// </summary>
		string TemplateKey { get; }
	}
}