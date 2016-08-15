using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public interface ICharacterService
    {
        Task<JObject> GetCharacter(string characterName);

        Task<JObject> GetCharacters();

        Task HydrateCharacterCache();
    }
}