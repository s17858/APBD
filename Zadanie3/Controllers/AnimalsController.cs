using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APBD.Controllers;

public class AnimalsController
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private List<Animal> animals = new List<Animal>
        {
            new Animal { Name = "Lew", Description = "Król sawanny", Category = "Ssak", Area = "Afryka" },
            new Animal { Name = "Słoń", Description = "Największe lądowe zwierzę", Category = "Ssak", Area = "Afryka, Azja" },
        };
		
		// Metoda dodawania nowego zwierzęcia
        [HttpPost]
        public ActionResult AddAnimal([FromBody] Animal newAnimal)
        {
            animals.Add(newAnimal);
            return Ok("Nowe zwierzę zostało dodane.");
        }
		

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAnimals(string orderBy = "name")
        {
            IEnumerable<Animal> sortedAnimals;

            switch (orderBy.ToLower())
            {
                case "name":
                    sortedAnimals = animals.OrderBy(a => a.Name);
                    break;
                case "description":
                    sortedAnimals = animals.OrderBy(a => a.Description);
                    break;
                case "category":
                    sortedAnimals = animals.OrderBy(a => a.Category);
                    break;
                case "area":
                    sortedAnimals = animals.OrderBy(a => a.Area);
                    break;
                default:
                    return BadRequest("Nieprawidłowy parametr orderBy. Dostępne wartości: name, description, category, area.");
            }

            return Ok(sortedAnimals);
        }
    }