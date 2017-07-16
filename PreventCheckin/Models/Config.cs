using Newtonsoft.Json;

namespace PreventCheckin.Models
{
    public class Config
    {
        public Config(string file)
        {
            this.File = file;
        }

        /// <summary>
        /// The relative file path to the input file.
        /// </summary>
        [JsonProperty("file")]
        public string File { get; }
    }
}