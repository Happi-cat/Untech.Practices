using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MyParcels.Domain;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.UserContext;

namespace MyParcels.Infrastructure
{
	public class ParcelService : IQueryHandler<ParcelsQuery, IEnumerable<Parcel>>,
		ICommandHandler<CreateOrUpdateParcel, Parcel>,
		ICommandHandler<DeleteParcel, Nothing>
	{
		private readonly IUserContext _userContext;
		private readonly Func<IDataContext> _dataContext;

		public ParcelService(IUserContext userContext, Func<IDataContext> dataContext)
		{
			_userContext = userContext;
			_dataContext = dataContext;
		}

		public async Task<IEnumerable<Parcel>> HandleAsync(ParcelsQuery request, CancellationToken cancellationToken)
		{
			using (var context = GetContext())
			{
				var items = await GetMyParcels(context).ToListAsync(cancellationToken);

				return items;
			}
		}

		public async Task<Parcel> HandleAsync(CreateOrUpdateParcel request, CancellationToken cancellationToken)
		{
			var item = new Parcel(request.TrackingNumber, _userContext.UserKey, request.Description);

			using (var context = GetContext())
			{
				await context.InsertOrReplaceAsync(item, token: cancellationToken);

				return item;
			}
		}

		public async Task<Nothing> HandleAsync(DeleteParcel request, CancellationToken cancellationToken)
		{
			using (var context = GetContext())
			{
				await GetMyParcels(context)
						.Where(n => n.Key == request.TrackingNumber)
						.DeleteAsync(cancellationToken);
			}

			return Nothing.AtAll;
		}

		private IDataContext GetContext()
		{
			return _dataContext();
		}

		private IQueryable<Parcel> GetMyParcels(IDataContext context)
		{
			return context.GetTable<Parcel>()
				.Where(p => p.UserKey == _userContext.UserKey);
		}
	}
}