using System;

namespace GDFUnity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FullLockersAttribute : ManagerAttribute
    {
        public FullLockersAttribute(Type[] types) : base(types)
        {
        }

        public FullLockersAttribute(Type type) : base(type)
        {
        }

        public FullLockersAttribute(Type type1, Type type2) : base(type1, type2)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3) : base(type1, type2, type3)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3, Type type4) : base(type1, type2, type3, type4)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3, Type type4, Type type5) : base(type1, type2, type3, type4, type5)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6) : base(type1, type2, type3, type4, type5, type6)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7) : base(type1, type2, type3, type4, type5, type6, type7)
        {
        }

        public FullLockersAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8) : base(type1, type2, type3, type4, type5, type6, type7, type8)
        {
        }
    }
}