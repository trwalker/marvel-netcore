using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public class CharacterService : ICharacterService
    {
        private ICharacterCacheService _characterCacheService;
        private ICharacterRepository _characterRepository;

        public CharacterService(ICharacterCacheService characterCacheService, ICharacterRepository characterRepository)
        {
            _characterCacheService = characterCacheService;
            _characterRepository = characterRepository;
        }

        public async Task HydrateCharacterCache()
        {
            var characterIds = CharacterMap.GetCharacterIds();
            IList<Task<JObject>> characterGetTasks = new List<Task<JObject>>(characterIds.Count);
            var characters = new List<CharacterModel>(characterGetTasks.Count);

            foreach(var characterId in characterIds)
            {
                characterGetTasks.Add(_characterRepository.GetCharacter(characterId));
            }

            try
            {
                var characterGetResults = await Task.WhenAll(characterGetTasks.ToArray());
                foreach(var characterGetResult in characterGetResults)
                {
                    try
                    {
                        var characterModel = BuildCharacterModel(characterGetResult);

                        characters.Add(characterModel);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _characterCacheService.HydrateCache(characters);
        }

        public async Task<JObject> GetCharacter(string characterName)
        {
            JObject character = null;

            if(IsValidCharacter(characterName))
            {
                if(!_characterCacheService.TryGetCharacter(characterName, out character))
                {
                    await HydrateCharacterCache();
                    _characterCacheService.TryGetCharacter(characterName, out character);
                }
            }

            return character;
        }

        public async Task<JObject> GetCharacters()
        {
            JObject characters;
            if(!_characterCacheService.TryGetCharacterList(out characters))
            {
                await HydrateCharacterCache();
                _characterCacheService.TryGetCharacterList(out characters);
            }

            return characters;
        }

        private bool IsValidCharacter(string characterName)
        {
            int characterId;
            return CharacterMap.TryGetCharacterIdByName(characterName, out characterId);
        }

        private CharacterModel BuildCharacterModel(JObject characterResponse)
        {
            JObject characterData = characterResponse["data"]["results"][0].Value<JObject>();

            return new CharacterModel {
                Id = characterData["id"].Value<int>(),
                Name = characterData["name"].Value<string>(),
                Description = characterData["description"].Value<string>(),
                Image = ParseImage(characterData),
                Comics = ParseComics(characterData)
            };
        }

        private string ParseImage(JObject characterData)
        {
            return characterData["thumbnail"]["path"].Value<string>() + "." + characterData["thumbnail"]["extension"].Value<string>();
        }

        private IList<string> ParseComics(JObject characterData)
        {
            var comics = characterData["comics"]["items"].Value<JArray>();

            var comicList = new List<string>(comics.Count);

            foreach(var jToken in comics)
            {
                var comic = (JObject) jToken;
                comicList.Add(comic["name"].Value<string>());
            }

            return comicList;
        }
    }
}