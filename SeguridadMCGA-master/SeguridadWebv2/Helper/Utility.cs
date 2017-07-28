using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Helper
{
    public class Utility
    {
        //public static PaymentResult Request(TransactionRequest request)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        PaymentResult result = null;
        //        client.BaseAddress = new Uri("http://localhost:4635/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        // HTTP POST

        //        var response = client.PostAsJsonAsync("api/payment", request).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            dynamic content = JsonConvert.DeserializeObject<PaymentResult>(response.Content.ReadAsStringAsync().Result);

        //            // Access variables from the returned JSON object
        //            result = content;
        //        }

        //        return result;
        //    }
        //}
    }
}