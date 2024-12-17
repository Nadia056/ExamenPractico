using System.Net.Http.Headers;
using Azure;
using Azure.Core;
using ExamenPractico.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class ApiService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }


    public async Task<HttpResponseMessage> GetResults()
    {
        var apiUrl = _configuration["Keys:ApiUrl"];
        var apiKey = _configuration["Keys:ApiKey"];
        var apiVersion = _configuration["Keys:ApiVersion"];
        if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("Missing configuration");
        }
        _client.BaseAddress = new Uri(apiUrl);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
        _client.DefaultRequestHeaders.Add("Version", apiVersion);

        return await _client.GetAsync("Test/Customer");
    }

    public async Task<List<Addresse>> GetAddress(HttpResponseMessage response)
    {

        string result = await response.Content.ReadAsStringAsync();
        string json = JsonConvert.DeserializeObject<string>(result); 
        JObject parsedJson = JObject.Parse(json);


        List<Addresse> addresses = new List<Addresse>();
        foreach (var item in parsedJson["addresses"])
        {
            string address1 = item["address1"].ToString();
            string city = item["city"].ToString();
            string stateCode = item["stateCode"].ToString();
            string postalCode = item["postalCode"].ToString();
            string countryCode = item["countryCode"].ToString();
            string creationDate = item["creationDate"].ToString();
            bool preferred = item["preferred"].ToObject<bool>();

            Addresse address = new Addresse
            {
                address1 = address1,
                city = city,
                stateCode = stateCode,
                postalCode = postalCode,
                countryCode = countryCode,
                creationDate = creationDate,
                preferred = preferred
            };
            addresses.Add(address);
        }

        return addresses;

    }

}









