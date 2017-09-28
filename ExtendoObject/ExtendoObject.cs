using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;

namespace ExtendoObject
{
    public class ExtendoObject : DynamicObject
    {
        private IDictionary<string, object> _instance;

        public ExtendoObject(object instance)
        {
            _instance = instance != null ? Mapper.Map<ExpandoObject>(instance) : new ExpandoObject();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object childObj;
            
            _instance.TryGetValue(binder.Name, out childObj);
            var t = childObj?.GetType() ?? typeof(object);

            if (t.IsClass && t != typeof (string))
                childObj = childObj as ExtendoObject ?? new ExtendoObject(childObj);

            _instance[binder.Name] = result = childObj;

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _instance[binder.Name] = value;
            return true;
        }
    }
}
