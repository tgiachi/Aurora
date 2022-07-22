using Aurora.Api.Entities.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aurora.Api.Entities.Context
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            SavingChanges += OnSavingChanges;

            foreach (var entityType in model.Model.GetEntityTypes())
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType != typeof(Enum))
                    {
                        continue;
                    }

                    var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

                    property.SetValueConverter(converter);
                }


            ScanForConcurrencyTokens();

            GetModelCreating(model);
        }

        private void ScanForConcurrencyTokens()
        {
            //     AssemblyUtils.GetAttribute<TableAttribute>().ForEach(e =>
            //     {
            //         
            //     });
        }

        protected virtual void GetModelCreating(ModelBuilder model)
        {

        }

        private void OnSavingChanges(object sender, SavingChangesEventArgs e)
        {
            if (sender is IBaseEntity<Guid> baseGuidEntity)
            {
                if (baseGuidEntity.Id == null || baseGuidEntity.Id == Guid.Empty)
                {
                    baseGuidEntity.Id = Guid.NewGuid();
                    baseGuidEntity.CreateDateTime = DateTime.Now;
                }

                baseGuidEntity.UpdatedDateTime = DateTime.Now;

                return;

            }


            if (sender is not IBaseEntity<long> baseEntity)
            {
                return;
            }

            if (baseEntity.Id == 0)
            {
                baseEntity.CreateDateTime = DateTime.Now;
            }

            baseEntity.UpdatedDateTime = DateTime.Now;
        }
    }
}
