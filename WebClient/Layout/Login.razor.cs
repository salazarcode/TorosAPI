using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProtectedLocalStore;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace WebClient.Layout
{
    public partial class Login
    {
        //
        private string username;
        private string password;
        private string token;
        private bool error;
        private string error_message;
        private WeatherForecast[]? forecasts;

        private async Task HandleLogin()
        {
            try
            {
                var loginData = new { username, password };
                var httpClient = new HttpClient();

                var response = await httpClient.PostAsJsonAsync("https://localhost:7263/Api/Auth/Login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    token = res.Token;
                }
                else
                {
                    // Handle login failure
                    Console.WriteLine("Login failed");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task GetWeather()
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    // Handle missing token case (e.g., redirect to login)
                    return;
                }

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                forecasts = await client.GetFromJsonAsync<WeatherForecast[]>("https://localhost:7255/WeatherForecast");
            }
            catch (Exception e)
            {
                error = true;
                error_message = e.Message;
            }

        }

        public class WeatherForecast
        {
            public DateOnly Date { get; set; }
            public int TemperatureC { get; set; }
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            public string? Summary { get; set; }
        }

        public class TokenResponse
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
        }
    }
}