using System.Collections.Concurrent;
using System.Collections.Generic;
using marvel_api.Threading;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public class CharacterCacheService : ICharacterCacheService
    {
        private static readonly ReaderWriterLock _slimLock;
        private static IDictionary<string, JObject> _characterCache;
        private static JObject _characterListCache;
        private static volatile bool _isHydrateInProgress;
        
        static CharacterCacheService()
        {
            _slimLock = new ReaderWriterLock();
            _characterCache = new ConcurrentDictionary<string, JObject>();
            _characterListCache = null;
            _isHydrateInProgress = false;
        }

        public void HydrateCache(IList<CharacterModel> characters)
        {
            if(!_isHydrateInProgress)
            {
                using(_slimLock.EnterWriteLock())
                {
                    if(!_isHydrateInProgress && IsCacheEmpty())
                    {
                        _isHydrateInProgress = true;

                        var charactersJson = new JObject(
                            new JProperty("characters", JArray.FromObject(characters))
                        );

                        _characterListCache = charactersJson;

                        var characterMap = new Dictionary<string, JObject>(characters.Count);

                        foreach(var character in characters)
                        {
                            characterMap[character.Name.ToLowerInvariant()] = JObject.FromObject(character);
                        }

                        _characterCache = characterMap;

                        _isHydrateInProgress = false;
                    }
                }
            }
            
        }

        public bool TryGetCharacter(string characterName, out JObject character)
        {
            if(_isHydrateInProgress)
            {
                using (_slimLock.EnterReadLock())
                {
                    return _characterCache.TryGetValue(characterName.ToLowerInvariant(), out character);
                }
            }
            else 
            {
                return _characterCache.TryGetValue(characterName.ToLowerInvariant(), out character);
            }
        }

        public bool TryGetCharacterList(out JObject characters)
        {
            if(_isHydrateInProgress)
            {
                using(_slimLock.EnterReadLock())
                {
                    characters = _characterListCache;
                }
            }
            else
            {
                characters = _characterListCache;
            }
            
            return characters != null;
        }

        private bool IsCacheEmpty()
        {
            return _characterListCache != null || _characterCache == null || _characterCache.Count == 0;
        }
    }
}