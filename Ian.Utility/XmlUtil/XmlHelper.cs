using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ian.Utility.XmlUtil
{
    public static class XmlHelper
    {
        public static object XmlElementToEntity<T>(this System.Xml.XmlNode node)
        {
            return Build(node, typeof(T));
        }

        private static object Build(System.Xml.XmlNode node, Type tp)
        {
            object instance = Activator.CreateInstance(tp);
            PropertyInfo[] properties = tp.GetProperties();
            foreach (PropertyInfo t in properties)
            {
                object[] attributes = t.GetCustomAttributes(typeof(ModelAttribute), true);

                if (attributes.Length == 0)
                    continue;

                ModelAttribute attribute = attributes[0] as ModelAttribute;
                object val = Build(node, attribute, t.PropertyType);
                try
                {
                    if (val != null && val.GetType() != t.PropertyType)
                        val = Convert.ChangeType(val, t.PropertyType);
                }
                catch { }
                t.SetValue(instance, val, null);
            }
            return instance;
        }

        private static object Build(System.Xml.XmlNode node, ModelAttribute attr, Type tp)
        {
            //获取集合
            if (attr.IsCollection)
            {
                Type gtp = tp.GetGenericArguments()[0];
                MethodInfo addMethod = tp.GetMethod("Add");
                object list = Activator.CreateInstance(tp);
                System.Xml.XmlNodeList nds = node.SelectNodes(attr.Name);
                for (int i = 0; i < nds.Count; i++)
                {
                    addMethod.Invoke(list, new object[] { Build(nds[i], gtp) });
                }
                return list;
            }

            //获取当前节点信息
            System.Xml.XmlNode nd = node.SelectSingleNode(attr.Name);
            if (nd == null)
                return null;

            if (attr.IsAttribute)
            {
                return nd.Attributes[attr.AttributeName].Value;
            }

            if (attr.IsLeaf)
                return nd.InnerText;

            if (nd.HasChildNodes)
            {
                return Build(nd, tp);
            }
            
            return null;
        }
    }
}
