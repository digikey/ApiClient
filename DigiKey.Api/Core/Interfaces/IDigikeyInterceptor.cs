using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DigiKey.Api.Core
{
    public interface IDigiKeyInterceptor
    {
        Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request,
                                                   CancellationToken cancellationToken,
                                                   DigiKeyClient client);

        Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response,
                                                    CancellationToken cancellationToken,
                                                    DigiKeyClient invokingClient);
    }
}
