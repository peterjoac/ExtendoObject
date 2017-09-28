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

        public static object MapFrom(IDictionary<string, object> o, string path)
        {
            var tokens = path.Split('.');
            foreach (var t in tokens.Take(tokens.Length - 1))
            {
                var m = Regex.Match(t, @"\[(\d+)\]");

                if (m.Success)
                {
                    var listName = t.Substring(0, m.Index);
                    var idx = int.Parse(m.Groups[1].Value);
                    object childObj;
                    o.TryGetValue(listName, out childObj);

                    var list = childObj as System.Collections.IList;
                    o = list[idx] as IDictionary<string, object>;
                }
                else
                {
                    o = o[t] as IDictionary<string, object>;
                }
            }

            return o[tokens.Last()];
        }

        public static IDictionary<string, object> MapTo(IDictionary<string, object> o, string path, string value)
        {
            var tokens = path.Split('.');

            foreach (var t in tokens.Take(tokens.Length - 1))
            {
                var m = Regex.Match(t, @"\[(\d+)\]");

                if (m.Success)
                {
                    var listName = t.Substring(0, m.Index);
                    var idx = int.Parse(m.Groups[1].Value);

                    object childObj;
                    o.TryGetValue(listName, out childObj);

                    var list = childObj as IList<ExpandoObject>;

                    if (list == null)
                    {
                        list = new List<ExpandoObject>();
                        o.Add(listName, list);
                    }

                    while (list.Count <= idx)
                        list.Add(null);

                    var listElement = list[idx] as ExpandoObject;

                    if (listElement == null)
                    {
                        listElement = new ExpandoObject();
                        list[idx] = listElement;
                    }

                    o = listElement;
                }
                else
                {
                    object childObj;
                    o.TryGetValue(t, out childObj);

                    if (childObj == null)
                    {
                        childObj = new ExpandoObject();
                        o.Add(t, childObj);
                    }

                    o = childObj as IDictionary<string, object>;
                }
            }

            o.Add(tokens.Last(), value);

            return o;
        }
    }
}
