using Newtonsoft.Json;
using System.Collections.Generic;

namespace Snake
{
    public class ColorInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    internal class GameSettings
    {
        [JsonProperty("squaresPerSecond")]
        public int SquaresPerSecond { get; set; }

        [JsonProperty("rowsCount")]
        public int RowsCount { get; set; }

        [JsonProperty("colsCount")]
        public int ColsCount { get; set; }

        [JsonProperty("colors")]
        public List<ColorInfo> Colors { get; set; }
    }
}
