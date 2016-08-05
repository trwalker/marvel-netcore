using System;
using System.Threading.Tasks;
using marvel_api.Config;
using marvel_api.Rest;

namespace marvel_api.Characters
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IHttpClientAdapter _httpClientAdapter;
        private readonly AuthConfigModel _authConfig;

        public CharacterRepository(IHttpClientAdapter httpClientAdapter, AuthConfigModel authConfig)
        {
            _httpClientAdapter = httpClientAdapter;
            _authConfig = authConfig;
        }

        public async Task<CharacterModel> GetCharacter(int characterId)
        {
            var timeStamp = "11111111111";
            var hash = "af542";

            var getUrl = $"http://gateway.marvel.com/v1/public/characters/{characterId}?ts={timeStamp}&apikey={_authConfig.PublicKey}&hash={hash}";

            var characterJson = await _httpClientAdapter.GetAsync(new Uri(getUrl));

            return new CharacterModel();
        }
    }
}