﻿namespace Untech.Practices.Mail
{
	/// <summary>
	/// Defines methods for sending mails into mail queue.
	/// </summary>
	public interface IMailQueue
	{
		/// <summary>
		/// Adds mail for sending into mail queue.
		/// </summary>
		/// <param name="mail">The mail to sent.</param>
		void Enqueue(Mail mail);
	}
}