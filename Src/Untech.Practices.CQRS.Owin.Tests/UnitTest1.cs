using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.Practices.CQRS.Owin.Routing;

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
	public class DummyCommand : ICommand<int> { }

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

				Route("/{bookId}/publish")
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);
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
				UseAdminUsers();
				UseAdminGroups();
				UseAdminRoles();
			}

			private void UseAdminUsers()
			{
				Route("/users")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/users/{userId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/users/{userId}/lock")
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/users/{userId}/unlock")
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);
			}

			private void UseAdminGroups()
			{
				Route("/groups")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/groups/{groupId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/groups/{groupid}/members")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/groups/{groupid}/members/{memberId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);
			}

			private void UseAdminRoles()
			{
				Route("/roles")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/roles/{roleId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);

				Route("/roles/{roleId}/users")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Post);

				Route("/roles/{roleId}/user/{userId}")
					.UseQuery<DummyQuery, int>()
					.UseCommand<DummyCommand, int>(HttpVerbs.Put)
					.UseCommand<DummyCommand, int>(HttpVerbs.Delete);
			}
		}
	}
}
