using System;
using System.Linq;
using Ensage;

namespace iZeus
{
    internal class Program
    {
        private static Hero Player;

        private static void Main(string[] args)
        {
            Player = ObjectMgr.LocalHero;

            // Listen to Events
            Game.OnUpdate += Game_OnUpdate;
        }

        private static float CalculatedDamage(Unit unit)
        {
            var ability = Player.Spellbook.SpellR;
            var damage = iUtility.HasItem(ClassID.CDOTA_Item_UltimateScepter)
                ? ability.AbilityData.First(x => x.Name == "damage_scepter").GetValue(Player.Level - 1)
                : ability.AbilityData.First(x => x.Name == "damage").GetValue(ability.Level - 1);

            return damage*unit.MagicDamageResist;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Player.Spellbook.SpellR.IsReady())
                return;

            foreach (
                var hero in
                    ObjectMgr.GetEntities<Hero>()
                        .Where(hero => hero.IsValidTarget() && CalculatedDamage(hero) > hero.Health))
            {
                Player.Spellbook.SpellR.Cast();
            }
        }
    }
}
