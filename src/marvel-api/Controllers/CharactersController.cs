using System.Threading.Tasks;
using marvel_api.Characters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace marvel_api.Controllers
{
    [Route("v1/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ICharacterRepository _characterRepository;

        public CharactersController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        [HttpGet]
        public async Task<JObject> GetAsync()
        {
            return await BuildCharactersAsync();
        }

        [HttpGet("{name}")]
        public async Task<JObject> GetAsync(string name)
        {
            return await _characterRepository.GetCharacter(int.Parse(name));
        }

        private async Task<JObject> BuildCharactersAsync()
        {
            JObject characters = await Task.Factory.StartNew(() => {
                // Imagine this is I/O, call to an API
                return new JObject(
                    new JProperty("characters", new JArray(
                        new JObject(
                            new JProperty("name", "spider-man")
                        ),
                        new JObject(
                            new JProperty("name", "hulk")
                        )
                    ))
                );
            });

            return characters;
        }

        private async Task<JObject> BuildCharacterAsync(string name)
        {
            JObject character = await Task.Factory.StartNew(() => {
                // Imagine this is I/O, call to an API
                return new JObject(
                    new JProperty("name", name)
                );
            });

            return character;
        }
    }
}
