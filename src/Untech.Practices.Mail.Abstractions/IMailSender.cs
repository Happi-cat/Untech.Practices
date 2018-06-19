using System.Threading.Tasks;

namespace Untech.Practices.Mail
{
	public interface IMailSender
	{
		void Send<T>(Mail<T> mail);
		Task SendAsync<T>(Mail<T> mail);
	}
}