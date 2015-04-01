using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ian.Utility.XmlUtil
{
    /// <summary>
    /// XML 节点模型特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ModelAttribute : Attribute
    {
        /// <summary>
        /// 标识当前节点名称
        /// <para>程序获取当前名称的 XmlNode 或者 XmlNodeList</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前值是否是属性
        /// </summary>
        public bool IsAttribute { get; set; }

        /// <summary>
        /// 保存值的属性名称
        /// <para>与 IsAttribute 结合使用</para>
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 标识当前节点是节点组
        /// </summary>
        public bool IsCollection { get; set; }

        /// <summary>
        /// 标识是否叶子节点，将直接取当前节点的innerText值
        /// </summary>
        public bool IsLeaf { get; set; }
    }
}
