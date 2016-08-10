using System.Collections.Concurrent;
using System.Collections.Generic;
using marvel_api.Threading;

namespace marvel_api.Characters
{
    public class CharacterCacheService : ICharacterCacheService
    {
        private static readonly ReaderWriterLock _slimLock = new ReaderWriterLock();

        private readonly ConcurrentDictionary<string, CharacterModel> _characterCache;
        private IList<CharacterModel> _characterListCache;

        public CharacterCacheService()
        {
            _characterCache = new ConcurrentDictionary<string, CharacterModel>();
            _characterListCache = null;
        }

        public bool TryGetCharacter(string characterName, out CharacterModel character)
        {
            character = null;
            var found = _characterCache.TryGetValue(characterName, out character);

            if (!found && _characterListCache != null)
            {
                using(_slimLock.EnterReadLock())
                {
                    foreach(var cacheCharacter in _characterListCache)
                    {
                        _characterCache[cacheCharacter.Name] = cacheCharacter;
                        
                        if (cacheCharacter.Name == characterName)
                        {
                            character = cacheCharacter;
                            found = true;
                        }
                    }
                }
            }

            return found;
        }

        public void SetCharacter(CharacterModel character)
        {
            _characterCache[character.Name] = character;
        }

        public bool TryGetCharacterList(out IList<CharacterModel> charactersList)
        {
            bool found = false;

            using(_slimLock.EnterReadLock())
            {
                charactersList = _characterListCache;
                found = charactersList != null;
            }

            return found;
        }

        public void SetCharacterList(IList<CharacterModel> characterList)
        {
            if (_characterListCache == null)
            {
                using(_slimLock.EnterWriteLock())
                {
                    if (_characterListCache == null)
                    {
                        _characterListCache = characterList;
                    }
                }   
            }
        }
    }
}