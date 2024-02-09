using System;
using System.Collections.Generic;
using System.Linq;

namespace KpblcNCadCfgIni.Data
{
    public class NCadConfiguration : IEquatable<NCadConfiguration>
    {
        public NCadConfiguration()
        {
            StartupApplicationList = new List<StartupApplication>();
        }
        public string ConfigurationName
        {
            get => _configurationName;
            set
            {
                _configurationName = value;
                ConfigurationNameForAppload = string.IsNullOrWhiteSpace(_configurationName)
                    ? "<<Default>>"
                    : _configurationName;
            }
        }

        public string ConfigurationNameForAppload { get; private set; }

        /// <summary> CfgFile </summary>
        public string CfgFileName { get; set; }

        /// <summary> PgpFile </summary>
        public string PgpFileName { get; set; }

        /// <summary> Plat </summary>
        public string Plat { get; set; }

        /// <summary> Перечень приложений автозагрузки </summary>
        public List<StartupApplication> StartupApplicationList { get; set; }

        internal List<string> PrepareToSave()
        {
            List<string> res = new List<string> {
              $"[{_configHeader}" +( string.IsNullOrWhiteSpace(ConfigurationName) ? "" : $@"\{ConfigurationName}") + "]",
              string.IsNullOrWhiteSpace(CfgFileName) ? string.Empty : $"CfgFile=s{CfgFileName}",
              string.IsNullOrWhiteSpace(PgpFileName) ? string.Empty : $"PgpFile=s{PgpFileName}",
              string.IsNullOrWhiteSpace(Plat) ? string.Empty : $"nPlat=s{Plat}",
          };

            if (StartupApplicationList.Any())
            {
                res.AddRange(new List<string>
              {
                  $@"[{_configHeader}\{ConfigurationNameForAppload}\Appload]",
                  $@"[{_configHeader}\{ConfigurationNameForAppload}\Appload\Startup]",
              });

                int index = 0;
                foreach (StartupApplication app in StartupApplicationList.OrderBy(o => o.LoadOrder))
                {
                    res.Add($@"[{_configHeader}\{ConfigurationNameForAppload}\Appload\Startup\app{index}]");
                    res.Add($"Loader=s{app.LoaderName}");
                    res.Add($"Type=s{app.AppType}");
                    res.Add($"Enabled=i" + (app.Enabled ? "1" : "0"));
                    index++;
                }
            }

            return res;
        }

        public bool Equals(NCadConfiguration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ConfigurationNameForAppload == other.ConfigurationNameForAppload;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NCadConfiguration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ConfigurationNameForAppload != null ? ConfigurationNameForAppload.GetHashCode() : 0) * 397);
            }
        }

        private string _configurationName;
        private readonly string _configHeader = @"\Configuration";
    }
}
