using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	///     Represents SMS info that can be used for rendering and sending.
	/// </summary>
	[DataContract]
	public class Sms
	{
		/// <summary>
		///     For serializers
		/// </summary>
		private Sms()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Sms" /> with a predefined <paramref name="toNumber" /> and arguments.
		/// </summary>
		/// <param name="toNumber">Recipients number</param>
		/// <param name="arguments"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public Sms(string toNumber, ISmsTemplateArguments arguments)
		{
			if (string.IsNullOrEmpty(toNumber)) throw new ArgumentNullException(nameof(toNumber));

			ToNumber = toNumber;
			TemplateArguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
		}

		/// <summary>
		///     Gets recipients phone number.
		/// </summary>
		[DataMember]
		public string ToNumber { get; private set; }

		/// <summary>
		///     Gets the template key.
		/// </summary>
		[DataMember]
		public string TemplateKey => TemplateArguments.TemplateKey;

		/// <summary>
		///     Gets additional arguments.
		/// </summary>
		[DataMember]
		public ISmsTemplateArguments TemplateArguments { get; private set; }
	}
}