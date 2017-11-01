// <copyright file="DigiKeyClientTest.cs" company="Digi-Key Corporation">Copyright © Digi-Key Corporation 2017</copyright>
using System;
using DigiKey.Api;
using DigiKey.Api.Core;
using DigiKey.Api.Models;
using DigiKey.Api.OAuth2;
using DigiKey.Api.OAuth2.Models;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace DigiKey.Api.Tests
{
    /// <summary>This class contains parameterized unit tests for DigiKeyClient</summary>
    [PexClass(typeof(DigiKeyClient))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class DigiKeyClientTest
    {
        /// <summary>Test stub for .ctor(DigiKeyAppCredentials, OAuth2AccessToken, IDigiKeyInterceptor, Boolean, ITokenManager)</summary>
        [PexMethod]
        public DigiKeyClient ConstructorTest(
            DigiKeyAppCredentials credentials,
            OAuth2AccessToken accessToken,
            IDigiKeyInterceptor interceptor,
            bool enableOAuth2TokenRefresh,
            ITokenManager tokenManager
        )
        {
            DigiKeyClient target = new DigiKeyClient(credentials, accessToken, 
                                                     interceptor, enableOAuth2TokenRefresh, tokenManager);
            return target;
            // TODO: add assertions to method DigiKeyClientTest.ConstructorTest(DigiKeyAppCredentials, OAuth2AccessToken, IDigiKeyInterceptor, Boolean, ITokenManager)
        }
    }
}
