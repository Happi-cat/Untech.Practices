namespace Untech.Practices.Mail
{
	public class SimpleMailQueueOptions
	{
		public SimpleMailQueueOptions(string host, int port, MailboxAddress from)
		{
			Host = host;
			Port = port;
			From = from;
		}

		public MailboxAddress From { get; private set; }
		public string Host { get; private set; }
		public int Port { get; private set; }
		public bool UseSsl { get; set; }
	}
}