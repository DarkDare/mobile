﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bit.App.Abstractions;
using Bit.App.Models.Api;
using Plugin.Connectivity.Abstractions;

namespace Bit.App.Repositories
{
    public class AccountsApiRepository : BaseApiRepository, IAccountsApiRepository
    {
        public AccountsApiRepository(IConnectivity connectivity)
            : base(connectivity)
        { }

        protected override string ApiRoute => "accounts";

        public virtual async Task<ApiResult> PostRegisterAsync(RegisterRequest requestObj)
        {
            if(!Connectivity.IsConnected)
            {
                return HandledNotConnected();
            }

            using(var client = new ApiHttpClient())
            {
                var requestMessage = new TokenHttpRequestMessage(requestObj)
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(client.BaseAddress, string.Concat(ApiRoute, "/register")),
                };

                var response = await client.SendAsync(requestMessage);
                if(!response.IsSuccessStatusCode)
                {
                    return await HandleErrorAsync(response);
                }

                return ApiResult.Success(response.StatusCode);
            }
        }
    }
}