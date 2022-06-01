using NeutronChat.Domain.Models;
using NeutronChat.Domain.Requests;
using NeutronChat.Domain.Resposes;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NeutronChat.BlazorClient.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            this.httpClient=httpClient;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }



        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync<LoginRequest>("api/Authentication/Login", request);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<LoginResponse>(responseBody);

            return new LoginResponse
            {
                ErrorMessage=responseBody
            };

        }

        public async Task<RegisterResponse?> RegisterUserAsync(RegisterRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/Authentication/Register", request);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<RegisterResponse>(responseBody);

            return new RegisterResponse
            {
                ErrorMessage = responseBody
            };
        }

        public async Task<LoginResponse?> UpdateTokenAsync(string refreshToken)
        {
            var response = await httpClient.PostAsJsonAsync("api/Authentication/UpdateToken", refreshToken);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<LoginResponse>(responseBody);

            return new LoginResponse
            {
                ErrorMessage = responseBody
            };
        }

        public async Task<User?> GetUserByAccessTokenAsync(string token)
        {
            var response = await httpClient.PostAsJsonAsync("api/Authentication/GetUserByAccessToken", token);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<User>(responseBody);

            return null;
        }

        public async Task SendConfirmationCodeAsync(string email) =>
            await httpClient.PostAsJsonAsync("api/Autentication/SendConfirmationCode", email);

        public async Task LogoutAsync() =>
            await httpClient.DeleteAsync("api/Autentication/Logout");
    }
}
