using System.Reflection;

namespace Vp.RestClient.Models
{
    public class Parameter
    {
        public Parameter(ParameterInfo parameterInfo, object value)
        {
            Name = parameterInfo.Name;
            Value = value;
            ParameterInfo = parameterInfo;
        }
        
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        
        public string Name { get;  }
        public object Value { get;  }
        public ParameterInfo ParameterInfo { get;  }
    }
}