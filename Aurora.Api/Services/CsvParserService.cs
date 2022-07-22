using System.Globalization;
using System.Text;
using Aurora.Api.Interfaces.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Dasync.Collections;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Services
{
    public class CsvParserService : ICsvParserService
    {
        private readonly ILogger _logger;

        public CsvParserService(ILogger<CsvParserService> logger)
        {
            _logger = logger;
        }

        public async Task<List<TRecord>> ParseRecords<TRecord, TClassMap>(string data, TClassMap classMap)
            where TClassMap : ClassMap<TRecord>
        {
            _logger.LogInformation("Parsing CSV, size: {Size}", data.Length.Bytes().Humanize());
            using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<TClassMap>();

            var asyncEnumerable = csv.GetRecordsAsync<TRecord>();
            return await asyncEnumerable.ToListAsync();
        }

        public async Task<List<TRecord>> ParseRecords<TRecord>(string data)
        {
            _logger.LogInformation("Parsing CSV, size: {Size}", data.Length.Megabytes().Humanize());
            using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var asyncEnumerable = csv.GetRecordsAsync<TRecord>();
            return await asyncEnumerable.ToListAsync();
        }


        public async Task<string> WriteRecords<TRecord>(IEnumerable<TRecord> records)
        {
            var outStream = new MemoryStream();
            await using var writer = new StreamWriter(outStream);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(records);

            await writer.FlushAsync();
            return Encoding.UTF8.GetString(outStream.ToArray());
        }
    }
}
