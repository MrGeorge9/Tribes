using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class BattleService : IBattleService
    {
        private readonly ApplicationContext _db;
        private readonly IArmyFactory _armyFactory;
        public String ErrorMessage;
        private readonly RuleService _ruleService;
        private readonly IArmyService _armyService;
        private readonly IBattleFactory _battleFactory;

        public BattleService(ApplicationContext db, IArmyFactory armyFactory, IArmyService armyService,
            RuleService ruleService, IBattleFactory battleFactory)
        {
            _ruleService = ruleService;
            _armyService = armyService;
            _db = db;
            _armyFactory = armyFactory;
            _battleFactory = battleFactory;
        }
        public void Battle(Battle battle)
        {
            Random random = new Random();
            Army attacker = _db.Armies.FirstOrDefault(a => a.Id == battle.AttackerArmyId);
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).FirstOrDefault(k => k.Id == battle.DefenderId);
            List<Resource> resources = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).ToList();
            List<Soldier> soldiers = new List<Soldier>();
            foreach (Resource resource in resources)
            {
                soldiers.Add((Soldier)resource);
            }
            List<Soldier> defenders = new List<Soldier>();
            foreach (Soldier soldier in soldiers)
            {
                if (soldier.Army == null)
                    defenders.Add(soldier);
            }
            Army defender = _armyFactory.CrateArmy(defenders, kingdom, 0);
            while (attacker.Soldiers.Count() > 0 && defender.Soldiers.Count() > 0)
            {
                Soldier soldier = attacker.Soldiers[random.Next(attacker.Soldiers.Count())];
                Soldier attacked = defender.Soldiers[random.Next(defender.Soldiers.Count())];
                attacked.CurrentHP = attacked.CurrentHP - soldier.Attack / 3 - random.Next(soldier.Attack / 3 * 2 + 1);
                soldier.CurrentHP = soldier.CurrentHP - attacked.Defense / 3 - random.Next(attacked.Defense / 3 * 2 + 1);
                if (soldier.CurrentHP <= 0)
                {
                    attacker.Soldiers.Remove(soldier);
                    soldier = null;
                }
                if (attacked.CurrentHP <= 0)
                {
                    defender.Soldiers.Remove(attacked);
                    attacked = null;
                }
                Army winner = RouteCheck(attacker, defender);
                if (winner != null)
                {
                    battle.WinnerId = winner.Kingdom.Id;
                    break;
                }
            }
            Kingdom winningKingdom;
            if ((attacker.Soldiers.Count() == 0 && defender.Soldiers.Count() == 0)
                || attacker.Soldiers.Count() == 0)
            {
                winningKingdom = defender.Kingdom;
            }
            else
            {
                winningKingdom = attacker.Kingdom;
                Plunder(attacker, defender);
            }
            battle.WinnerId = winningKingdom.Id;
            HPReset(attacker);
            HPReset(defender);
            _db.SaveChanges();
        }

        public Army RouteCheck(Army attacker, Army defender)
        {
            if (attacker.Soldiers.Sum(s => s.CurrentHP) / attacker.Soldiers.Sum(s => s.TotalHP) >= 0.7 &&
                defender.Soldiers.Sum(s => s.CurrentHP) / defender.Soldiers.Sum(s => s.TotalHP) <= 0.1)
            {
                defender.Soldiers.Clear();
                return attacker;
            }
            if (attacker.Soldiers.Sum(s => s.CurrentHP) / attacker.Soldiers.Sum(s => s.TotalHP) < 0.2 &&
                defender.Soldiers.Sum(s => s.CurrentHP) / defender.Soldiers.Sum(s => s.TotalHP) <= 0.7)
            {
                List<Soldier> survivors = new List<Soldier>();
                Random random = new Random();
                foreach (Soldier soldier in attacker.Soldiers)
                {
                    if (random.Next(2) == 1)
                    {
                        survivors.Add(soldier);
                    }
                }
                attacker.Soldiers = survivors;
                return defender;
            }
            return null;
        }
        //this method was not very well documented in task description and at the time writing this code most of my team is not available,
        //so numbers here as well as method of storing them might be a subject to change
        public void Plunder(Army attacker, Army defender)
        {
            List<Resource> defenderResources = _db.Resources.Where(r => r.Kingdom.Id == defender.Kingdom.Id).ToList();
            List<Resource> attackerResources = _db.Resources.Where(r => r.Kingdom.Id == attacker.Kingdom.Id).ToList();
            foreach (Resource defenderResource in defenderResources)
            {
                if (defenderResource.GetType() != typeof(People) && defenderResource.GetType() != typeof(People))
                {
                    Resource attackerResource = attackerResources.FirstOrDefault(r => r.GetType() == defenderResource.GetType());
                    attackerResource.Amount = attackerResource.Amount + (int)(0.3 * defenderResource.Amount);
                    defenderResource.Amount = (int)(defenderResource.Amount * 0.7);
                }
            }
            _db.SaveChanges();
        }
        public void HPReset(Army army)
        {
            foreach (Soldier soldier in army.Soldiers)
            {
                soldier.CurrentHP = soldier.TotalHP;
            }
            _db.SaveChanges();
        }
        //attacker id in bettle request to be replaced with token authentication
        public StatusDTO CreateBattle(BattleRequestDTO battleRequest, int id)
        {
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Location).Include(k => k.World).FirstOrDefault(k => k.Id == id
            && _db.Kingdoms.FirstOrDefault(k2 => k2.Id == battleRequest.AttackerId).World == k.World);
            if (id < 1)
            {
                ErrorMessage = "Invalid kingdom Id";
                return null;
            }
            if (kingdom == null)
            {
                ErrorMessage = "Kingdom not found";
                return null;
            }
            Kingdom attackingKingdom = _db.Kingdoms.Include(k => k.Location).Include(k => k.World).FirstOrDefault(k => k.Id == battleRequest.AttackerId);
            double distance = Math.Sqrt(Math.Pow(attackingKingdom.Location.XCoordinate - kingdom.Location.XCoordinate, 2) +
               Math.Pow(attackingKingdom.Location.YCoordinate - kingdom.Location.YCoordinate, 2));
            BattleCostDTO battleCost = this.BattleCost(battleRequest, id);
            if (battleCost == null)
            {
                return null;
            }
            Army army = _armyService.CreateArmy(battleRequest.NumberOfUnitsByLevel, _db.Kingdoms.FirstOrDefault(k => k.Id == battleRequest.AttackerId).Id, (int)distance);
            if (battleCost.food > army.Kingdom.Resources.FirstOrDefault(r => r.GetType() == typeof(Food)).Amount)
            {
                ErrorMessage = "Insufficient food to attack";
                return null;
            }
            _db.Armies.Add(army);
            Battle battle = _battleFactory.CreateBattle(army.Kingdom, kingdom, army.Id, (int)distance);
            _db.Battles.Add(battle);
            kingdom.CanBeAttackedAt = DateTime.Now.AddHours(_ruleService.CanBeAttacked());
            Resource attackersFood = attackingKingdom.Resources.FirstOrDefault(r => r.GetType() == typeof(Food));
            attackersFood.Amount = attackersFood.Amount - battleCost.food;
            _db.SaveChanges();
            return new StatusDTO("Attack order issued");
        }

        public BattleCostDTO BattleCost(BattleRequestDTO battleRequest, int id)
        {
            if (id < 1)
            {
                ErrorMessage = "Invalid kingdom Id";
                return null;
            }
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Location).Include(k => k.World).FirstOrDefault(k => k.Id == id
            && _db.Kingdoms.FirstOrDefault(k2 => k2.Id == battleRequest.AttackerId).World == k.World);
            if (kingdom == null)
            {
                ErrorMessage = "Kingdom not found";
                return null;
            }
            Kingdom attackingKingdom = _db.Kingdoms.Include(k => k.Location).Include(k => k.World).FirstOrDefault(k => k.Id == battleRequest.AttackerId);
            double distance = Math.Sqrt(Math.Pow(attackingKingdom.Location.XCoordinate - kingdom.Location.XCoordinate, 2) +
               Math.Pow(attackingKingdom.Location.YCoordinate - kingdom.Location.YCoordinate, 2));
            Army army = _armyService.CreateArmy(battleRequest.NumberOfUnitsByLevel, _db.Kingdoms.FirstOrDefault(a => a.Id == battleRequest.AttackerId).Id, (int)distance);
            if (army == null)
            {
                ErrorMessage = _armyService.GetError();
                return null;
            }
            if (id == army.Kingdom.Id)
            {
                ErrorMessage = "You cannot attack yourself";
                return null;
            }
            //token in header needs to be inplemented first
            /*if (army does not belong to payler)
            {
                ErrorMessage = "Unauthorized";
            }*/
            if (!(kingdom.CanBeAttackedAt <= DateTime.Now))
            {
                ErrorMessage = "Kingdom can not be attacked yet, as it was attacked recently";
                return null;
            }
            BattleCostDTO battleCostDTO = new BattleCostDTO(_ruleService.getRequiredAmountOfFoodForBattle(army.Kingdom, kingdom, army));
            foreach (Soldier soldier in army.Soldiers)
            {
                soldier.Army = null;
            }
            return battleCostDTO;
            //might want to update this to include gold
        }

        public String GetError()
        {
            String output = ErrorMessage;
            ErrorMessage = null;
            return output;
        }
    }
}