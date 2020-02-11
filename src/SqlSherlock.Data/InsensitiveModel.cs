using System.Collections.Generic;

namespace SqlSherlock.Data
{
    public class InsensitiveModel
    {
        public Dictionary<string, object> Model { get; }

        public InsensitiveModel(Dictionary<string, object> userModel)
        {
            Model = new Dictionary<string, object>();
            foreach(var m in userModel)
            {
                Model.Add(m.Key.ToLower(), m.Value);
            }
        }
    }
}
