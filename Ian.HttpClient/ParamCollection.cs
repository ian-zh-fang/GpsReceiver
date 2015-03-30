using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace Ian.HttpClient
{
    public class ParamCollection : NameValueCollection
    {
        protected readonly string _separator = "&"; 

        // 摘要: 
        //     初始化 System.Collections.Specialized.NameValueCollection 类的新实例，该实例为空且具有默认初始容量，并使用不区分大小写的默认哈希代码提供程序和不区分大小写的默认比较器。
        public ParamCollection():base(){}

        //
        // 摘要: 
        //     初始化 System.Collections.Specialized.NameValueCollection 类的新实例，该实例为空、具有默认的初始容量并使用指定的
        //     System.Collections.IEqualityComparer 对象。
        //
        // 参数: 
        //   equalityComparer:
        //     System.Collections.IEqualityComparer 对象，用于确定两个键是否相等，并为集合中的键生成哈希代码。
        public ParamCollection(IEqualityComparer equalityComparer):base(equalityComparer)
        {

        }

        //
        // 摘要: 
        //     初始化 System.Collections.Specialized.NameValueCollection 类的新实例，该实例为空且具有指定的初始容量，并使用不区分大小写的默认哈希代码提供程序和不区分大小写的默认比较器。
        //
        // 参数: 
        //   capacity:
        //     System.Collections.Specialized.NameValueCollection 可包含的初始项数。
        //
        // 异常: 
        //   System.ArgumentOutOfRangeException:
        //     capacity 小于零。
        public ParamCollection(int capacity):base(capacity)
        {

        }

        //
        // 摘要: 
        //     将项从指定的 System.Collections.Specialized.NameValueCollection 复制到一个新的 System.Collections.Specialized.NameValueCollection，这个新集合的初始容量与复制的项数相等，并使用与源集合相同的哈希代码提供程序和比较器。
        //
        // 参数: 
        //   col:
        //     要复制到新 System.Collections.Specialized.NameValueCollection 实例的 System.Collections.Specialized.NameValueCollection。
        //
        // 异常: 
        //   System.ArgumentNullException:
        //     col 为 null。
        public ParamCollection(NameValueCollection col):base(col)
        {

        }

        //
        // 摘要: 
        //     初始化 System.Collections.Specialized.NameValueCollection 类的新实例，该实例为空、具有指定的初始容量并使用指定的
        //     System.Collections.IEqualityComparer 对象。
        //
        // 参数: 
        //   capacity:
        //     System.Collections.Specialized.NameValueCollection 对象可包含的初始项数。
        //
        //   equalityComparer:
        //     System.Collections.IEqualityComparer 对象，用于确定两个键是否相等，并为集合中的键生成哈希代码。
        //
        // 异常: 
        //   System.ArgumentOutOfRangeException:
        //     capacity 小于零。
        public ParamCollection(int capacity, IEqualityComparer equalityComparer):base(capacity, equalityComparer)
        {

        }
        
        //
        // 摘要: 
        //     将项从指定的 System.Collections.Specialized.NameValueCollection 复制到一个新的 System.Collections.Specialized.NameValueCollection，这个新集合使用指定的初始容量或与具有与复制的项数相等的初始容量（两者中较大的一个），并使用不区分大小写的默认哈希代码提供程序和不区分大小写的默认比较器。
        //
        // 参数: 
        //   capacity:
        //     System.Collections.Specialized.NameValueCollection 可包含的初始项数。
        //
        //   col:
        //     要复制到新 System.Collections.Specialized.NameValueCollection 实例的 System.Collections.Specialized.NameValueCollection。
        //
        // 异常: 
        //   System.ArgumentOutOfRangeException:
        //     capacity 小于零。
        //
        //   System.ArgumentNullException:
        //     col 为 null。
        public ParamCollection(int capacity, NameValueCollection col):base(capacity, col)
        {

        }

        public int Count
        {
            get { return Keys.Count; }
        }

        public override string ToString()
        {
            int count = Count;
            string[] buffer = new string[count];
            for (int offset = 0; offset < count; offset++)
            {
                buffer[offset] = string.Format("{0}={1}", Keys[offset], this[offset]);
            }

            return string.Join(_separator, buffer);
        }
    }
}
