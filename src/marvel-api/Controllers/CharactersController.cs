using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace marvel_api.Controllers
{
    [Route("v1/[controller]")]
    public class CharactersController : Controller
    {
        [HttpGet]
        public JObject Get()
        {
            var characters = new JObject(
                new JProperty("characters", new JArray(
                    new JObject(
                        new JProperty("name", "spider-man")
                    ),
                    new JObject(
                        new JProperty("name", "hulk")
                    )
                ))
            );

            return characters;
        }

        [HttpGet("{name}")]
        public JObject Get(string name)
        {
            var character = new JObject(
                new JProperty("name", name)
            );

            return character;
        }
    }
}
