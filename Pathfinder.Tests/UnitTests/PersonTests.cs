using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PersonTests
    {
        [Fact]
        public void ConstructorCreatesPerson()
        {
            const string wantName = "Bob";
            var wantPosition = Vector3.One;
            var got = new Person(wantName, wantPosition);

            Assert.Equal(wantName, got.Name);
            Assert.Equal(wantPosition, got.Position);
        }

        [Fact]
        public void CanAddIdToPerson()
        {
            var want = 12;
            var got = new Person("Suzy", Vector3.One);
            got.Id = want;

            Assert.Equal(want, got.Id);
        }
    }
}