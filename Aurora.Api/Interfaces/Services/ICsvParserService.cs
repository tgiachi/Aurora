using CsvHelper.Configuration;

namespace Aurora.Api.Interfaces.Services
{
    public interface ICsvParserService
    {
        Task<List<TRecord>> ParseRecords<TRecord>(string data);

        Task<string> WriteRecords<TRecord>(IEnumerable<TRecord> records);

        Task<List<TRecord>> ParseRecords<TRecord, TClassMap>(string data, TClassMap classMap)
            where TClassMap : ClassMap<TRecord>;
    }
}
