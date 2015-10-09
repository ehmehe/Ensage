using System.Linq;
using Ensage;
using SharpDX;

namespace iZeus
{
    public static class iUtility
    {
        public static bool IsValidTarget(this Unit unit,float range = float.MaxValue, bool checkTeam = true)
        {
            if (unit == null || !unit.IsValid || !unit.IsAlive || !unit.IsVisible || !unit.IsSpawned)
            {
                return false;
            }

            if (checkTeam && unit.Team == ObjectMgr.LocalHero.Team)
            {
                return false;
            }

            var @base = unit as Hero;
            var unitPosition = @base != null ? @base.NetworkPosition : unit.Position;

            return !(range < float.MaxValue) ||
                !(Vector2.DistanceSquared((ObjectMgr.LocalHero.NetworkPosition).To2D(), unitPosition.To2D()) > range * range);
        }

        public static bool HasItem(ClassID classId)
        {
            return (ObjectMgr.LocalHero.Inventory.Items.Any(item => item.ClassID == classId));
        }

        public static bool IsReady(this Ability ability)
        {
            return ability.AbilityState == AbilityState.Ready && ability.Level > 0;
        }

        public static void Cast(this Ability ability)
        {
            if (ability.IsReady())
            {
                ability.UseAbility();
            }
        }

        public static void Cast(this Ability ability, Unit unit)
        {
            if (ability.IsReady() && unit.IsValidTarget(ability.CastRange))
            {
                ability.UseAbility(unit);
            }
        }
    }
}
