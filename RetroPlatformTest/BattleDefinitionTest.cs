using System;
using RetroPlatform.Battle;
using UnityEngine;
using Xunit;

namespace RetroPlatformTest
{
    public class BattleDefinitionTest
    {
        [Fact]
        public void ShouldNotStartBattleWithNameNone()
        {
            Exception ex = Assert.Throws<UnassignedReferenceException>(() => BattleDefinition.GetBattle(BattleName.None, null));

            Assert.Equal("Invalid BattleName: None", ex.Message);
        }
    }
}
