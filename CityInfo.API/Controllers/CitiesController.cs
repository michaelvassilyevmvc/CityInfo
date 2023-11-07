using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController: ControllerBase
    {
        [HttpGet]
        public ActionResult<CityDto> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityReturn = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);
            if(cityReturn == null)
            {
                return NotFound();
            }
            return Ok(cityReturn);
        }
    }
}
