using System.Collections.Generic;

namespace marvel_api.Characters
{
    public interface ICharacterCacheService
    {
        bool TryGetCharacter(string characterName, out CharacterModel character);

        bool TryGetCharacterList(out IList<CharacterModel> characterList);

        void SetCharacter(CharacterModel character);

        void SetCharacterList(IList<CharacterModel> characterList);
    }
}