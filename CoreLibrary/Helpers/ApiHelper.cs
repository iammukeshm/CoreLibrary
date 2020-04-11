using CoreLibrary.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Helpers
{
    public class ApiHelper
    {
        private HttpClient apiClient;
        public ApiHelper(string baseAddress)
        {
            InitializeClient(baseAddress);
        }
        private void InitializeClient(string baseAddress)
        {
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(baseAddress);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void AddJwtAuthorization(string jwtToken)
        {
            if (!string.IsNullOrEmpty(jwtToken))
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }
        public void RemoveJwtAuthorization()
        {
            apiClient.DefaultRequestHeaders.Remove("Authorization");
        }
        private readonly int maxRetryAttempts = 3;
        private TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(5);
        public async Task<T> GetAsync<T>(string endPointUrl)
        {
            var response = new HttpResponseMessage();
            RetryOnException(maxRetryAttempts, pauseBetweenFailures, async () =>
            {
                response = await apiClient.GetAsync(endPointUrl);
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedException();
            }
            else
            {
                throw new Exception("Exception!");
            }
        }

        public async Task<T> PostAsync<T>(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();
            RetryOnException(maxRetryAttempts, pauseBetweenFailures, async () =>
            {
                response = await apiClient.PostAsync(endPointUrl, payload);
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedException();
            }
            else
            {
                throw new Exception("Exception!");
            }
        }

        public async Task<T> DeleteAsync<T>(string endPointUrl)
        {
            var response = new HttpResponseMessage();
            RetryOnException(maxRetryAttempts, pauseBetweenFailures, async () =>
            {
                response = await apiClient.DeleteAsync(endPointUrl);
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedException();
            }
            else
            {
                throw new Exception("Exception!");
            }
        }

        public async Task<T> PutAsync<T>(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();
            RetryOnException(maxRetryAttempts, pauseBetweenFailures, async () =>
            {
                response = await apiClient.PutAsync(endPointUrl, payload);
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedException();
            }
            else
            {
                throw new Exception("Exception!");
            }
        }

        private  void RetryOnException(int times, TimeSpan delay, Action operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    operation();
                    break; // Sucess! Lets exit the loop!
                }
                catch (Exception)
                {
                    Task.Delay(delay).Wait();
                }
            } while (attempts <= times);
        }
    }
}
