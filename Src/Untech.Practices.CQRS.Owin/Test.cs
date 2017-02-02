using Owin;
using Untech.Practices.CQRS.Owin.Routing;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Owin
{
	public class Test
	{
		public class Query : IQuery<int> { }
		public class Query1 : IQuery<int> { }
		public class Query2 : IQuery<int> { }
		public class Command : ICommand { }

		public void TestApp(IAppBuilder appBuilder)
		{
			//appBuilder.UseCqrsApi(new RouteTable(), new Dispatcher(null));

			BuildTable(new RouteTable());
		}

		private static void BuildTable(RouteTable t)
		{
			t.Route("/blah/blah").UseCommand<Command, Unit>(HttpVerbs.Post);
			t.Use(new TestRoutes());
			t.Use("/v1", new TestRoutes());
		}

		public class Rotues : RouteTable
		{
			public Rotues()
			{
				Route("/blah/blah").UseCommand<Command, Unit>(HttpVerbs.Post);
				Use(new TestRoutes());
				Use("/v1", new TestRoutes());
			}
		}

		public class TestRoutes : RouteTable
		{
			public TestRoutes()
			{
				Route("/lalala/topolya")
					.UseQuery<Query, int>()
					.UseCommand<Command, Unit>(HttpVerbs.Post);

				Route("/lalala/topolya/{id}").UseQuery<Query1, int>();
				Route("/lalala/topolya/{id}/blahblah").UseQuery<Query2, int>();
			}
		}
	}
}