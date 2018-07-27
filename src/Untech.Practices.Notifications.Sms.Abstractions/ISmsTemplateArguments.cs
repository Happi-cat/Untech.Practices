namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	///     Used as a marker for SMS template arguments.
	/// </summary>
	public interface ISmsTemplateArguments
	{
		/// <summary>
		///     Gets the key that can be used for template selection.
		/// </summary>
		string TemplateKey { get; }
	}
}