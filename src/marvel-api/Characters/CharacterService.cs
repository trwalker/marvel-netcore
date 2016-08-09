using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public class CharacterService : ICharacterService
    {
        private ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<CharacterModel> GetCharacter(string characterName)
        {
            var characterResponse = await _characterRepository.GetCharacter(1009610);

            return BuildCharacterModel(characterResponse);
        }

        public async Task<IList<CharacterModel>> GetCharacters()
        {
            throw new NotImplementedException();
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
            return characterData["thumbnail"]["path"].Value<string>() + "." + (string)characterData["thumbnail"]["extension"].Value<string>();
        }

        private IList<string> ParseComics(JObject characterData)
        {
            var comics = characterData["comics"]["items"].Value<JArray>();

            var comicList = new List<string>(comics.Count);

            foreach(JObject comic in comics)
            {
                comicList.Add(comic["name"].Value<string>());
            }

            return comicList;
        }
    }
}