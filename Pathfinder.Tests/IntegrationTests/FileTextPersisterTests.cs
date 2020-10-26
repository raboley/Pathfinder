using System;
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
            var persister = PersisterSetup.SetupTestFileTextPersister();
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