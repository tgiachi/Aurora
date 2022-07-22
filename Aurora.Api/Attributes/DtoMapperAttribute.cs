namespace Aurora.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DtoMapperAttribute : Attribute
    {
        public DtoMapperAttribute(Type sourceType, Type targetType)
        {
            TargetType = targetType;
            SourceType = sourceType;
        }

        public Type SourceType { get; set; }
        public Type TargetType { get; set; }
    }
}
