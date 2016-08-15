using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public interface ICharacterCacheService
    {
        void HydrateCache(IList<CharacterModel> characters);

        bool TryGetCharacter(string characterName, out JObject character);

        bool TryGetCharacterList(out JObject characters);
    }
}