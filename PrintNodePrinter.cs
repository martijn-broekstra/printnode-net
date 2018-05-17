using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public sealed class PrintNodePrinter
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("computer")]
        public PrintNodeComputer Computer { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("capabilities")]
        public PrintNodePrinterCapabilities Capabilities { get; set; }

        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("createTimeStamp")]
        public DateTime CreateTimeStamp { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        private PrintNodeDelegatedClientContext ClientContext { get; set; }

        public static async Task<IEnumerable<PrintNodePrinter>> ListAsync(PrintNodeDelegatedClientContext clientContext = null)
        {
            var response = await ApiHelper.Get("/printers", clientContext);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

            // Set clientContext on each printer object;
            list.ForEach(p => p.ClientContext = clientContext);

            return list;
        }

        public static async Task<PrintNodePrinter> GetAsync(long id, PrintNodeDelegatedClientContext clientContext = null)
        {
            var response = await ApiHelper.Get("/printers/" + id, clientContext);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

            // Set clientContext on each printer object;
            list.ForEach(p => p.ClientContext = clientContext);

            return list.FirstOrDefault();
        }

        public async Task<long> AddPrintJob(PrintNodePrintJob job)
        {
            job.PrinterId = Id;

            var response = await ApiHelper.Post("/printjobs", job, ClientContext);

            return JsonConvert.DeserializeObject<long>(response);
        }
    }
}
