using ExamenPractico.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExamenPractico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetResult()
        {
            var ApiService = new ApiService(new HttpClient(), new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var response = await ApiService.GetResults();
            if (response.IsSuccessStatusCode)
            {

                string result = await response.Content.ReadAsStringAsync();

                string json = JsonConvert.DeserializeObject<string>(result);
                if (json == null)
                {
                    return BadRequest();
                }
                JObject parsedJson = JObject.Parse(json);

                string customerId = parsedJson["customerId"].ToString();
                string email = parsedJson["email"].ToString();
                string firstName = parsedJson["firstName"].ToString();
                string lastName = parsedJson["lastName"].ToString();
                string birthday = parsedJson["birthday"].ToString();
                string phoneMobile = parsedJson["phoneMobile"].ToString();
                
                List<Addresse> addresses = await ApiService.GetAddress(response);

                CustomerItem customer = new CustomerItem
                {
                    customerId = customerId,
                    firstName = firstName,
                    lastName = lastName,
                    birthday = birthday,
                    email = email,
                    phoneMobile = phoneMobile,
                    addresses = addresses,

                };

                var standardResponse = new StandardResponse { msg = "Success", success = true, data = customer };



                return Ok(standardResponse);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("GetAddresses", Name = "GetAddresses")]
        public async Task<ActionResult> GetAddresses(bool OrderAsc, string OrderBy)
        {


            if ((OrderBy != "creationDate" && OrderBy != "Address1") || (OrderAsc != true && OrderAsc != false))
            {
                string validationMessage = "OrderBy must be creationDate or Address1 and OrderAsc must be true or false";
                return BadRequest(validationMessage);
            }
            var ApiService = new ApiService(new HttpClient(), new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var response = await ApiService.GetResults();
            if (response.IsSuccessStatusCode)
            {


                List<Addresse> addresses = await ApiService.GetAddress(response);

                IEnumerable<Addresse> query;

                if (OrderAsc && OrderBy == "creationDate")
                {
                    query = from address in addresses orderby address.creationDate select address;
                }
                else if (!OrderAsc && OrderBy == "creationDate")
                {
                    query = from address in addresses orderby address.creationDate descending select address;
                }
                else if (OrderAsc && OrderBy == "Address1")
                {
                    query = from address in addresses orderby address.address1 select address;
                }
                else if (!OrderAsc && OrderBy == "Address1")
                {
                    query = from address in addresses orderby address.address1 descending select address;
                }
                else
                {
                    return BadRequest();
                }


                var standardResponse = new StandardResponse { msg = "Success", success = true, data = query };



                return Ok(standardResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetPreferredAddress", Name = "GetPreferredAddress")]

        public async Task<ActionResult> GetPreferredAddress()
        {
            var ApiService = new ApiService(new HttpClient(), new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var response = await ApiService.GetResults();
            if (response.IsSuccessStatusCode)
            {
                List<Addresse> addresses = await ApiService.GetAddress(response);
                var query = from address in addresses where address.preferred == true select address;

                if (query == null)
                {
                    return BadRequest();
                }
                var standardResponse = new StandardResponse { msg = "Success", success = true, data = query };

                return Ok(standardResponse);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("GetAddressByPostalCode", Name = "GetAddressByPostalCode")]
        public async Task<ActionResult> GetAddressByPostalCode(string postalCode)
        {
            if (postalCode == null)
            {
                string validationMessage = "PostalCode is required";
                return BadRequest(validationMessage);
            }


            var ApiService = new ApiService(new HttpClient(), new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            var response = await ApiService.GetResults();
            if (response.IsSuccessStatusCode)
            {
                List<Addresse> addresses = await ApiService.GetAddress(response);
                var query = from address in addresses where address.postalCode == postalCode select address;

                return Ok(query);
            }
            else
            {
                return BadRequest();
            }
        }


    }


}
