using Application.DTOs;
using Core.Appliaction.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.SendMail
{
    internal class MailAddressVerificationService : IMailAddressVerificationService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public string _verificationServiceApiKey;

        public MailAddressVerificationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _verificationServiceApiKey = _configuration.GetSection("abstractApi")["verificationKey"];
        }

        public async Task<BaseResponse> VerifyMailAddress(string emailAddress)
        {
            string requestUri = @$"https://emailvalidation.abstractapi.com/v1/?api_key={_verificationServiceApiKey}&email={emailAddress}";
            requestUri = requestUri.Replace(" ", "");
            _httpClient.BaseAddress = new Uri(requestUri);
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(new
            {
                to = emailAddress,
                FromApplication = "The FORT Application"
            }));

            var requestResponse = await _httpClient.GetAsync(requestUri);
            if (requestResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.Error.WriteLine($"Request Failed with status: {requestResponse.StatusCode}");
                return new BaseResponse
                {
                    Status = false
                };
            }
            string responseString = await requestResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<MailAddressVerificationResponse>(responseString);

            if (parsedResponse.Deliverability != "DELIVERABLE")
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Invalid EmailAddress",
                };
            }

            return new BaseResponse
            {
                Status = true,
            };
        }
    }
}
