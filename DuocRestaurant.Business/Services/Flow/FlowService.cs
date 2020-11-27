using Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class FlowService : IFlowService
    {
        private readonly FlowSettings flowSettings;
        public FlowService(IOptions<FlowSettings> flowSettings)
        {
            this.flowSettings = flowSettings.Value;
        }

        public CreateEmailResponse CreateEmailPayment(string email, int amount, string description, int paymentId)
        {
            CreateEmailResponse result = null;
            List<Param> paymentParams = new List<Param>
            {
                new Param()
                {
                    Key = "apiKey",
                    Value = this.flowSettings.APIKey
                },
                new Param()
                {
                    Key = "commerceOrder",
                    Value = paymentId
                },
                new Param()
                {
                    Key = "subject",
                    Value = description
                },
                new Param()
                {
                    Key = "currency",
                    Value = "CLP"
                },
                new Param()
                {
                    Key = "amount",
                    Value = amount
                },
                new Param()
                {
                    Key = "email",
                    Value = email
                },
                new Param()
                {
                    Key = "urlConfirmation",
                    Value = $"http://localhost:4200/payment-confirmed/{paymentId}"
                },
                new Param()
                {
                    Key = "urlReturn",
                    Value = $"http://localhost:4200/payment-confirmed/{paymentId}"
                }
            };

            paymentParams = paymentParams.OrderBy(x => x.Key).ToList();

            string sign = CalculateHMAC(this.GetStringToSing(paymentParams));
            var client = new RestClient("https://sandbox.flow.cl/api/payment/createEmail");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            foreach (var param in paymentParams)
            {
                request.AddParameter(param.Key, param.Value);
            }

            request.AddParameter("s", sign);
            IRestResponse response = client.Execute(request).Result;

            if (response.IsSuccess)
                result = JsonConvert.DeserializeObject<CreateEmailResponse>(response.Content);

            return result;
        }

        public PaymentStatusResponse GetStatus(int commerceId)
        {
            PaymentStatusResponse result = null;

            List<Param> paymentParams = new List<Param>()
            {
                new Param()
                {
                    Key = "apiKey",
                    Value = this.flowSettings.APIKey
                },
                new Param()
                {
                    Key = "commerceId",
                    Value = commerceId.ToString()
                }
            };
            paymentParams = paymentParams.OrderBy(x => x.Key).ToList();
            string sign = CalculateHMAC(this.GetStringToSing(paymentParams));
            string queryParams = GetQueryParams(paymentParams, sign);
            var client = new RestClient($"https://sandbox.flow.cl/api/payment/getStatusByCommerceId?{queryParams}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request).Result;

            if (response.IsSuccess)
                result = JsonConvert.DeserializeObject<PaymentStatusResponse>(response.Content);

            return result;
        }

        private string GetQueryParams(List<Param> paymentParams, string sign)
        {
            string result = string.Empty;
            foreach (var param in paymentParams)
            {
                if (param == paymentParams.First())
                    result = $"{param.Key}={param.Value}";
                else
                    result += $"&{param.Key}={param.Value}";
            }

            if (!string.IsNullOrWhiteSpace(sign))
                result += $"&s={sign}";

            return result;
        }

        private string GetStringToSing(List<Param> paymentParams)
        {
            string result = string.Empty;

            if (paymentParams != null && paymentParams.Any())
            {
                foreach (var p in paymentParams)
                {
                    result += $"{p.Key}{p.Value}";
                }
            }

            return result;
        }

        private string CalculateHMAC(string data)
        {
            byte[] key = Encoding.ASCII.GetBytes(this.flowSettings.Secret);
            HMACSHA256 myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            Console.WriteLine(result);
            return result;
        }

        private class Param
        {
            public string Key { get; set; }
            public object Value { get; set; }
        }
    }

    public class CreateEmailResponse
    {
        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("floworder")]
        public int FlowOrder { get; set; }
    }

    public class PaymentStatusResponse
    {
        [JsonProperty("flowOrder")]
        public int FlowOrder { get; set; }
        [JsonProperty("commerceOrder")]
        public string CommerceOrder { get; set; }
        [JsonProperty("requestDate")]
        public string RequestDate { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("payer")]
        public string PayerEmail { get; set; }
        [JsonProperty("paymentData")]
        public PaymentData PaymentData { get; set; }
    }

    public class PaymentData
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("media")]
        public string Media { get; set; }
        [JsonProperty("conversionDate")]
        public string ConversionDate { get; set; }
        [JsonProperty("conversionRate")]
        public decimal? ConversionRate { get; set; }
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("fee")]
        public decimal? Fee { get; set; }
        [JsonProperty("balance")]
        public decimal? Balance { get; set; }
        [JsonProperty("transferDate")]
        public string TransferDate { get; set; }
    }
}
