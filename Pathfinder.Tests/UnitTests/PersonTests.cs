using System.ComponentModel;
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

        [Fact]
        public void PropertyChangedEventTriggersOnPropertyUpdates()
        {
            const int wantId = 1;
            const string wantName = "Bob";
            var wantPosition = Vector3.One;
            var spyPropertyChanged = new spyPropertyChanged();

            var got = new Person(wantId, wantName, wantPosition);
            got.PropertyChanged += spyPropertyChanged.GotOnPropertyChanged;

            got.Position = Vector3.Zero;

            Assert.Equal(wantName, got.Name);
            Assert.Equal(Vector3.Zero, got.Position);
            Assert.Equal(1, spyPropertyChanged.CalledTimes);
        }
    }

    internal class spyPropertyChanged
    {
        public int CalledTimes { get; set; }

        public void GotOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalledTimes++;
        }
    }
}