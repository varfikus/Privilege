using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PrivilegeAPI
{
    public class MyHttpClient
    {
        private readonly HttpClient _client;
        /// <summary>Подкаталог API для обновления токенов</summary>
        private const string RefreshTokenUrl = "api/Auth/Refresh";
        private string _accessToken = string.Empty;
        private string _refreshToken = string.Empty;

        /// <summary>Последний код запроса</summary>
        public HttpStatusCode LastCode { get; private set; }

        /// <summary>Последний сырой контент ответа</summary>
        public string LastContent { get; private set; } = string.Empty;

        /// <summary>Автообновление токенов</summary>
        public bool RefreshTokens { get; set; } = true;

        public MyHttpClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Установить токены
        /// </summary>
        /// <param name="accessToken">Токен доступа</param>
        /// <param name="refreshToken">Токен обновления</param>
        public void SetTokens(string accessToken, string refreshToken)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            SetHeaderAuthorization(_accessToken);
        }

        /// <summary>
        /// Установить токен в заголовок
        /// </summary>
        /// <param name="token">Токен</param>
        public void SetHeaderAuthorization(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Удаление заголовка авторизации
        /// </summary>
        public void RemoveHeaderAuthorization()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            // Или
            //_client.DefaultRequestHeaders.Remove("Authorization");
        }

        /// <summary>
        /// Асинхронный Get-запрос
        /// </summary>
        /// <typeparam name="T">Базовый ответ API с данными или DTO-модели</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<T?> GetRawAsync<T>(string url, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecuteGetRequestAsync<T>(link, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecuteGetRequestAsync<T>(link, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<T>(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Асинхронный Post-запрос
        /// </summary>
        /// <typeparam name="T">Базовый ответ API с данными или DTO-модели</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<T?> PostAsync<T>(string url, T data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecutePostRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecutePostRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<T>(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Асинхронный Post-запрос
        /// </summary>
        /// <typeparam name="TRequest">Тип данных, который отправляется на сервер</typeparam>
        /// <typeparam name="TResponse">Тип данных, который ожидается в ответе</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecutePostRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecutePostRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<TResponse>(response, cancel).ConfigureAwait(false);
        }

        public async Task<T?> PostAsync<T>(string url, HttpContent content, CancellationToken cancel = default)
        {
            var response = await _client.PostAsync(url, content, cancel);
            LastCode = response.StatusCode;
            LastContent = await response.Content.ReadAsStringAsync(cancel);
            return await GetIsSuccessJsonFromResponse<T>(response, cancel);
        }

        /// <summary>
        /// Асинхронный Put-запрос
        /// </summary>
        /// <typeparam name="T">Базовый ответ API с данными или DTO-модели</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<T?> PutAsync<T>(string url, T data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecutePutRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecutePutRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<T>(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Асинхронный Put-запрос
        /// </summary>
        /// <typeparam name="TRequest">Тип данных, который отправляется на сервер</typeparam>
        /// <typeparam name="TResponse">Тип данных, который ожидается в ответе</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecutePutRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecutePutRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<TResponse>(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Асинхронный Delete-запрос
        /// </summary>
        /// <typeparam name="T">Базовый ответ API с данными или DTO-модели</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<T?> DeleteAsync<T>(string url, T data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecuteDeleteRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecuteDeleteRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<T>(response, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// Асинхронный Delete-запрос
        /// </summary>
        /// <typeparam name="TRequest">Тип данных, который отправляется на сервер</typeparam>
        /// <typeparam name="TResponse">Тип данных, который ожидается в ответе</typeparam>
        /// <param name="url">Подкаталог API</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Данные из API</returns>
        public async Task<TResponse?> DeleteAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancel = default)
        {
            var link = $"{_client.BaseAddress?.OriginalString}/{TrimUrl(url)}";
            var response = await ExecuteDeleteRequestAsync(link, data, cancel).ConfigureAwait(false);

            // Если включено автообновление токена и произошла ошибка авторизации,
            // то происходит обновление токена и повтор запроса
            if (RefreshTokens && LastCode == HttpStatusCode.Unauthorized)
            {
                // Обновление токена
                if (!await RefreshTokenAsync(cancel))
                    throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");

                // Повтор запроса
                response = await ExecuteDeleteRequestAsync(link, data, cancel).ConfigureAwait(false);
            }

            return await GetIsSuccessJsonFromResponse<TResponse>(response, cancel).ConfigureAwait(false);
        }

        /*-------------------------------------*/

        /// <summary>
        /// Очистить ссылку от лишних символов
        /// </summary>
        /// <param name="url">Ссылка</param>
        /// <returns></returns>
        private static string TrimUrl(string url)
        {
            return url.Trim('/').Trim();
        }

        /// <summary>
        /// В случае положительного кода API (включая исключения) получить десериализованный ответ в объект T.
        /// </summary>
        /// <typeparam name="T">Базовый ответ API с данными или DTO-модели</typeparam>
        /// <param name="response">Ответ сервера</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Получившийся ответ или <c>default</c></returns>
        private async Task<T?> GetIsSuccessJsonFromResponse<T>(HttpResponseMessage response, CancellationToken cancel = default)
        {
            // Дополнительное разрешение для кодов BadRequest и NotFound,
            // т.к. API может возвращать ответ с такими кодами.
            // Возможно стоит инициировать ошибку с текстовым представлением ответа...
            if (response.IsSuccessStatusCode ||
                !string.IsNullOrWhiteSpace(LastContent) ||
                response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
                return await response
                    .Content
                    .ReadFromJsonAsync<T>(cancellationToken: cancel)
                    .ConfigureAwait(false);
            throw new HttpRequestException($"Ошибка авторизации: {LastCode}, Содержание: {LastContent}");
        }

        private async Task<bool> RefreshTokenAsync(CancellationToken cancel = default)
        {
            if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_refreshToken))
                return false;

            var item = new TokenDto
            {
                AccessToken = _accessToken,
                RefreshToken = _refreshToken
            };

            var response = await ExecutePostRequestAsync(RefreshTokenUrl, item, cancel).ConfigureAwait(false);
            var info = await GetIsSuccessJsonFromResponse<BaseResult<TokenDto>>(response, cancel).ConfigureAwait(false);

            if (info?.Data != null)
            {
                _accessToken = info.Data.AccessToken;
                _refreshToken = info.Data.RefreshToken;
                SetHeaderAuthorization(_accessToken);
                return true;
            }

            return false;
        }

        /*-------------------------------------*/

        /// <summary>
        /// Выполнение GET-запроса
        /// </summary>
        /// <typeparam name="T">Тип данных, которые отправляются на сервер</typeparam>
        /// <param name="url">Ссылка</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>HTTP-ответ от сервера</returns>
        private async Task<HttpResponseMessage> ExecuteGetRequestAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await _client
                .GetAsync(url, cancel)
                .ConfigureAwait(false);

            // Сохранение данных в свойства
            LastCode = response.StatusCode;
            LastContent = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Выполнение POST-запроса
        /// </summary>
        /// <typeparam name="T">Тип данных, которые отправляются на сервер</typeparam>
        /// <param name="url">Ссылка</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>HTTP-ответ от сервера</returns>
        private async Task<HttpResponseMessage> ExecutePostRequestAsync<T>(string url, T data,
            CancellationToken cancel = default)
        {
            var response = await _client
                .PostAsJsonAsync(url, data, cancel)
                .ConfigureAwait(false);

            // Сохранение данных в свойства
            LastCode = response.StatusCode;
            LastContent = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Выполнение PUT-запроса
        /// </summary>
        /// <typeparam name="T">Тип данных, которые отправляются на сервер</typeparam>
        /// <param name="url">Ссылка</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>HTTP-ответ от сервера</returns>
        private async Task<HttpResponseMessage> ExecutePutRequestAsync<T>(string url, T data,
            CancellationToken cancel = default)
        {
            var response = await _client
                .PutAsJsonAsync(url, data, cancel)
                .ConfigureAwait(false);

            // Сохранение данных в свойства
            LastCode = response.StatusCode;
            LastContent = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Выполнение DELETE-запроса
        /// </summary>
        /// <typeparam name="T">Тип данных, которые отправляются на сервер</typeparam>
        /// <param name="url">Ссылка</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>HTTP-ответ от сервера</returns>
        private async Task<HttpResponseMessage> ExecuteDeleteRequestAsync<T>(string url, T data,
            CancellationToken cancel = default)
        {
            // В обычном методе DeleteAsync() нельзя добавить сущность в запрос,
            // поэтому используется решение ниже
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            { Content = JsonContent.Create(data) };
            var response = await _client.SendAsync(request, cancel).ConfigureAwait(false);

            // Сохранение данных в свойства
            LastCode = response.StatusCode;
            LastContent = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);

            return response;
        }

        public async Task<HttpResponseMessage> GetRawAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task DeleteAsync<T>(string v)
        {
            throw new NotImplementedException();
        }
    }
}
