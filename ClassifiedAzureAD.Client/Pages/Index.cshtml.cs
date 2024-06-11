using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassifiedAzureAD.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var httpclient = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7040/WeatherForecast");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            var response = await httpclient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(response.StatusCode.ToString());
            }

        }
    }
}
