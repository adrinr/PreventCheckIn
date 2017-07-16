using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PreventCheckin.Models
{
    public class Configs
    {
        public IList<Config> configs { get; }

        public Configs(IEnumerable<Config> configs)
        {
            this.configs = configs.ToList();
        }

        public void Add(string fileName)
        {
            if (!this.Contains(fileName))
            {
                this.configs.Add(new Config(fileName));
            }
        }

        public void Remove(string fileName)
        {
            var configsToRemove = this.configs.Where(this.SameFilePredicate(fileName)).ToList();
            foreach (var config in configsToRemove)
            {
                this.configs.Remove(config);
            }
        }

        public bool Contains(string fileName)
        {
            var result = this.configs.Any(this.SameFilePredicate(fileName));
            return result;
        }
        private Func<Config, bool> SameFilePredicate(string fileName)
        {
            Func<Config, bool> result = x => Path.GetFullPath(x.File).Equals(Path.GetFullPath(fileName));
            return result;
        }
    }
}