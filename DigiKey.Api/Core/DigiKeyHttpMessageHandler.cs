using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DigiKey.Api.Core.Interfaces;

namespace DigiKey.Api.Core
{
    /// <summary>
    ///     Message Handler for HttpClient to intercept Http post and check if Authorization failed
    ///     and if it failed attempt to refresh the AccessToken and try one more time.
    /// </summary>
    public class DigiKeyHttpMessageHandler : DelegatingHandler
    {
        private readonly IDigiKeyInterceptor _interceptor;

        public DigiKeyClient DigiKeyClient { get; private set; }

        public DigiKeyHttpMessageHandler(DigiKeyClient digiKeyClient, IDigiKeyInterceptor interceptor)
        {
            DigiKeyClient = digiKeyClient;
            _interceptor = interceptor;

            // Examples say this is needed? 
            InnerHandler = new HttpClientHandler();
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL requests made by DigiKeyClient
        // We override the SendAsync method to intercept both the request and response path

        /// <summary>
        ///     This handler is invoked by all request made by DigiKeyClient, this is done by overriding the SendAsync
        ///     to intercept the request and response
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>HttpResponseMessage</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> response = null;
            Debug.WriteLine("DigikeyHttpMessageHandler::SendAsync-> Request details: {0}", request.ToString());

            if (_interceptor != null)
            {
                response = _interceptor.OnRequest(request, cancellationToken, DigiKeyClient);
            }

            // Then highjack the request pipeline and return the HttpResponse returned by interceptor. 
            // invoke Response handler at return.
            if (response != null)
            {
                //If we are faking the response, have the courtesy of setting the original HttpRequestMessage
                response.Result.RequestMessage = request;
                return response.ContinueWith(
                    responseTask => ResponseHandler(responseTask, cancellationToken).Result
                );
            }
            else //Let the base object continue with the request pipeline. Invoke Response handler at return.
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(
                    responseTask => ResponseHandler(responseTask, cancellationToken).Result
                );
            }
        }

        /// <summary>
        ///     Intercept the response
        /// </summary>
        /// <param name="responseTask">The response task.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> ResponseHandler(Task<HttpResponseMessage> responseTask,
                                                                CancellationToken cancellationToken)
        {
            DebugLogResponse(responseTask);

            if (_interceptor == null)
            {
                return await responseTask;
            }

            var response = await _interceptor.OnResponse(responseTask, cancellationToken, DigiKeyClient);

            //then highjack the request pipeline and return the HttpResponse returned by interceptor. Invoke Response handler at return.
            if (response != null)
            {
                //If we are faking the response, have the courtesy of setting the original HttpRequestMessage
                response.RequestMessage = (await responseTask).RequestMessage;
                return response;
            }
            return await responseTask;
        }

        [Conditional("DEBUG")]
        private static void DebugLogResponse(Task<HttpResponseMessage> requestTask)
        {
            string responseContent = null;

            if (requestTask.Result.Content != null)
            {
                responseContent = requestTask.Result.Content.ReadAsStringAsync().Result;
            }

            Debug.WriteLine("Entering Http client's response message handler. Response details: \n {0}",
                            requestTask.Result);
            Debug.WriteLine("Response Content: \n {0}", responseContent ?? "Response body was empty");
        }
    }
}
