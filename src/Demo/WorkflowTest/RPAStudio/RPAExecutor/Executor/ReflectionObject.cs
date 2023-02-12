using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPAExecutor.Executor
{
    internal class ReflectionObject : DynamicObject
    {
        private const BindingFlags Flags =
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

       
        private readonly object inner;

       
        internal ReflectionObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            this.inner = obj;
        }

        
        public object Inner
        {
            get
            {
                return this.inner;
            }
        }

       
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.TryGetProperty(binder.Name, out result) || this.TryGetField(binder.Name, out result);
        }

        
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var method = GetMethod(this.Inner.GetType(), binder.Name, GetTypes(args));

            if (method != null)
            {
                result = method.Invoke(this.Inner, args);
                return true;
            }

            result = null;
            return false;
        }

        
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.TrySetProperty(binder.Name, value) || this.TrySetField(binder.Name, value);
        }

        
        internal static dynamic Load(Assembly assembly, string typeName, params object[] args)
        {
            var type = assembly.GetTypes().First(item => item.Name == typeName);
            var ctor = type.GetConstructor(Flags, null, GetTypes(args), null);

            return ctor != null ? new ReflectionObject(ctor.Invoke(args)) : null;
        }

        
        private static FieldInfo GetField(Type type, string name)
        {
            var fld = type.GetField(name, Flags);

            return fld ?? (type.BaseType != null ? GetField(type.BaseType, name) : null);
        }

        
        private static MethodInfo GetMethod(Type type, string name, Type[] args)
        {
            var method = type.GetMethod(name, Flags, null, args, null);

            return method ?? (type.BaseType != null ? GetMethod(type.BaseType, name, args) : null);
        }

       
        private static PropertyInfo GetProperty(Type type, string name)
        {
            var prop = type.GetProperty(name, Flags);

            return prop ?? (type.BaseType != null ? GetProperty(type.BaseType, name) : null);
        }

        
        private static Type[] GetTypes(IEnumerable<object> args)
        {
            return args.Select((o, i) => o.GetType()).ToArray();
        }

        
        private bool TryGetField(string name, out object result)
        {
            var fld = GetField(this.Inner.GetType(), name);

            if (fld != null)
            {
                result = fld.GetValue(this.Inner);
                return true;
            }

            result = null;
            return false;
        }

        
        private bool TryGetProperty(string name, out object result)
        {
            var prop = GetProperty(this.Inner.GetType(), name);
            if (prop != null)
            {
                result = prop.GetValue(this.Inner, null);
                return true;
            }

            result = null;
            return false;
        }

        
        private bool TrySetField(string name, object value)
        {
            var fld = GetField(this.Inner.GetType(), name);

            if (fld != null)
            {
                fld.SetValue(this.Inner, value);
                return true;
            }

            return false;
        }

        
        private bool TrySetProperty(string name, object value)
        {
            var prop = GetProperty(this.Inner.GetType(), name);

            if (prop != null)
            {
                prop.SetValue(this.Inner, value, null);
                return true;
            }

            return false;
        }
    }
}