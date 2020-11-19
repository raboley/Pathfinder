using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class ZoningStateTests
    {
        // [Fact]
        // public void ZoningCanBreakOutOfStateMachine()
        // {
        //     const string targetZoneName = "B";
        //     var targetPosition = ExampleWorld.ZoneA().Boundaries.Find(b => b.ToZone == targetZoneName).ToPosition;
        //     var traveler = new Traveler
        //     {
        //         CurrentZone = ExampleWorld.ZoneA(),
        //         Position = Vector3.Zero,
        //         World = ExampleWorld.Sample()
        //     };
        //
        //     var zoner = new Zoner(traveler, ExampleWorld.Sample());
        //     while (traveler.Walker.Zoning == false)
        //     {
        //         traveler.GoToZone(targetZoneName);
        //     }
        //
        //     Assert.Equal(targetZoneName, traveler.CurrentZone.Name);
        //     Assert.Equal(targetPosition, traveler.Walker.CurrentPosition);
        // }

        [Fact]
        public void StartAndCancel()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var tasks = Enumerable.Repeat(0, 2)
                .Select(i => Task.Run(() => Dial(token), token))
                .ToArray(); // start dialing on two threads
            Thread.Sleep(200); // give the tasks time to start
            cancellationTokenSource.Cancel();
            Assert.Throws<AggregateException>(() => Task.WaitAll(tasks));
            Assert.True(tasks.All(t => t.Status == TaskStatus.Canceled));
        }

        public void Dial(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine("Called from thread {0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(50);
            }
        }
    }
}