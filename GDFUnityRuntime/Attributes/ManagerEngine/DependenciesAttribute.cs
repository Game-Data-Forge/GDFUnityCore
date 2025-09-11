using System;

namespace GDFUnity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DependencyAttribute : ManagerAttribute
    {
        public DependencyAttribute(Type[] types) : base(types)
        {
        }

        public DependencyAttribute(Type type) : base(type)
        {
        }

        public DependencyAttribute(Type type1, Type type2) : base(type1, type2)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3) : base(type1, type2, type3)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3, Type type4) : base(type1, type2, type3, type4)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3, Type type4, Type type5) : base(type1, type2, type3, type4, type5)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6) : base(type1, type2, type3, type4, type5, type6)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7) : base(type1, type2, type3, type4, type5, type6, type7)
        {
        }

        public DependencyAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8) : base(type1, type2, type3, type4, type5, type6, type7, type8)
        {
        }
    }
}