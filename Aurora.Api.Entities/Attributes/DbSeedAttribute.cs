namespace Aurora.Api.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DbSeedAttribute : Attribute
    {
        public DbSeedAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
