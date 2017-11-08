using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DigiKey.Api.Core.Interfaces
{
    public interface IDigiKeyInterceptor
    {
        /// <summary>
        /// Called when we want to intercept a HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> OnRequest(HttpRequestMessage request,
                                            CancellationToken cancellationToken,
                                            DigiKeyClient client);

        /// <summary>
        /// Called when we want to intercept a HTTP request.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="invokingClient">The invoking client.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> OnResponse(Task<HttpResponseMessage> response,
                                             CancellationToken cancellationToken,
                                             DigiKeyClient invokingClient);
    }
}
