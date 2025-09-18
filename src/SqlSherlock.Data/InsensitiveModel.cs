using System.Collections.Generic;
using System.Text.Json;

namespace SqlSherlock.Data
{
    public class InsensitiveModel
    {
        public Dictionary<string, object> Model { get; }

        public InsensitiveModel(Dictionary<string, JsonElement> userModel)
        {
            Model = [];
            foreach(var m in userModel)
            {
                Model.Add(m.Key.ToLower(), m.Value.ToValue());
            }
        }
    }
}
