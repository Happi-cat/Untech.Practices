using System;
using System.Buffers;
using System.Security.Cryptography;

namespace Untech.Practices.Security
{
	public class SecretsGenerator : ISecretsGenerator
	{
		public string GenerateSecret(int bytesCount, bool urlSafe)
		{
			using RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider();

			byte[] buffer = ArrayPool<byte>.Shared.Rent(bytesCount);
			cryptoRandomDataGenerator.GetBytes(buffer);
			var result = Convert.ToBase64String(buffer);
			ArrayPool<byte>.Shared.Return(buffer);

			return urlSafe ? result.ToUrlSafeString() : result;
		}
	}
}