using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public interface ICharacterRepository
    {
        Task<JObject> GetCharacter(int characterId);
    }
}