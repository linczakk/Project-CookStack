using CookStack.Client.Services.ToastMessage;
using System.Net.Http.Json;

namespace CookStack.Client.Services
{
    public class BaseApiClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly ToastService _toast;

        public BaseApiClient(HttpClient http, ToastService toast)
        {
            _httpClient = http;
            _toast = toast;
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<T>(url);
            }
            catch
            {
                _toast.ShowError("Failed to fetch data");
                return default;
            }
        }

        protected async Task<bool> PostAsync<T>(string url, T data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, data);

                if(!response.IsSuccessStatusCode)
                {
                    _toast.ShowError("Something went wrong");
                    return false;
                }
                return true;
            }
            catch
            {
                _toast.ShowError("Server unreachable");
                return false;
            }
        }

        protected async Task<bool> PutAsync<T>(string url, T data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, data);

                if(!response.IsSuccessStatusCode)
                {
                    _toast.ShowError("Update failed");
                    return false;
                }
                return true;
            }
            catch
            {
                _toast.ShowError("Server unreachable");
                return false;
            }
        }

        protected async Task<bool> DeleteAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                
                if(!response.IsSuccessStatusCode)
                {
                    _toast.ShowError("Delete failed");
                    return false;
                }
                return true;
            }
            catch
            {
                _toast.ShowError("Server unreachable");
                return false;
            }
        }
    }
}
