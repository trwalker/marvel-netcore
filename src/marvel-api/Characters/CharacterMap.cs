using System.Collections.Generic;

namespace marvel_api.Characters
{
    public static class CharacterMap
    {
        private static readonly IDictionary<string, int> _characterNameToIdMap = new Dictionary<string, int> {
          { "spider-man", 1009610 },
          { "hulk", 1009351 },
          { "captain-america", 1009220 },
          { "iron-man", 1009368 },
          { "thor", 1009664 },
          { "wolverine", 1009718 },
          { "storm", 1009629 },
          { "jean-grey", 1009496 },
          { "gambit", 1009313 },
          { "cyclops", 1009257 },
          { "beast", 1009175 },  
        };

        public static bool TryGetCharacterIdByName(string name, out int id)
        {
            return _characterNameToIdMap.TryGetValue(name, out id);
        }

        public static ICollection<int> GetCharacterIds()
        {
            return _characterNameToIdMap.Values;
        }
    }
}