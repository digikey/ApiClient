using System.Collections.Generic;
using System.Net.Http;
using DigiKey.Api.Core;

namespace DigiKey.Api.Extensions
{
    public static class DigikeyClientExtensions
    {
        public static HttpMessageHandler CreatePipeline(this DigiKeyClient client,
                                                        List<IDigiKeyInterceptor> interceptors)
        {
            if (interceptors.Count <= 0)
            {
                return null;
            }

            // inspired by the code referenced from the web api source; this creates the russian doll effect
            var innerHandler = new DigiKeyHttpMessageHandler(client, interceptors[0]);
            var innerHandlers = interceptors.GetRange(1, interceptors.Count - 1);

            foreach (var handler in innerHandlers)
            {
                var messageHandler = new DigiKeyHttpMessageHandler(client, handler) {InnerHandler = innerHandler};
                innerHandler = messageHandler;
            }
            return innerHandler;
        }
    }
}
