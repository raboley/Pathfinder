using System;
using Pathfinder.Persistence;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class FileTextPersisterTests
    {
        [Fact]
        public void SaveCanPersistTextToFile()
        {
            const string want = @"
-------------------
|  ?  |  ?  |  ?  |
-------------------
|  ?  |     |     |
-------------------
|     |  ?  |     |
-------------------
";
            var persister = SetupPersister.SetupTestFileTextPersister();
            persister.Save(want);

            string got = persister.Load<string>();

            Assert.Equal(want, got);
        }

        [Fact]
        public void SaveThrowsErrorIfYouTryToSaveWithoutFileName()
        {
            var persister = new FileTextPersister();

            Assert.Throws<Exception>(() => persister.Save("Some text"));
        }
    }
}