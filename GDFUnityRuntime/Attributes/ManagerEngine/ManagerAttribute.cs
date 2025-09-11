using System;

namespace GDFUnity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class ManagerAttribute : Attribute
    {
        public Type[] types;

        public ManagerAttribute(Type type)
            : this(new Type[] { type })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2)
            : this(new Type[] { type1, type2 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3)
            : this(new Type[] { type1, type2, type3 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3, Type type4)
            : this(new Type[] { type1, type2, type3, type4 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)
            : this(new Type[] { type1, type2, type3, type4, type5 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
            : this(new Type[] { type1, type2, type3, type4, type5, type6 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
            : this(new Type[] { type1, type2, type3, type4, type5, type6, type7 })
        {
            
        }
        public ManagerAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8)
            : this(new Type[] { type1, type2, type3, type4, type5, type6, type7, type8 })
        {
            
        }
        public ManagerAttribute(Type[] types)
        {
            this.types = types ?? Array.Empty<Type>();
        }
    }
}