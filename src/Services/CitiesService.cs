namespace Company.Function;

public class CitiesService : ICitiesService
{
    private readonly List<City> _cities = new List<City>
    {
        new City { Id = "1", Name = "Paris" },
        new City { Id = "2", Name = "Berlin" },
        new City { Id = "3", Name = "Madrid" },
        new City { Id = "4", Name = "Rome" },
        new City { Id = "5", Name = "San Francisco" },
        new City { Id = "6", Name = "New York" },
        new City { Id = "7", Name = "Moscow" },
        new City { Id = "8", Name = "Sydney" },
        new City { Id = "9", Name = "Toronto" },
        new City { Id = "10", Name = "Amsterdam" }
    };

    public Task<IEnumerable<City>> GetCitiesAsync()
    {
        return Task.FromResult<IEnumerable<City>>(_cities);
    }

    public Task<City> GetCityByIdAsync(string id)
    {
        var city = _cities.FirstOrDefault(c => c.Id == id);
        return Task.FromResult<City>(city);
    }

    public Task AddCityAsync(City city)
    {
        _cities.Add(city);
        return Task.CompletedTask;
    }

    public Task UpdateCityAsync(string id, City city)
    {
        var existingCity = _cities.FirstOrDefault(c => c.Id == id);
        if (existingCity != null)
        {
            existingCity.Name = city.Name;
        }
        return Task.CompletedTask;
    }

    public Task DeleteCityAsync(string id)
    {
        var city = _cities.FirstOrDefault(c => c.Id == id);
        if (city != null)
        {
            _cities.Remove(city);
        }
        return Task.CompletedTask;
    }
}