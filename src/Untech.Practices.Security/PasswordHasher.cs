using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Untech.Practices.Security
{
	public sealed class PasswordHasher : IPasswordHasher
	{
		private const int SaltSizeInBytes = 16; // 128 bit
		private const int KeySizeInBytes = 32; // 256 bit

		public PasswordHasher(IOptions<HashingOptions> options)
		{
			Options = options.Value;
		}

		private HashingOptions Options { get; }

		public string Hash(string password)
		{
			using var algorithm = new Rfc2898DeriveBytes(password, SaltSizeInBytes, Options.Iterations, HashAlgorithmName.SHA256);

			var key = Convert.ToBase64String(algorithm.GetBytes(KeySizeInBytes));
			var salt = Convert.ToBase64String(algorithm.Salt);

			return $"{Options.Iterations}.{salt}.{key}";
		}

		public PasswordCheckResult Check(string hash, string password)
		{
			var parts = hash.Split('.', 3);

			if (parts.Length != 3)
			{
				throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");
			}

			var iterations = Convert.ToInt32(parts[0]);
			var salt = Convert.FromBase64String(parts[1]);
			var key = Convert.FromBase64String(parts[2]);

			using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
			var keyToCheck = algorithm.GetBytes(KeySizeInBytes);

			return new PasswordCheckResult(keyToCheck.SequenceEqual(key), iterations != Options.Iterations);
		}
	}
}
