using System;


namespace Common.Attributes
{
    /// <summary>
    /// Indicates whether or not an enum must have a NameAttribute and a DescriptionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class DescriptiveEnumEnforcementAttribute : Attribute
    {
        public DescriptiveEnumEnforcementType EnforcementType { get; set; }

        public DescriptiveEnumEnforcementAttribute()
        {
            this.EnforcementType = DescriptiveEnumEnforcementType.DefaultToValue;
        }
        public DescriptiveEnumEnforcementAttribute(DescriptiveEnumEnforcementType enforcementType)
        {
            this.EnforcementType = enforcementType;
        }
    }

    public enum DescriptiveEnumEnforcementType
    {
        ThrowException,
        DefaultToValue
    }
}