namespace Untech.Practices.Security
{
	public readonly struct PasswordCheckResult
	{
		public PasswordCheckResult(bool verified, bool needsUpgrade)
		{
			Verified = verified;
			NeedsUpgrade = needsUpgrade;
		}
		public bool Verified { get; }
		public bool NeedsUpgrade { get; }

		public static implicit operator bool(PasswordCheckResult result)
		{
			return result.Verified;
		}
	}
}