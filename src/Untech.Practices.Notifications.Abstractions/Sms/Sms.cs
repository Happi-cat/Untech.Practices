using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	///     Represents SMS info that can be used for rendering and sending.
	/// </summary>
	[DataContract]
	public class Sms : INotification
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
		/// <param name="templateKey"></param>
		/// <param name="payload"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public Sms(string toNumber, string templateKey, object payload = null)
		{
			if (string.IsNullOrEmpty(toNumber)) throw new ArgumentNullException(nameof(toNumber));

			ToNumber = toNumber;
			TemplateKey = templateKey ?? throw new ArgumentNullException(nameof(templateKey));
			Payload = payload;
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
		public string TemplateKey { get; private set; }

		/// <summary>
		///     Gets additional arguments.
		/// </summary>
		[DataMember]
		public object Payload { get; private set; }
	}
}