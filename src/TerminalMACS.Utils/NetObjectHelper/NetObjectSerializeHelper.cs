using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalMACS.Utils.NetObjectHelper
{
    /// <summary>
    /// 网络对象序列化接口
    /// </summary>
    public static class NetObjectSerializeHelper
    {
        /// <summary>
        /// 将byte[]反序列化为指定对象(T)
        /// </summary>
        /// <param name="buf">byte[]</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(byte[] buf) where T : class, new()
        {
            NetObjectAttribute objNameInfo = null;
            List<NetObjectPropertyAttribute> lstFileds = null;
            GetAttributes<T>(default(T), typeof(T), false, ref objNameInfo, ref lstFileds);

            ArrayHelper helper = new ArrayHelper(NetBase.CommonEncoding, buf);

            //头
            int dds_magic = helper.DequeueInt32();                 //消息标识
            string objName = helper.DequeueStringWithoutEndChar(NetBase.CommonEncoding); //对象名称
            int ver = helper.DequeueInt32();                       //对象版本号

            T instance = new T();

            //反序列化操作
            foreach (var item in lstFileds)
            {
                object val = Deserialize(helper, item.Type, item.CurrentEncoding);
                if (item.Obj.GetType().Name.EndsWith(typeof(PropertyInfo).Name))
                    (item.Obj as PropertyInfo).SetValue(instance, val, null);
                else
                    (item.Obj as FieldInfo).SetValue(instance, val);
            }

            return instance;
        }

        //反序列化对象
        private static object Deserialize(ArrayHelper helper, Type childType, Encoding encoding)
        {
            object val = default(object);

            //列表、字典
            if (childType.Name.Equals(typeof(List<>).Name)
                || childType.Name.Equals(typeof(ObservableRangeCollection<>).Name)
                   || childType.Name.Equals(typeof(Dictionary<,>).Name))
                val = Deserialize2(helper, childType);

            //基本数据类型赋值
            else if (childType.IsPrimitive
                || childType.Name.Equals(typeof(string).Name)
                || childType.Name.Equals(typeof(byte[]).Name))
                val = GetFieldValue(helper, childType, encoding);

            //引用类型赋值
            else
            {
                NetObjectAttribute objNameInfo = null;
                List<NetObjectPropertyAttribute> lstFileds = null;
                val = childType.Assembly.CreateInstance(childType.FullName);
                GetAttributes(val, childType, false, ref objNameInfo, ref lstFileds);
                foreach (var item in lstFileds)
                {
                    object val2 = Deserialize(helper, item.Type, item.CurrentEncoding);
                    if (item.Obj.GetType().Name.EndsWith(typeof(PropertyInfo).Name))
                        (item.Obj as PropertyInfo).SetValue(val, val2, null);
                    else
                        (item.Obj as FieldInfo).SetValue(val, val2);
                }
            }

            return val;
        }

        //反序列化列表(List<>)或者字典Dictionary<,>数据
        private static object Deserialize2(ArrayHelper helper, Type childType)
        {
            object obj = childType.Assembly.CreateInstance(childType.FullName);//集合实例

            //长度
            int count = helper.DequeueInt32();
            MethodInfo addMethod = childType.GetMethod("Add");
            object val1 = default(object);
            object val2 = default(object);

            for (int i = 0; i < count; i++)
            {
                val1 = GetObject(helper, childType.GetGenericArguments()[0], NetBase.CommonEncoding);
                if (childType.GetGenericArguments().Length > 1)
                {
                    val2 = GetObject(helper, childType.GetGenericArguments()[1], NetBase.CommonEncoding);
                    addMethod.Invoke(obj, new object[] { val1, val2 });
                }
                else
                    addMethod.Invoke(obj, new object[] { val1 });
            }

            return obj;
        }

        //反序列化指定类型
        private static object GetObject(ArrayHelper helper, Type childType, Encoding encoding)
        {
            object val;

            //基本数据类型
            if (childType.IsPrimitive
                || childType.Equals(typeof(string))
                || childType.Equals(typeof(byte[])))
                val = GetFieldValue(helper, childType, encoding);

            //集合数据类型
            else if (childType.Name.Equals(typeof(List<>).Name)
                || childType.Name.Equals(typeof(ObservableRangeCollection<>).Name)
                || childType.Name.Equals(typeof(Dictionary<,>).Name))
            {
                val = Deserialize2(helper, childType);
            }

            //其他引用类型
            else
            {
                NetObjectAttribute objNameInfo = null;
                List<NetObjectPropertyAttribute> lstFileds = null;
                val = childType.Assembly.CreateInstance(childType.FullName);
                GetAttributes(val, childType, false, ref objNameInfo, ref lstFileds);
                foreach (var item in lstFileds)
                {
                    object val2 = Deserialize(helper, item.Type, item.CurrentEncoding);
                    if (item.Obj.GetType().Name.EndsWith(typeof(PropertyInfo).Name))
                        (item.Obj as PropertyInfo).SetValue(val, val2, null);
                    else
                        (item.Obj as FieldInfo).SetValue(val, val2);
                }
            }

            return val;
        }

        /// <summary>
        /// 将指定对象序列化为byte[]
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="needHead">是否需要序列化头部信息(消息标识、对象名称、对象版本号等数据)</param>
        /// <returns>byte[]</returns>
        public static byte[] Serialize<T>(T obj)
        {
            NetObjectAttribute objNameInfo = null;
            List<NetObjectPropertyAttribute> lstFileds = null;

            GetAttributes<T>(obj, typeof(T), true, ref objNameInfo, ref lstFileds);

            ArrayHelper helper = new ArrayHelper(NetBase.CommonEncoding);

            //头
            helper.Enqueue(NetBase.DDS_MAGIC);              //消息标识
            helper.EnqueueWithoutEndChar(objNameInfo.Name, NetBase.CommonEncoding); //对象名称
            helper.Enqueue(objNameInfo.Version);            //对象版本号

            //类属性
            Serialize(helper, lstFileds);

            return helper.Arrays;
        }

        //序列化属性
        private static void Serialize(ArrayHelper helper, List<NetObjectPropertyAttribute> lstFileds)
        {
            //数据
            foreach (var item in lstFileds)
            {
                //序列化基本数据类型
                if (item.Type.IsPrimitive
                    || item.Type.Equals(typeof(string))
                    || item.Type.Equals(typeof(byte[])))
                    SetFieldValue(helper, item.Type, item.Value, item.CurrentEncoding);

                //列表序列化List<T>,Dictionary<T,T1>
                else if (item.Type.Name.Equals(typeof(List<>).Name)
                    || item.Type.Name.Equals(typeof(ObservableRangeCollection<>).Name)
                    || item.Type.Name.Equals(typeof(Dictionary<,>).Name))
                    Serialize(helper, item.Type, item.Value);

                //普通对象序列化
                else
                {
                    NetObjectAttribute objNameInfo2 = null;
                    List<NetObjectPropertyAttribute> lstFileds2 = null;

                    GetAttributes<object>(item.Value, item.Type, true, ref objNameInfo2, ref lstFileds2);
                    Serialize(helper, lstFileds2);
                }
            }
        }

        //序列化列表数据项
        private static void Serialize(ArrayHelper helper, Type type, object val)
        {
            int count = default(int);

            dynamic col = val;
            if (val != null)
                count = col.Count;        //获取数据项个数
            else
                count = 0;
            helper.Enqueue(count);
            if (val == null) return;

            //列表数据类型序列化
            if (type.Name.Equals(typeof(List<>).Name)
                || type.Name.Equals(typeof(ObservableRangeCollection<>).Name))
                foreach (var item in col)
                    Serialize2(helper, type.GetGenericArguments()[0], item, NetBase.CommonEncoding);

            //字典数据类型序列化
            else
            {
                //遍历字典
                foreach (var kvp in col)
                {
                    //序列化键
                    Serialize2(helper, type.GetGenericArguments()[0], kvp.Key, NetBase.CommonEncoding);

                    //序列化值
                    Serialize2(helper, type.GetGenericArguments()[1], kvp.Value, NetBase.CommonEncoding);
                }
            }
        }

        //序列化
        private static void Serialize2(ArrayHelper helper, Type type, object objCh, Encoding encoding)
        {
            //列表中存放基本数据类型
            if (type.IsPrimitive
                || type.Equals(typeof(string))
                 || type.Equals(typeof(byte[])))
                SetFieldValue(helper, type, objCh, encoding);

            //列表序列化List<T>,Dictionary<T,T1>,列表中再嵌入列表
            else if (type.Name.Equals(typeof(List<>).Name)
                || type.Name.Equals(typeof(ObservableRangeCollection<>).Name)
                || type.Name.Equals(typeof(Dictionary<,>).Name))
                Serialize(helper, type, objCh);

            //列表中存放引用类型
            else
            {
                NetObjectAttribute objNameInfo = null;
                List<NetObjectPropertyAttribute> lstFileds = null;
                GetAttributes<object>(objCh, type, true, ref objNameInfo, ref lstFileds);
                Serialize(helper, lstFileds);
            }
        }

        /// <summary>
        /// 获取指定对象的指定特性
        /// </summary>
        /// <typeparam name="T1">对象类型</typeparam>
        /// <typeparam name="T2">特性</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>特性实例</returns>
        public static T2 GetAttribute<T1, T2>(T1 obj) where T2 : class
        {
            object[] objs = null;

            if (obj == null || !(obj is MemberInfo))
            {
                Type t = default(Type);
                if (obj == null)
                    t = typeof(T1);
                else
                    t = obj.GetType();
                objs = t.GetCustomAttributes(typeof(T2), false);
            }
            else
                objs = (obj as MemberInfo).GetCustomAttributes(typeof(T2), false);

            if (objs.Length > 0)
                return objs[0] as T2;

            return default(T2);
        }

        /// <summary>
        /// 获取对象的序列化特性数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例，当为空时，只保存特性对应的属性或者字段引用，否则获取对应属性或者字段的值</param>
        /// <param name="isSerialize">使用方式，true:序列化使用，false:反序列化使用</param>
        /// <param name="objNameInfo">对象信息</param>
        /// <param name="lstFileds">对象字段信息(包括属性信息)</param>
        public static void GetAttributes<T>(T obj, Type objType, bool isSerialize, ref NetObjectAttribute objNameInfo, ref List<NetObjectPropertyAttribute> lstFileds)
        {

            //取得对象名称及版本号
            objNameInfo = GetAttribute<T, NetObjectAttribute>(obj);

            lstFileds = new List<NetObjectPropertyAttribute>();

            //取得需要序列化的属性列表
            PropertyInfo[] pis = objType.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                NetObjectPropertyAttribute soa = GetAttribute<PropertyInfo, NetObjectPropertyAttribute>(pi);
                if (soa == null)
                    continue;

                soa.Type = pi.PropertyType;
                if (isSerialize)
                    soa.Value = pi.GetValue(obj, null);
                else
                    soa.Obj = pi;

                lstFileds.Add(soa);
            }

            //取得需要序列化的字段列表
            FieldInfo[] fis = objType.GetFields();
            foreach (FieldInfo fi in fis)
            {
                NetObjectPropertyAttribute soa = GetAttribute<FieldInfo, NetObjectPropertyAttribute>(fi);
                if (soa == null)
                    continue;

                soa.Type = fi.FieldType;
                if (isSerialize)
                    soa.Value = fi.GetValue(obj);
                else
                    soa.Obj = fi;

                lstFileds.Add(soa);
            }

            //排序
            lstFileds.Sort((oA, oB) => oA.ID.CompareTo(oB.ID));
        }

        /// <summary>
        /// 获取基本数据类型值
        /// </summary>
        /// <param name="att">特性</param>
        /// <param name="helper">字节帮助对象</param>
        private static object GetFieldValue(ArrayHelper helper, Type type, Encoding encoding)
        {
            object val = default(object);

            if (typeof(string).Equals(type))           //string
                val = helper.DequeueStringWithoutEndChar(encoding);
            else if (typeof(UInt16).Equals(type))      //UInt16
                val = helper.DequeueUInt16();
            else if (typeof(Int16).Equals(type))       //Int16
                val = helper.DequeueInt16();
            else if (typeof(UInt32).Equals(type))      //UInt32
                val = helper.DequeueUInt32();
            else if (typeof(Int32).Equals(type))       //Int32
                val = helper.DequeueInt32();
            else if (typeof(UInt64).Equals(type))      //UInt64
                val = helper.DequeueULong();
            else if (typeof(Int64).Equals(type))       //Int64
                val = helper.DequeueLong();
            else if (typeof(double).Equals(type))      //double
                val = helper.DequeueDouble();
            else if (typeof(byte).Equals(type))        //byte
                val = helper.DequeueByte();
            else if (typeof(byte[]).Equals(type))      //byte[]
                val = helper.DequeueBytes();
            else if (typeof(bool).Equals(type))        //bool
                val = helper.DequeueBoolean();
            else if (typeof(char).Equals(type))        //bool
                val = helper.DequeueChar();

            return val;
        }


        /// <summary>
        /// 序列化属性值
        /// </summary>
        /// <param name="helper">字节帮助类</param>
        /// <param name="type">属性类型</param>
        /// <param name="val">属性值</param>
        private static void SetFieldValue(ArrayHelper helper, Type type, object val, Encoding encoding)
        {
            if (typeof(string).Equals(type))           //string
                helper.EnqueueWithoutEndChar(val == null ? string.Empty : val.ToString(), encoding);
            else if (typeof(UInt16).Equals(type))      //UInt16
                helper.Enqueue(val == null ? default(UInt16) : UInt16.Parse(val.ToString()));
            else if (typeof(Int16).Equals(type))       //Int16
                helper.Enqueue(val == null ? default(Int16) : Int16.Parse(val.ToString()));
            else if (typeof(UInt32).Equals(type))      //UInt32
                helper.Enqueue(val == null ? default(UInt32) : UInt32.Parse(val.ToString()));
            else if (typeof(Int32).Equals(type))       //Int32
                helper.Enqueue(val == null ? default(Int32) : Int32.Parse(val.ToString()));
            else if (typeof(UInt64).Equals(type))      //UInt64
                helper.Enqueue(val == null ? default(UInt64) : UInt64.Parse(val.ToString()));
            else if (typeof(Int64).Equals(type))       //Int64
                helper.Enqueue(val == null ? default(Int64) : Int64.Parse(val.ToString()));
            else if (typeof(double).Equals(type))      //double
                helper.Enqueue(val == null ? default(double) : double.Parse(val.ToString()));
            else if (typeof(byte).Equals(type))        //byte
                helper.Enqueue(val == null ? default(byte) : byte.Parse(val.ToString()));
            else if (typeof(byte[]).Equals(type))      //byte[]
                helper.Enqueue(val == null ? default(byte[]) : (byte[])val);
            else if (typeof(bool).Equals(type))        //bool
                helper.Enqueue(val == null ? default(bool) : bool.Parse(val.ToString()));
            else if (typeof(char).Equals(type))        //bool
                helper.Enqueue(val == null ? default(char) : (char)val);
        }

        /// <summary>
        /// 获取对象基本信息
        /// </summary>
        /// <param name="buf">序列化数据</param>
        /// <param name="obj">数据标识</param>
        public static void GetNetObjBase(byte[] buf, out NetObject obj)
        {
            obj = new NetObject();

            ArrayHelper arr = new ArrayHelper(NetBase.CommonEncoding, buf);
            try
            {
                obj.Mark = arr.DequeueInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("解析对象标识异常: {0}", ex.ToString()), ex);
            }
            try
            {
                obj.Name = arr.DequeueStringWithoutEndChar(NetBase.CommonEncoding);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("解析对象名称异常，标识为: {0}, 详细异常: {1}", obj.Mark, ex.ToString()), ex);
            }
            try
            {
                obj.Version = arr.DequeueInt32();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("解析对象版本异常，标识为: {0}, 名称为: {1}, 详细异常: {2}", obj.Mark, obj.Name, ex.ToString()), ex);
            }
            try
            {
                obj.TaskID = arr.DequeueInt32();
            }
            catch
            {
            }
            obj.Datas = buf;
        }

        /// <summary>
        /// 复制对象数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="from">复制源对象</param>
        /// <param name="to">复制目标对象</param>
        public static void CopyTo<T>(T from, T to)
        {
            PropertyInfo[] pis = typeof(T).GetProperties();
            foreach (var pi in pis)
            {
                try
                {
                    object va = pi.GetValue(from, null);
                    pi.SetValue(to, va, null);
                }
                catch { }
            }
        }


        /// <summary>
        /// 字符串列表转换为以“,”分隔的字符串
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public static string ListToString(List<string> listData)
        {
            if (listData == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            listData.ForEach(cu =>
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append(cu);
            });
            return sb.ToString();
        }

        /// <summary>
        /// get the encode of the str
        /// </summary>
        /// <param name="str">judge string</param>
        /// <returns>encode</returns>
        public static Encoding GetStringEncoding(string str)
        {
            byte[] bts = Encoding.Default.GetBytes(str);
            string tmpStr = Encoding.Default.GetString(bts);
            if (tmpStr == str)
            {
                return Encoding.Default;
            }
            else
            {
                return Encoding.Unicode;
            }
        }

        /// <summary>
        /// convert the unknown encode string to utf8 string
        /// </summary>
        /// <param name="unknowEncodeStr"></param>
        /// <returns></returns>
        public static string GetUTF8String(this string unknowEncodeStr)
        {
            string utf8Str;
            Encoding unknowEncode = GetStringEncoding(unknowEncodeStr);
            if (unknowEncode == Encoding.Default)
            {
                utf8Str = DefaultStringToUTF8(unknowEncodeStr);
            }
            else
            {
                utf8Str = UnicodeStringToUTF8(unknowEncodeStr);
            }

            return utf8Str;
        }

        /// <summary>
        /// utf8 string convert to default string
        /// </summary>
        /// <param name="utf8String"></param>
        /// <returns></returns>
        public static string UTF8StringToDefault(this string utf8String)
        {
            Encoding utf8Code = Encoding.UTF8;
            Encoding defaultCode = Encoding.Default;

            byte[] utf8Bytes = utf8Code.GetBytes(utf8String);
            byte[] defaultBytes = Encoding.Convert(utf8Code, defaultCode, utf8Bytes);
            string defaultString = defaultCode.GetString(defaultBytes, 0, defaultBytes.Length);

            return defaultString;
        }


        /// <summary>
        /// default string convert to utf8 string
        /// </summary>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string DefaultStringToUTF8(this string defaultString)
        {
            Encoding utf8Code = Encoding.UTF8;
            Encoding defaultCode = Encoding.Default;

            byte[] defaultBytes = defaultCode.GetBytes(defaultString);
            byte[] utf8Bytes = Encoding.Convert(defaultCode, utf8Code, defaultBytes);
            string utf8String = utf8Code.GetString(utf8Bytes, 0, utf8Bytes.Length);

            return utf8String;
        }



        /// <summary>
        /// unicode string convert to utf8 string
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string UnicodeStringToUTF8(this string unicodeString)
        {
            Encoding utf8Code = Encoding.UTF8;
            Encoding unicodeCode = Encoding.Unicode;

            byte[] unicodeBytes = unicodeCode.GetBytes(unicodeString);
            byte[] utf8Bytes = Encoding.Convert(unicodeCode, utf8Code, unicodeBytes);
            string utf8String = utf8Code.GetString(utf8Bytes, 0, utf8Bytes.Length);

            return utf8String;
        }
    }
}
