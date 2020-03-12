using System.Collections.Generic;
using System.Runtime.Serialization;
using NodaTime;
using Untech.Practices;

namespace MyBudgetPlan.Domain
{
	[DataContract]
	public class YearMonth : ValueObject<YearMonth>
	{
		private YearMonth()
		{
		}

		public YearMonth(int year, int month)
		{
			Year = year;
			Month = month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		public static implicit operator LocalDate(YearMonth date)
		{
			return new LocalDate(date.Year, date.Month, 1);
		}

		public static implicit operator YearMonth(LocalDate date)
		{
			(int year, int month, _) = date;
			return new YearMonth(year, month);
		}

		protected override IEnumerable<object> GetEquatableProperties()
		{
			yield return Year;
			yield return Month;
		}

		public override string ToString()
		{
			return Year + "." + Month;
		}
	}
}