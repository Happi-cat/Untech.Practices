using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MyParcels.Domain;
using MyParcels.Infrastructure.Data;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.UserContext;

namespace MyParcels.Infrastructure
{
	public class ParcelService : IQueryAsyncHandler<ParcelsQuery, IEnumerable<Parcel>>,
		ICommandAsyncHandler<CreateOrUpdateParcel, Parcel>,
		ICommandAsyncHandler<DeleteParcel, Nothing>
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
				var daos = await GetMyParcels(context).ToListAsync(cancellationToken);

				return daos
					.Select(n => new Parcel(n.Key, n.Description))
					.ToList();
			}
		}

		public async Task<Parcel> HandleAsync(CreateOrUpdateParcel request, CancellationToken cancellationToken)
		{
			var dao = new ParcelDao(request.TrackingNumber, _userContext.UserKey)
			{
				Description = request.Description
			};

			using (var context = GetContext())
			{
				await context.InsertOrReplaceAsync(dao, token: cancellationToken);

				return new Parcel(dao.Key, dao.Description);
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

		private IQueryable<ParcelDao> GetMyParcels(IDataContext context)
		{
			return context.GetTable<ParcelDao>()
				.Where(p => p.UserKey == _userContext.UserKey);
		}
	}
}