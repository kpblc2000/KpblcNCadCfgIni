using KpblcNCadCfgIni.Data;
using KpblcNCadCfgIni.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KpblcNCadCfgIni
{
    public class NCadConfig
    {
        public NCadConfig(string ConfigIniFileName)
        {
            _iniFileName = ConfigIniFileName;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<string> data = File.ReadAllLines(_iniFileName, Encoding.GetEncoding(1251))
                .ToList()
                .Select(o =>
                {
                    string value = o.Trim('\t');
                    if (value.StartsWith("["))
                    {
                        return value.Trim(' ');
                    }
                    return value;

                })
                .ToList();

            ConfigurationList = new List<NCadConfiguration>(
                data.Where(o =>
                {
                    string name = o.Trim(new char[] { '\\', '[', ']' });
                    if (!name.ToUpper().StartsWith(_configurationHeader.ToUpper()))
                    {
                        return false;
                    }

                    var splitted = name.Split(new char[] { '\\' });
                    return splitted.Length < 3;
                }
                    )
                    .Select(o => new NCadConfiguration()
                    {
                        ConfigurationName = o.Trim(new char[] { '\\', '[', ']', ' ' }).Substring(_configurationHeader.Length).Trim('\\'),
                    }
                    )
                    .Distinct()
            );

            int pos = 0;

            while (pos < data.Count)
            {
                if (data[pos].Equals("[\\]"))
                {
                    UnnamedSection = new Section()
                    {
                        Name = "\\",
                    };
                    while (!data[pos + 1].StartsWith("["))
                    {
                        GetKeyValueFromString(data[pos + 1], out string key, out string value);
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            UnnamedSection.Data.Add(new KeyValue(key, value));
                        }
                        pos++;
                    }

                    pos--;
                }
                else if (GetSectionName(data[pos]).Equals("Inccfgdirs", StringComparison.InvariantCultureIgnoreCase))
                {
                    IncludeConfigurationFolders = new Section()
                    {
                        Name = GetSectionName(data[pos]),
                    };
                    while (!data[pos + 1].StartsWith("["))
                    {
                        GetKeyValueFromString(data[pos + 1], out string key, out string value);
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            IncludeConfigurationFolders.Data.Add(new KeyValue(key, value));
                        }
                        pos++;
                    }

                    pos--;
                }
                else if (GetSectionName(data[pos]).ToUpper().StartsWith(_configurationHeader.ToUpper()))
                {
                    string confName = GetConfigurationName(data[pos]);

                    if (confName.Contains('\\'))
                    {
                        string[] splitted = confName.Split('\\');
                        confName = splitted[0];
                    }

                    NCadConfiguration configuration = ConfigurationList.First(o =>
                        o.ConfigurationName.Equals(confName) || o.ConfigurationNameForAppload.Equals(confName));

                    if (!data[pos].ToUpper().Contains("APPLOAD"))
                    {
                        string key, value;
                        while (!data[pos + 1].StartsWith("["))
                        {
                            GetKeyValueFromString(data[pos + 1], out key, out value);
                            if (key.Equals("cfgfile", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.CfgFileName = value.Substring(1);
                            }
                            else if (key.Equals("pgpfile", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.PgpFileName = value.Substring(1);
                            }
                            else if (key.Equals("nplat", StringComparison.InvariantCultureIgnoreCase))
                            {
                                configuration.Plat = value.Substring(1);
                            }

                            pos++;
                        }
                    }
                    else if (data[pos].ToUpper().Contains("APPLOAD") &&
                             Regex.IsMatch(data[pos], @"^(.*)app\d+\](.*)$", RegexOptions.CultureInvariant))
                    {
                        string sOrder = data[pos].Split('\\').Last().Trim('\\', 'a', 'p', ']');
                        int.TryParse(sOrder, out int order);

                        StartupApplication app = new StartupApplication()
                        {
                            LoadOrder = order,
                        };

                        string key, value;
                        while (pos < data.Count - 1 && !data[pos + 1].StartsWith("["))
                        {
                            GetKeyValueFromString(data[pos + 1], out key, out value);
                            if (key.Equals("Loader", StringComparison.InvariantCultureIgnoreCase))
                            {
                                app.LoaderName = value.Substring(1);
                            }
                            else if (key.Equals("Type", StringComparison.InvariantCultureIgnoreCase))
                            {
                                AppTypeEnum appType = AppTypeEnum.Unknown;
                                AppTypeEnum.TryParse(value.Substring(1), out appType);
                                app.AppType = appType;
                            }
                            else if (key.Equals("Enabled", StringComparison.InvariantCultureIgnoreCase))
                            {
                                app.Enabled = value.Substring(1) == "1";
                            }
                            pos++;
                        }

                        configuration.StartupApplicationList.Add(app);
                    }
                }

                pos++;
            }
        }

        /// <summary> Сохранение в файл </summary>
        public void Save()
        {
            List<string> saveList = new List<string>();
            saveList.AddRange(UnnamedSection.PrepareToSave());
            saveList.AddRange(IncludeConfigurationFolders.PrepareToSave());
            foreach (NCadConfiguration item in ConfigurationList)
            {
                saveList.AddRange(item.PrepareToSave().ToList());
            }

            File.WriteAllLines(_iniFileName, saveList.ToArray(), Encoding.GetEncoding(1251));
        }

        public Section UnnamedSection { get; private set; }
        public Section IncludeConfigurationFolders { get; private set; }

        public List<NCadConfiguration> ConfigurationList { get; private set; }

        private string GetSectionName(string Name)
        {
            return Name.Trim('\t', '[', ']', '\\');
        }

        private string GetConfigurationName(string Name)
        {
            return Name.Trim('\\', '[', ']', '\t').Substring(_configurationHeader.Length).Trim('\\');
        }

        private void GetKeyValueFromString(string Line, out string Key, out string Value)
        {
            if (!Line.Contains(_separator))
            {
                Key = string.Empty;
                Value = string.Empty;
                return;
            }

            int index = Line.IndexOf(_separator);
            Key = Line.Substring(0, index);
            Value = Line.Substring(index + 1);
        }

        private string _iniFileName;
        private readonly string _separator = "=";
        private readonly string _configurationHeader = "Configuration";
    }
}
