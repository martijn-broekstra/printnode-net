﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public sealed class PrintNodePrintJob
    {
        /// <summary>
        /// Assigned by API. Any value submitted here will be ignored.
        /// </summary>
        [JsonProperty("id")]
        public long? Id { get; set; }

        /// <summary>
        /// The printer id associated with your account.
        /// </summary>
        [JsonProperty("printerId")]
        public long PrinterId { get; set; }

        [JsonProperty("printer")]
        public PrintNodePrinter Printer { get; set; }

        /// <summary>
        /// A title to give the PrintJob. This is the name which will appear in the operating system's print queue.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Either 'pdf_uri', 'pdf_base64', 'raw_uri', 'raw_base64'. 
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#create-printjob-content">content</a>.
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// A uri accessible by the client when contentType is 'pdf_uri'.
        /// or
        /// A base64 encoded representation of the pdf when contentType is 'pdf_base64'.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// A text description of how the printjob was created or where the printjob originated.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// An object describing various options which can be set for this PrintJob. See options. Printing options have no effect when raw printing.
        /// </summary>
        [JsonProperty("options")]
        public PrintNodePrintJobOptions Options { get; set; }

        /// <summary>
        /// The maximum number of seconds PrintNode should retain this PrintJob for attempted printing in the event the PrintJob cannot be 
        /// printed immediately. The current default is 14 days or 1,209,600 seconds.
        /// </summary>
        [JsonProperty("expireAfter")]
        public long? ExpireAfter { get; set; }

        /// <summary>
        /// The default value is 1. A positive integer representing the number of times this PrintJob should be delivered to the print queue. 
        /// This differs from the "copies" option in that this will send a document to a printer multiple times and does not rely on print driver 
        /// support. This is the only way to support multiple copies when raw printing. This also enables printing multiple copies even when a 
        /// printer driver does not natively support this.
        /// </summary>
        [JsonProperty("qty")]
        public int Qty { get; set; }

        /// <summary>
        /// If a contentType of 'pdf_uri' or 'raw_uri' is used and the uri requires either HTTP Basic or Digest Authentication you can specify 
        /// the username and password information here. Supported in clients v4.7.0 or newer.
        /// 
        /// For Basic authentication
        /// {
        ///     "type": "BasicAuth",
        ///     "credentials": {
        ///         "user": "username",
        ///         "pass": "password"
        ///     }
        /// }
        /// 
        /// For Digest authentication
        /// 
        /// {
        ///     "type": "DigestAuth",
        ///     "credentials": {
        ///         "user": "username",
        ///         "pass": "password"
        ///     }
        /// }
        /// Just replace the "username" and "password" with your credentials as appropriate.
        /// </summary>
        [JsonProperty("authentication")]
        public PrintNodePrintJobAuthentication Authentication { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("createTimeStamp")]
        public DateTime? CreateTimeStamp { get; set; }

        private PrintNodeDelegatedClientContext ClientContext { get; set; }

        public static async Task<IEnumerable<PrintNodePrintJob>> ListAsync(PrintNodeDelegatedClientContext clientContext = null)
        {
            var response = await ApiHelper.Get("/printjobs", clientContext);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrintJob>>(response);

            // Set clientContext on each job object;
            list.ForEach(j => j.ClientContext = clientContext);

            return list;
        }

        public static async Task<IEnumerable<PrintNodePrintJob>> ListForPrinterAsync(int printerId, PrintNodeDelegatedClientContext clientContext = null)
        {
            var response = await ApiHelper.Get("/printers/" + printerId + "/printjobs");

            var list = JsonConvert.DeserializeObject<List<PrintNodePrintJob>>(response);

            // Set clientContext on each job object;
            list.ForEach(j => j.ClientContext = clientContext);

            return list;
        }

        public static async Task<PrintNodePrintJob> GetAsync(int id, PrintNodeDelegatedClientContext clientContext = null)
        {
            var response = await ApiHelper.Get("/printjobs/" + id);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrintJob>>(response);

            // Set clientContext on each job object;
            list.ForEach(j => j.ClientContext = clientContext);

            return list.FirstOrDefault();
        }

        public async Task<IEnumerable<PrintNodePrintJobState>> GetStates()
        {
            var response = await ApiHelper.Get("/printjobs/" + Id + "/states", ClientContext);

            var list = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PrintNodePrintJobState>>>(response);

            return list.FirstOrDefault();
        }
    }
}
