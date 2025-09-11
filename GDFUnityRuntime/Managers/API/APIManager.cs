#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFUnityShared.csproj APIManager.cs create at 2025/03/26 12:03:56
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using GDFFoundation;
using Newtonsoft.Json;

#endregion

namespace GDFUnity
{
    /// <summary>
    ///     Abstract class of any API call.
    /// </summary>
    public abstract class APIManager : AsyncManager
    {
        #region Instance methods

        /// <summary>
        ///     Convert the body of the response.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="responseBody">The body of the response.</param>
        /// <returns>The data of the response.</returns>
        private RES ConvertResponse<RES>(string responseBody)
        {
            try
            {
                return JsonConvert.DeserializeObject<RES>(responseBody);
            }
            catch (Exception e)
            {
                GDFLogger.Warning($"Could not convert HTTP Body to type: {typeof(RES).FullName}! Returned default value");
                GDFLogger.Error(e);
            }

            return default;
        }

        /// <summary>
        ///     Read the error in the body of the response.
        /// </summary>
        /// <param name="responseBody">The body of the response.</param>
        /// <returns>The found error.</returns>
        private Exception ConvertToError(string responseBody)
        {
            try
            {
                ApiErrorMessage body = JsonConvert.DeserializeObject<ApiErrorMessage>(responseBody);
                return new APIException(body);
            }
            catch
            {
                return Exceptions.InvalidBody;
            }
        }

        /// <summary>
        ///     Send a HTTP DELETE request.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <returns>The recieved data.</returns>
        protected RES Delete<RES>(IJobHandler handler, string url, Dictionary<string, string> headers = null)
        {
            return Send<RES>(handler, url, HttpMethod.Delete, headers, null);
        }

        /// <summary>
        ///     Send a HTTP GET request.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <returns>The recieved data.</returns>
        protected RES Get<RES>(IJobHandler handler, string url, Dictionary<string, string> headers = null)
        {
            return Send<RES>(handler, url, HttpMethod.Get, headers, null);
        }

        /// <summary>
        ///     Send a HTTP POST request.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <param name="payload">The payload of the request.</param>
        /// <returns>The recieved data.</returns>
        protected RES Post<RES>(IJobHandler handler, string url, Dictionary<string, string> headers = null, object payload = null)
        {
            return Send<RES>(handler, url, HttpMethod.Post, headers, payload);
        }

        /// <summary>
        ///     Send a HTTP PUT request.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <param name="payload">The payload of the request.</param>
        /// <returns>The recieved data.</returns>
        protected RES Put<RES>(IJobHandler handler, string url, Dictionary<string, string> headers = null, object payload = null)
        {
            return Send<RES>(handler, url, HttpMethod.Put, headers, payload);
        }

        /// <summary>
        ///     Read the body of the response.
        /// </summary>
        /// <param name="response">The response to read from.</param>
        /// <returns>The body of the response as a string.</returns>
        private string ReadResponseBody(HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentLength == 0)
            {
                return "";
            }

            using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Send a HTTP request.
        /// </summary>
        /// <typeparam name="RES">The type of the response body.</typeparam>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <param name="payload">The payload of the request.</param>
        /// <returns>The recieved data.</returns>
        protected RES Send<RES>(IJobHandler handler, string url, HttpMethod method, Dictionary<string, string> headers, object payload)
        {
            string response = SendRequest(handler, url, method, headers, payload);
            handler.Step();

            if (string.IsNullOrEmpty(response))
            {
                return default;
            }

            return ConvertResponse<RES>(response);
        }

        /// <summary>
        ///     Get the HTTP Response of the request.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="url">The full URL of the request.</param>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <param name="payload">The payload of the request.</param>
        /// <returns>The HTTP response.</returns>
        private string SendRequest(IJobHandler handler, string url, HttpMethod method, Dictionary<string, string> headers, object payload)
        {
            handler.StepAmount = 6;
            using HttpClient client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
            handler.Step();
            using HttpRequestMessage request = new HttpRequestMessage(method, url);
            handler.Step();
            SetHeaders(request, headers);
            if (payload != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            }

            handler.Step();

            using HttpResponseMessage response = client.SendAsync(request).Result;
            handler.Step();

            string responseBody = ReadResponseBody(response);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.ContentLength == 0)
                {
                    throw Exceptions.InvalidStatusCode(response);
                }

                throw ConvertToError(responseBody);
            }

            return responseBody;
        }

        /// <summary>
        ///     Set the headers of the request.
        /// </summary>
        /// <param name="request">The request to set the headers from.</param>
        /// <param name="headers">The headers to set.</param>
        private void SetHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<string, string> header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        #endregion

        #region Nested type: Exceptions

        static private class Exceptions
        {
            #region Static fields and properties

            static public APIException InvalidBody => new APIException(HttpStatusCode.InternalServerError, "API", 2, "Could not deserialize error from server");

            #endregion

            #region Static methods

            static public APIException InvalidStatusCode(HttpResponseMessage response) => new APIException(response.StatusCode, "API", 1, $"Server respondeded with invalid status code: {response.StatusCode} !");

            #endregion
        }

        #endregion
    }
}