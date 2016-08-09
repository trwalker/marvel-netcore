using System.Collections.Generic;
using System.Threading.Tasks;

namespace marvel_api.Characters
{
    public interface ICharacterService
    {
        Task<CharacterModel> GetCharacter(string characterName);

        Task<IList<CharacterModel>> GetCharacters();
    }
}