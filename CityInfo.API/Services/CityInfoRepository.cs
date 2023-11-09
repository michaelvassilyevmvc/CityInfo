using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CitiInfoContext _context;

        public CityInfoRepository(CitiInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async  Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetCitiesAsync();
            }

            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.Name.Contains(searchQuery) || (x.Description != null && x.Description.Contains(searchQuery)));

            }

            return await collection.OrderBy(x => x.Name).ToListAsync();


        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if(includePointOfInterest)
            {
                return await _context.Cities.Include(x => x.PointsOfInterest).Where(x => x.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(x => x.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(x => x.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterests.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterests.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if(city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests?.Remove(pointOfInterest);
        }
    }
}
