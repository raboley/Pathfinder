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
            const int wantId = 1;
            const string wantName = "Bob";
            var wantPosition = Vector3.One;

            var got = new Person(wantId, wantName, wantPosition);

            Assert.Equal(wantName, got.Name);
            Assert.Equal(wantPosition, got.Position);
        }
    }
}