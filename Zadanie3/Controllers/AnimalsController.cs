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
		
		
		// Metoda do aktualizacji danych konkretnego zwierzęcia
        [HttpPut("{idAnimal}")]
        public ActionResult UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
        {
            var existingAnimal = animals.FirstOrDefault(a => a.IdAnimal == idAnimal);
            if (existingAnimal == null)
            {
                return NotFound("Zwierzę o podanym identyfikatorze nie zostało znalezione.");
            }

            existingAnimal.Name = updatedAnimal.Name;
            existingAnimal.Description = updatedAnimal.Description;
            existingAnimal.Category = updatedAnimal.Category;
            existingAnimal.Area = updatedAnimal.Area;

            return Ok("Dane zwierzęcia zostały zaktualizowane.");
        }
    
		
		
		
		 // Metoda do usuwania danych konkretnego zwierzęcia
        [HttpDelete("{idAnimal}")]
        public ActionResult DeleteAnimal(int idAnimal)
        {
            var existingAnimal = animals.FirstOrDefault(a => a.IdAnimal == idAnimal);
            if (existingAnimal == null)
            {
                return NotFound("Zwierzę o podanym identyfikatorze nie zostało znalezione.");
            }

            animals.Remove(existingAnimal);

            return Ok("Dane zwierzęcia zostały usunięte.");
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