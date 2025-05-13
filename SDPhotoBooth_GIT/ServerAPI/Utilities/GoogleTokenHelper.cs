using BusinessLogic.DTO.GoogleDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.Service;
using BussinessObject.Enums;
using BussinessObject.Models;
using BussinessObject.ViewModels;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ServerAPI.Controllers;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ServerAPI.Helpers
{
    public class GoogleTokenHelper
    {
        private static string CredentialPath = "Key/sd-photobooth-452212-2ff885bdb49e.json";

        public static async Task<GoogleJsonWebSignature.Payload?> VerifyAccessTokenAsync(string idToken, string googleWebClientId, string googleMobileClientId)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleWebClientId, googleMobileClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch
            {
                return null;
            }
        }


        public static async Task<GooglePurchaseResponse> VerifyPurchaseAsync(string typeSessionId, string purchaseToken)
        {
            string url = $"https://androidpublisher.googleapis.com/androidpublisher/v3/applications/com.techx.sd_photobooth_app/purchases/products/{typeSessionId}/tokens/{purchaseToken}";

            string accessToken = await GoogleTokenHelper.GetGoogleAccessToken();
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to verify purchase");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GooglePurchaseResponse>(responseBody);
        }

        private static async Task<string> GetGoogleAccessToken()
        {
            GoogleCredential credential;

            using (var stream = new FileStream(CredentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);
            }

            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return token;
        }

    }
}
