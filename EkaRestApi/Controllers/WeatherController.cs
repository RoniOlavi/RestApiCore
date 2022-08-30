using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        [Route("{key}")]
        public string GetWeather(string key)
        {
            WebClient client = new WebClient();
            try
            {
                string data = client.DownloadString("https://ilmatieteenlaitos.fi/saa/" + key);
                int index = data.IndexOf("Temperature");
                if (index > 0)
                {
                    string weather = data.Substring(index + 40, 3).Replace(":", "");
                    return weather;
                }
            }
            finally
            {
                client.Dispose();
            }
            return ("unknown");
        }

    }
}
