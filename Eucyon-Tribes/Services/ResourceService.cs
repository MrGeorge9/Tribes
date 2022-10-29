using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class ResourceService
    {
        private readonly ApplicationContext _db;

        public ResourceService(ApplicationContext context)
        {
            this._db = context;
        }

        public void UpdateResource()
        {
            var kingdoms = _db.Kingdoms.Include(k => k.Resources).Include(k => k.Buildings);
            if (kingdoms == null)
            {
                return;
            }

            foreach (var kingdom in kingdoms)
            {
                DateTime newUpdateAt = DateTime.Now;
                UpdateFoodResource(kingdom, newUpdateAt);
                UpdateGoldResource(kingdom, newUpdateAt);
                UpdatePeopleResource(kingdom, newUpdateAt);
                UpdateWoodResource(kingdom, newUpdateAt);
            }
            _db.SaveChanges();
        }

        private void UpdateWoodResource(Kingdom kingdom, DateTime updateTime)
        {
            var woodProduction = kingdom.Buildings.Where(b => b is Sawmill).Sum(b => b.Production);
            Resource wood = kingdom.Resources.FirstOrDefault(r => r is Wood);
            int timeDifferenceInMinutes = (int)updateTime.Subtract(wood.UpdatedAt).TotalMinutes;
            wood.Amount += woodProduction * timeDifferenceInMinutes;
            wood.UpdatedAt = updateTime;
        }

        private void UpdateGoldResource(Kingdom kingdom, DateTime updateTime)
        {
            var goldProduction = kingdom.Buildings.Where(b => b is Mine).Sum(b => b.Production);
            Resource gold = kingdom.Resources.FirstOrDefault(r => r is Gold);
            int timeDifferenceInMinutes = (int)updateTime.Subtract(gold.UpdatedAt).TotalMinutes;
            gold.Amount += goldProduction * timeDifferenceInMinutes;
            gold.UpdatedAt = updateTime;
        }

        private void UpdatePeopleResource(Kingdom kingdom, DateTime updateTime)
        {
            var peopleProduction = kingdom.Buildings.Where(b => b is TownHall).Sum(b => b.Production);
            Resource people = kingdom.Resources.FirstOrDefault(r => r is People);
            int timeDifferenceInMinutes = (int)updateTime.Subtract(people.UpdatedAt).TotalMinutes;
            people.Amount += peopleProduction * timeDifferenceInMinutes;
            people.UpdatedAt = updateTime;
        }

        private void UpdateFoodResource(Kingdom kingdom, DateTime updateTime)
        {
            var foodProduction = kingdom.Buildings.Where(b => b is Farm).Sum(b => b.Production);
            Resource food = kingdom.Resources.FirstOrDefault(r => r is Food);
            int timeDifferenceInMinutes = (int)updateTime.Subtract(food.UpdatedAt).TotalMinutes;
            int soldierAmount = kingdom.Resources.Where(r => r is Soldier).First().Amount;
            food.Amount += (foodProduction - soldierAmount) * timeDifferenceInMinutes;
            food.UpdatedAt = updateTime;
        }
    }
}