namespace Company.Function;

public interface ICitiesService
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City> GetCityByIdAsync(string id);
    Task AddCityAsync(City city);
    Task UpdateCityAsync(string id, City city);
    Task DeleteCityAsync(string id);
}
