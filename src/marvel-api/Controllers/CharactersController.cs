using System.Threading.Tasks;
using marvel_api.Characters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace marvel_api.Controllers
{
    [Route("v1/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<JObject> GetAsync()
        {
            var characters = await _characterService.GetCharacters();

            return new JObject(
                new JProperty("characters", JArray.FromObject(characters))
            );
        }

        [HttpGet("{name}")]
        public async Task<JObject> GetAsync(string name)
        {
            var character = await _characterService.GetCharacter(name);
            
            return JObject.FromObject(character);
        }
    }
}