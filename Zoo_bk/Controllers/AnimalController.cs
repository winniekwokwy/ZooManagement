using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Zoo.Controllers;

[ApiController]
[Route("[controller]")]
public class AnimalController : ControllerBase
{

    private readonly ILogger<AnimalController> _logger;
    private readonly ZooDbContext _db;

    public AnimalController(
        ILogger<AnimalController> logger,
        ZooDbContext db)
    {
        _logger = logger;
        _db = db;
    }


    [HttpGet(Name = "GetAnimalbyId/{Id}")]
    public IEnumerable<Animal> Get(int Id)
    {
        var animal = _db.Animals.Where(x => x.Id == Id);
        return animal;
    }

    [HttpPost(Name = "AddAnimal")]
    public ActionResult AddAnimal(Animal animal)
    {
        if (animal.Name == null
        || animal.Sex == null
        || animal.DateofAcquisition == DateOnly.MinValue
        || animal.DateofBirth == DateOnly.MinValue)
        {
            return ValidationProblem("Input information is invalid.");
        }
        _db.Animals.Add(new Animal
        {
            Name = animal.Name,
            Sex = animal.Sex,
            DateofAcquisition = animal.DateofAcquisition,
            DateofBirth = animal.DateofBirth,
            SpeciesId = animal.SpeciesId,
            EnclosureId = animal.EnclosureId
        });
        _db.SaveChanges();
        return Ok($"Animal {animal.Name} is added.");
    }

    [HttpGet(Name = "GetSpeciesInZoo")]
    public IEnumerable<List<Animal>> GetSpeciesInZoo()
    {
        List<List<Animal>> animals = [.. _db.Animals
        .GroupBy(b => b.SpeciesId)
        .Select(group => group.ToList())];

        Console.WriteLine(animals.ToString());
        return animals;
    }
    
}
