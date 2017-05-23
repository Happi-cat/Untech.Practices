using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Untech.Practices.AspNetCore.CQRS.Builder;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;

namespace Untech.Practices.AspNetCore.CQRS.Tests
{
	public static class DummyCQRS
	{
		public class Query : IQuery<int>
		{
			public string ParamFromHeader { get; set; }
			public string ParamFromPath { get; set; }
			public string ParamFromCookie { get; set; }
			public string ParamFromQuery { get; set; }
		}

		public class QueryRequestMapper : RequestMapper<Query>
		{
			public QueryRequestMapper()
			{
				Header("header", n => n.ParamFromHeader);
				Path("name", n => n.ParamFromPath);
				Cookie("cookie", n => n.ParamFromCookie);
				Query("query", n => n.ParamFromQuery);
			}
		}

		public class QueryResponseMapper : ResponseMapper<int>
		{
			public QueryResponseMapper()
			{
				Body();
			}
		}

		public class Command : ICommand<int>
		{
			public string ParamFromHeader { get; set; }
			public string ParamFromPath { get; set; }
			public string ParamFromCookie { get; set; }
			public string ParamFromQuery { get; set; }
		}

		public class CommandRequestMapper : IRequestMapper<Command>
		{
			public Command Map(HttpRequest request)
			{
				return new Command
				{
					ParamFromHeader = request.Headers["header"],
					ParamFromPath = (string)request.HttpContext.GetRouteValue("name"),
					ParamFromCookie = request.Cookies["cookie"],
					ParamFromQuery = request.Query["query"]
				};
			}
		}

		public class CommandResponseMapper : IResponseMapper<int>
		{
			public void Map(int input, HttpResponse response)
			{
				// ??
				response.WriteAsync(input.ToString());
				throw new NotImplementedException();
			}
		}


		public class Command2 : ICommand<int>
		{
		}

		public class Command3 : ICommand<int>
		{
		}

		public class Notification : INotification
		{
		}
	}

	public static class Routes
	{
		public static void Configure(IRouteBuilder routes)
		{
			routes.Route("/root/{name}")
				.MapGet(Query.Use(
					new DummyCQRS.QueryRequestMapper(),
					new DummyCQRS.QueryResponseMapper()))
				.MapVerb("POST", Command.Use(
					new DummyCQRS.CommandRequestMapper(),
					new DummyCQRS.CommandResponseMapper()))
				.MapVerb("DELETE", new Command<DummyCQRS.Command, int>(null, null));

		}
	}
}
