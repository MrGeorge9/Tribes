using Eucyon_Tribes.Models;

namespace Eucyon_Tribes.Factories
{
    public interface IBattleFactory
    {
        public Battle CreateBattle(Kingdom attacker, Kingdom defender, int armyId, int distance);
    }
}
