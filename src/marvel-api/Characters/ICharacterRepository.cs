
using System.Threading.Tasks;

namespace marvel_api.Characters
{
    public interface ICharacterRepository
    {
        Task<CharacterModel> GetCharacter(int characterId);
    }
}