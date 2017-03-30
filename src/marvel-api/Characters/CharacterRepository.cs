using System;
using System.Diagnostics;
using System.Threading.Tasks;
using marvel_api.Auth;
using marvel_api.Rest;
using Newtonsoft.Json.Linq;

namespace marvel_api.Characters
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IHttpClientAdapter _httpClientAdapter;
        private readonly ICredentialsService _credentialsService;

        public CharacterRepository(ICredentialsService credentialsService, IHttpClientAdapter httpClientAdapter)
        {
            _httpClientAdapter = httpClientAdapter;
            _credentialsService = credentialsService;
        }

        public async Task<JObject> GetCharacter(int characterId)
        {
            var credentials = _credentialsService.GenerateCredentials();

            var getUrl = $"http://gateway.marvel.com/v1/public/characters/{characterId}?ts={credentials.TimeStamp}&apikey={credentials.PublicKey}&hash={credentials.Hash}";

            Console.WriteLine(getUrl);

            return await _httpClientAdapter.GetAsync(new Uri(getUrl));
        }
    }
}