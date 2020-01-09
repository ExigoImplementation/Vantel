using System;

namespace Common.Filters
{
    public class PropertyBinderAttribute : Attribute
    {
        public PropertyBinderAttribute(Type binderType)
        {
            BinderType = binderType;
        }

        public Type BinderType { get; private set; }
    }
}