using ffxi.Tests.stubs;
using MemoryAPI;
using Xunit;

namespace ffxi.Tests.FeatureTests
{
    public class GetSignet
    {
        [Fact]
        public void GetSignetFromPersonWhileBastok()
        {
            // Setup
            var player = new PlayerStub();
            
            // Check if player needs signet
            // True
            // Find the closest signet person for player nation (Bastok)
            // 
            // Pathfind to that location
            // Walk to that location
            // Talk to the signet person
            // continue
            
            Assert.Contains(player.StatusEffects, effect => effect == StatusEffect.Signet);   
        }
    }


}