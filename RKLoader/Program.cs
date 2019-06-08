using CommandLine;
using RKeeperReporter.RKeeperExchange;
using Sysiphus.Tasks.SampleTask;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace RKLoader
{

    public class Options
    {
        [Option('D', "connectionString", Required = true, HelpText = @"Строка подключения к БД")]
        public string ConnectionString { get; set; }

        [Option('p', "provider", Required = false, HelpText = "Имя провайдера")]
        public string ProviderName { get; set; } = "System.Data.SqlClient";

        [Option('C', "classificationGroup", Required = true, HelpText = "Шифр группы классификации")]
        public short ClassificationGroupSIFR { get; set; } = 0;

        [Option('r', "restarauntCode", Required = false, HelpText = "Код ресторана, если не задан - все рестораны")]
        public int RestaurantCode { get; set; } = 0;

        [Option('a', "databasePrefix", Required = false, HelpText = "Префикс базы")]
        public int DatabasePrefix { get; set; } = 0;

        [Option('c', "databaseCode", Required = false, HelpText = "Код базы")]
        public int DatabaseCode { get; set; } = 0;

        [Option('n', "databaseCode", Required = false, HelpText = "Имя базы")]
        public string databaseName { get; set; } = "default DB";

        [Option('s', "statDate", Required = false, HelpText = "Начальная дата выгрзуки")]
        public DateTime ReportStartDateTime { get; set; } = DateTime.Now;

        [Option('e', "endDate", Required = false, HelpText = "Конечная дата выгрузки")]
        public DateTime ReportEndDateTime { get; set; } = DateTime.Now;

        [Option('u', "uploadLastDays", Required = false, HelpText = "Выгружать N дней от конечной даты")]
        public int UploadLastDays { get; set; } = 0;

        [Option('f', "file", Required = false, HelpText = "Файл выгрузки")]
        public string File { get; set; } = $"{DateTime.Now.ToString("MM.dd.yyyy-HH.mm.ss")}.json";
    }

    class Program
    {
        private static Options options;

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            result.WithParsed<Options>(o => { options = o; });
            result.WithNotParsed(errors => { Environment.Exit(1); });

            Console.WriteLine("Подождите, происходит формирование выгрузки.");

            var settings = new RkeeperLoader.RkeeperLoaderSettings()
            {
                ConnectionString = options.ConnectionString,
                ProviderName = options.ProviderName,
                ClassificationGroupSIFR = options.ClassificationGroupSIFR,
                restaurantCode = options.RestaurantCode,
                databasePrefix = options.DatabasePrefix,
                ReportEndDateTime = options.ReportEndDateTime,
                ReportStartDateTime = options.ReportStartDateTime
            };
            var reports = RkeeperLoader.GetReports(settings);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Report>));

            using (FileStream fs = new FileStream(options.File, FileMode.OpenOrCreate))
            {
                serializer.WriteObject(fs, reports);
            }

            Console.WriteLine("Готово.");
        }
    }
}
