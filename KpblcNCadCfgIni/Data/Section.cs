using System.Collections.Generic;
using System.Linq;

namespace KpblcNCadCfgIni.Data
{
    internal class Section
    {
        public Section()
        {
            Data = new List<KeyValue>();
        }
        public string Name { get; set; }
        public List<KeyValue> Data { get; set; }

        internal List<string> PrepareToSave()
        {
            List<string> res = new List<string>
            {
                $"[{Name}]"
            };

            if (Data.Any())
            {
                res.AddRange(Data.Select(o => o.ToString()));
            }

            return res;
        }
    }
}
