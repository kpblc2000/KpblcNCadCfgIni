using System;

namespace KpblcNCadCfgIni.Data
{
    public class KeyValue
    {
        public KeyValue(string Key, string Value)
        {
            this.Key = Key;
            if (Value.StartsWith("s"))
            {
                this.Value = Value.Substring(1);
            }
            else if (Value.StartsWith("i") && Key.Equals("Enabled", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Value = Value == "i1";
            }
            else if (Value.StartsWith("i"))
            {
                this.Value = int.Parse(Value.Substring(1));
            }
            else
            {
                this.Value = Value;
            }
        }

        public string Key { get; private set; }
        public object Value { get; set; }

        public override string ToString()
        {
            string value = string.Empty;

            if (Value is string)
            {
                value = $"s{Value}";
            }
            else if (Value is int)
            {
                value = $"i{Value}";
            }
            else if (Value is bool bValue)
            {
                value = "i" + (bValue ? "1" : "0");
            }

            return Key + "=" + value;
        }
    }

}
