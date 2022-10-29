using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class WorldService : IWorldService
    {
        private readonly ApplicationContext _db;

        public WorldService(ApplicationContext db)
        {
            this._db = db;
        }

        public WorldResponseDTO[] GetWorldsWithKingdoms()
        {
            var worlds = _db.Worlds.Include(w => w.Kingdoms).ToList();
            if (worlds == null || worlds.Count() == 0)
            {
                return new WorldResponseDTO[0];
            }
            WorldResponseDTO[] result = new WorldResponseDTO[worlds.Count()];
            for (int i = 0; i < worlds.Count(); i++)
            {
                result[i] = new WorldResponseDTO(worlds.ElementAt(i).Id, worlds.ElementAt(i).Name, worlds.ElementAt(i).Kingdoms.Count);
            }
            return result;
        }

        public bool CreateWorld(string name)
        {
            if (name == null || name.Equals(string.Empty))
            {
                return false;
            }
            if (_db.Worlds.Any(w => w.Name == name))
            {
                return false;
            }
            World newWorld = new World() { Name = name, Kingdoms = new List<Kingdom>(), Locations = new List<Location>()};
            _db.Worlds.Add(newWorld);
            _db.SaveChanges();
            return true;
        }

        public bool StoreWorld(StoreWorldDTO? newWorldDTO)
        {
            if (newWorldDTO == null)
            {
                return false;
            }
            if (_db.Worlds.Any(w => w.Name.Equals(newWorldDTO.Name)) || newWorldDTO.Name.Equals(String.Empty))
            {
                return false;
            }
            World newWorld = new World() { Name = newWorldDTO.Name};
            newWorld.Kingdoms = newWorldDTO.Kingdoms == null ? new List<Kingdom>() : newWorldDTO.Kingdoms;
            newWorld.Locations = newWorldDTO.Locations == null ? new List<Location>() : newWorldDTO.Locations;
            _db.Worlds.Add(newWorld);
            _db.SaveChanges();
            return true;
        }

        public WorldDetailDTO GetWorldDetails(int id)
        {
            var world = _db.Worlds.Include(w => w.Kingdoms).FirstOrDefault(w => w.Id == id);
            if (world == null)
            {
                return null;
            }
            var kingdomNames = new List<string>();
            foreach (var kingdom in world.Kingdoms)
            {
                kingdomNames.Add(kingdom.Name);
            }
            var worldDTO = new WorldDetailDTO(world.Id, world.Name, kingdomNames);
            return worldDTO;
        }

        public bool EditWorld(int id, string? name)
        {
            var world = _db.Worlds.FirstOrDefault(w => w.Id == id);
            if (world == null)
            {
                return false;
            }
            if (name == null || name.Equals(string.Empty) || world.Name == name)
            {
                return false;
            }
            world.Name = name;
            _db.Worlds.Update(world);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateWorld(int id, UpdateWorldDTO newWorldDTO)
        {
            var world = _db.Worlds.FirstOrDefault(w => w.Id == id);
            if (world == null || newWorldDTO == null)
            {
                return false;
            }
            world.Name = newWorldDTO.Name;
            _db.Worlds.Update(world);
            _db.SaveChanges();
            return true;
        }

        public bool DestroyWorld(int id)
        {
            var world = _db.Worlds.FirstOrDefault(w => w.Id == id);
            if (world == null)
            {
                return false;
            }
            var kingdoms = _db.Kingdoms.Where(k => k.WorldId == id);
            foreach (var kingdom in kingdoms)
            {
                _db.Kingdoms.Remove(kingdom);
            }
            var locations = _db.Locations.Where(k => k.WorldId == id);
            foreach (var location in locations)
            {
                _db.Locations.Remove(location);
            }
            world.Kingdoms = new List<Kingdom>();
            world.Locations = new List<Location>();
            _db.Worlds.Remove(world);
            _db.SaveChanges();
            return true;
        }
    }
}
