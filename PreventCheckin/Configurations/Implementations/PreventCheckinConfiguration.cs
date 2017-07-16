using System;
using System.IO;

using PreventCheckin.Configurations.Interfaces;

namespace PreventCheckin.Configurations.Implementations
{
    [Serializable]
    public class PreventCheckinConfiguration : IPreventCheckinConfiguration
    {
        private const string ConfigFileName = "preventcheckin.json";
        private const string ConfigFolderName = "preventcheckinextension";

        private string baseFolder
        {
            get
            {
                var result = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return result;
            }
        }

        public string FullFileConfigName => Path.Combine(this.FullFolderName, ConfigFileName);

        public string FullFolderName => Path.Combine(this.baseFolder, ConfigFolderName);
    }
}