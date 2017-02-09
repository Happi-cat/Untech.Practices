using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.Practices.CQRS.Owin.Routing;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Owin.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
		}
	}

	public class DummyQuery : IQuery<int> { }
	public class DummyCommand: ICommand<int> { }

	public class RoutesExample : RouteTable
	{
		public RoutesExample()
		{
			Use("/books", new Books());
			Use("/profile", new Profile());
			Use("/admin", new Admin());
		}

		public class Books : RouteTable
		{
			public Books()
			{
				Route("")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{bookId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/{bookId}/preview")
					.UseQuery<DummyQuery, int>();
			}
		}

		public class Profile : RouteTable
		{
			public Profile()
			{
				Route("")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/me")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{profileId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);
			}
		}

		public class Admin : RouteTable
		{
			public Admin()
			{
				Use("/users", new AdminUsers());
				Use("/groups", new AdminUsers());
				Use("/roles", new AdminUsers());
			}
		}

		public class AdminUsers:RouteTable
		{
			public AdminUsers()
			{
				Route("")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{userId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/{userId}/lock")
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{userId}/unlock")
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);
			}
		}

		public class AdminGroups: RouteTable
		{
			public AdminGroups()
			{
				Route("")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{groupId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/{groupid}/members")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{groupid}/members/{memberId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);
			}
		}

		public class AdminRoles: RouteTable
		{
			public AdminRoles()
			{
				Route("")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{roleId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/{roleId}/users")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/{roleId}/user/{userId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);
			}
		}
	}
}
