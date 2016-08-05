using System.Collections.Generic;
using Newtonsoft.Json;

namespace marvel_api.Characters
{
    public class CharacterModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("comics")]
        public IList<string> Comics { get; set; }
    }
}