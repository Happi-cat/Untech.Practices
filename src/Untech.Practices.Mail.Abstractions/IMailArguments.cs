namespace Untech.Practices.Mail
{
	/// <summary>
	/// Used as a marker for mail arguments.
	/// </summary>
	public interface IMailArguments
	{
		/// <summary>
		/// Gets mail type that can be used for template selection or other purpose.
		/// </summary>
		string Type { get; }
	}
}