using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using PreventCheckin.Configurations.Interfaces;
using PreventCheckin.Models;
using PreventCheckin.Services.Interfaces;

namespace PreventCheckin.Services.Implementations
{
    [Serializable]
    public class PreventCheckinService : IPreventCheckinService
    {
        private readonly IPreventCheckinConfiguration iPreventCheckinConfiguration;

        public PreventCheckinService(IPreventCheckinConfiguration preventCheckinConfiguration)
        {
            if (preventCheckinConfiguration == null)
            {
                throw new ArgumentNullException(nameof(preventCheckinConfiguration));
            }

            this.iPreventCheckinConfiguration = preventCheckinConfiguration;
        }

        public void PreventCheckinStatus(IEnumerable<string> files)
        {
            this.ChangePreventCheckinStatus(files, true);
        }

        public void UnpreventCheckinStatus(IEnumerable<string> files)
        {
            this.ChangePreventCheckinStatus(files, false);
        }

        public bool AreExcluded(params string[] files)
        {
            var configs = this.GetConfigs();

            var result = files.All(x => configs.Contains(x));
            return result;
        }

        private Configs GetConfigs()
        {
            var fileName = this.iPreventCheckinConfiguration.FullFileConfigName;
            FileInfo file = new FileInfo(fileName);

            var configs = Enumerable.Empty<Config>();
            if (file.Exists)
            {
                string content = File.ReadAllText(fileName);
                configs = JsonConvert.DeserializeObject<IEnumerable<Config>>(content);
            }

            var result = new Configs(configs);
            return result;
        }

        private void SaveConfigFile(string content)
        {
            if (!Directory.Exists(this.iPreventCheckinConfiguration.FullFolderName))
            {
                Directory.CreateDirectory(this.iPreventCheckinConfiguration.FullFolderName);
            }

            var configFile = this.iPreventCheckinConfiguration.FullFileConfigName;
            File.WriteAllText(configFile, content, new UTF8Encoding(true));
        }

        private void ChangePreventCheckinStatus(IEnumerable<string> fileNames, bool include)
        {
            var configs = this.GetConfigs();

            if (include)
            {
                foreach (var fileName in fileNames)
                {
                    configs.Add(fileName);
                }
            }
            else
            {
                foreach (var fileName in fileNames)
                {
                    configs.Remove(fileName);
                }
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            };

            string content = JsonConvert.SerializeObject(configs.configs, settings);
            this.SaveConfigFile(content);
        }
    }
}