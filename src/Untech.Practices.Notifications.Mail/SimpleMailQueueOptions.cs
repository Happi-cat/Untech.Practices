namespace Untech.Practices.Notifications.Mail
{
	public class SimpleMailQueueOptions
	{
		public SimpleMailQueueOptions(string host, int port, MailboxAddress from)
		{
			Host = host;
			Port = port;
			From = from;
		}

		public MailboxAddress From { get; }
		public string Host { get; }
		public int Port { get; }
		public bool UseSsl { get; set; }
	}
}