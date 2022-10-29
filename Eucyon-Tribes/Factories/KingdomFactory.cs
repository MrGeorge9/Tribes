using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Eucyon_Tribes.Services;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Factories
{
    public class KingdomFactory : IKingdomFactory
    {
        private readonly ApplicationContext _db;
        private readonly IResourceFactory _resourceFactory;
        private readonly IBuildingFactory _buildingFactory;
        private readonly RuleService _ruleService;

        public KingdomFactory(ApplicationContext db, IResourceFactory resourceFactory, IBuildingFactory buildingFactory
            , RuleService ruleService)
        {
            _db = db;
            _resourceFactory = resourceFactory;
            _buildingFactory = buildingFactory;
            _ruleService = ruleService;
        }

        public bool CreateKingdom(User user, String name, World world)
        {
            int[,] field = new int[16, 16];
            int freeFieldsCount = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (world.Locations.All(k => Math.Abs(k.XCoordinate - i) > 1 || Math.Abs(k.YCoordinate - j) > 1))
                    {
                        field[i, j] = 1;
                        freeFieldsCount++;
                    }
                }
            }
            if (freeFieldsCount == 0)
            {
                return false;
            }
            else
            {
                Random random = new Random();
                int spot = random.Next(freeFieldsCount);
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (field[i, j] == 1)
                        {
                            if (spot == 0)
                            {
                                Kingdom kingdom = new Kingdom(name, user.Id, world.Id);
                                kingdom.CanBeAttackedAt = DateTime.Now.AddDays(2.0);
                                _db.Kingdoms.Add(kingdom);
                                _db.SaveChanges();
                                var kingdomId = _db.Kingdoms.FirstOrDefault(k => k.UserId.Equals(user.Id)).Id;
                                Location location = new Location(i, j, kingdomId, world.Id);      
                                AddResourcesToNewKingdom(location);
                                return true;
                            }
                            else
                            {
                                spot--;
                            }
                        }                     
                    }
                }
                return true;
            }
        }

        public string CreateKingdomWithCoordinates(KingdomCreateRequestDTO request)
        {
            if (_db.Locations.Any(l => l.XCoordinate.Equals(request.Coordinate_X) 
                && l.YCoordinate.Equals(request.Coordinate_Y) 
                && l.WorldId.Equals(request.WorldId)))
            {
                return "This Place has been occupied";
            }
            else if (_db.Kingdoms.Include(k => k.Location).Any(k => k.Name.Equals(request.Name) && k.Location.WorldId.Equals(request.WorldId)))
            {
                return "Kingdom with that name already exists";
            }
            else if (_db.Locations.Any(l => Math.Abs(l.XCoordinate - request.Coordinate_X) < 2 && Math.Abs(l.YCoordinate - request.Coordinate_Y) < 2 && l.WorldId.Equals(request.WorldId)))
            {
                return "Coordinates are too close to another kingdom";
            }
            else
            {
                Kingdom kingdom = new Kingdom(request.Name, request.UserId, request.WorldId);
                kingdom.CanBeAttackedAt = DateTime.Now.AddDays(2.0);
                _db.Kingdoms.Add(kingdom);
                _db.SaveChanges();
                var kingdomId = _db.Kingdoms.FirstOrDefault(k => k.UserId.Equals(request.UserId)).Id;
                Location location = new Location(request.Coordinate_X, request.Coordinate_Y
                    , kingdomId, request.WorldId);                     
                AddResourcesToNewKingdom(location);
                return "Kingdom created";
            }
        }
        
        private void AddResourcesToNewKingdom(Location location)
        {
            Resource gold = _resourceFactory.GetGoldResource();
            gold.Amount = _ruleService.getKingdomInitialGoldAmount();
            gold.KingdomId = location.KingdomId;
            Resource food = _resourceFactory.GetFoodResource();
            food.Amount = _ruleService.getKingdomInitialFoodAmount();
            food.KingdomId = location.KingdomId;
            Resource people = _resourceFactory.GetPeopleResource();
            people.KingdomId = location.KingdomId;
            Resource wood = _resourceFactory.GetWoodResource();
            wood.KingdomId = location.KingdomId;
            _db.Resources.Add(gold);
            _db.Resources.Add(food);
            _db.Resources.Add(people);
            _db.Resources.Add(wood);
            Building townHall = _buildingFactory.CreateTownHall();
            townHall.KingdomId = location.KingdomId;
            _db.Buildings.Add(townHall);
            _db.Locations.Add(location);
            _db.SaveChanges();
        }
    }
}
