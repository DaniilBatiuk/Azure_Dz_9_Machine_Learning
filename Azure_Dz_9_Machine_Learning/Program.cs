// See https://aka.ms/new-console-template for more information
using Azure_Dz_9_Machine_Learning;
using Newtonsoft.Json;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

await InvokeRequestResponseService();
async Task InvokeRequestResponseService()
{
    var handler = new HttpClientHandler()
    {
        ClientCertificateOptions = ClientCertificateOption.Manual,
        ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
    };
    using (var client = new HttpClient(handler))
    {
        // Request data goes here
        // The example below assumes JSON formatting which may be updated
        // depending on the format your endpoint expects.
        // More information can be found here:
        // https://docs.microsoft.com/azure/machine-learning/how-to-deploy-advanced-entry-script

        BikeShareData bikeShareData = new()
        {
            Inputs = new Inputs
            {
                Data = new List<Datum> {
                    new Datum()
                    {
                        Instant = 732,
                        Date = "2013-01-01 00:00:00",
                        Season = 1,
                        Yr = 0,
                        Mnth = 1,
                        Weekday = 6,
                        Weathersit = 2,
                        Temp = 0.344167,
                        Atemp = 0.363635,
                        Hum = 0.805833,
                        Windspeed = 0.160446

                    }
                }
            }
        };

        client.BaseAddress = new Uri("http://58d87bac-c87b-47c6-bbb5-57a42af00f56.northeurope.azurecontainer.io/score");
        string requestBody = JsonConvert.SerializeObject(bikeShareData);
        var content = new StringContent(requestBody);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // WARNING: The 'await' statement below can result in a deadlock
        // if you are calling this code from the UI thread of an ASP.Net application.
        // One way to address this would be to call ConfigureAwait(false)
        // so that the execution does not attempt to resume on the original context.
        // For instance, replace code such as:
        //      result = await DoSomeTask()
        // with the following:
        //      result = await DoSomeTask().ConfigureAwait(false)
        HttpResponseMessage response = await client.PostAsync("", content);

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Result: {0}", result);
            BikeShareResult? bikeShareResult = JsonConvert.DeserializeObject<BikeShareResult>(result);
            Console.WriteLine($"Result: {bikeShareResult?.Results[0]}");
        }
        else
        {
            Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

            // Print the headers - they include the requert ID and the timestamp,
            // which are useful for debugging the failure
            Console.WriteLine(response.Headers.ToString());

            string responseContent = await response.Content.ReadAsStringAsync();
        }
    }
}