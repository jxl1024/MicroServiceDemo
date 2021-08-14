using IdentityModel.Client;
using MicroService.MVCClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.MVCClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration; 

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            //#region token模式 
            //{
            //    // 1、生成AccessToken
            //    // 1.1 客户端模式
            //    string access_token = await GetAccessToken();

            //    // 2、使用AccessToken 进行资源访问
            //    string result = await UseAccessToken(access_token);

            //    // 3、响应结果到页面
            //    ViewData.Add("access_token", access_token);
            //    ViewData.Add("Json", result);
            //}
            //#endregion

            #region openid connect 协议
            {
                // 1、获取token(id-token , access_token ,refresh_token)
                var accessToken = await HttpContext.GetTokenAsync("access_token"); // ("id_token")
                Console.WriteLine($"accessToken:{accessToken}");
                // var refreshToken =await HttpContext.GetTokenAsync("refresh_token");
                var client = _httpClientFactory.CreateClient();
                // token添加到Header中 格式：Bearer token值
                client.SetBearerToken(accessToken);
                /*client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);*/

                // 2、使用token
                var result = await client.GetStringAsync("http://localhost:5000/api/teams");

                // 3、响应结果到页面
                ViewData.Add("access_token", accessToken);
                ViewData.Add("Json", result);
            }
            #endregion

            return View();
        }

        /// <summary>
        /// 1、生成token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            //1、创建HttpClient，通过工厂方式创建
            var client = _httpClientFactory.CreateClient();
            //2、 获取发现文档   获取各种配置信息 参数是基地址
            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(_configuration.GetSection("DiscoveryDocumentUrl").Value);
            if (disco.IsError)
            {
                Console.WriteLine($"[DiscoveryDocumentResponse Error]: {disco.Error}");
            }
            //// 1.1、通过客户端获取AccessToken
            //TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint, // 1、生成AccessToken中心
            //    ClientId = "simple_client", // 2、客户端编号
            //    ClientSecret = "simple_client_secret",// 3、客户端密码
            //    Scope = "TeamService" // 4、客户端需要访问的API
            //});

            // 1.2 通过客户端用户密码获取accesstoken
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "simple_pass_client",
                ClientSecret = "simple_client_secret",
                Scope = "TeamService",
                UserName = "admin",
                Password = "1234"
            });

            // 1.3 通过授权code获取AccessToken[需要进行登录]
            //TokenResponse tokenResponse = await client.RequestAuthorizationCodeTokenAsync
            //    (new AuthorizationCodeTokenRequest
            //    {
            //        Address = disco.TokenEndpoint,
            //        ClientId = "simple_code",
            //        ClientSecret = "simple_code_secret",
            //        Code = "12",
            //        RedirectUri = "http://localhost:5005"
            //    });

            if (tokenResponse.IsError)
            {
                //ClientId 与 ClientSecret 错误，报错：invalid_client
                //Scope 错误，报错：invalid_scope
                //UserName 与 Password 错误，报错：invalid_grant
                string errorDesc = tokenResponse.ErrorDescription;
                if (string.IsNullOrEmpty(errorDesc)) errorDesc = "";
                if (errorDesc.Equals("invalid_username_or_password"))
                {
                    Console.WriteLine("用户名或密码错误，请重新输入！");
                }
                else
                {
                    Console.WriteLine($"[TokenResponse Error]: {tokenResponse.Error}, [TokenResponse Error Description]: {errorDesc}");
                }

                // Console.WriteLine($"[TokenResponse Error]: {tokenResponse.Error}, [TokenResponse Error Description]: {tokenResponse.ErrorDescription}");
            }
            else
            {
                Console.WriteLine($"Access Token: {tokenResponse.Json}");
                Console.WriteLine($"Access Token: {tokenResponse.RefreshToken}");
                Console.WriteLine($"Access Token: {tokenResponse.ExpiresIn}");
            }
            return tokenResponse.AccessToken;
        }

        /// <summary>
        /// 2、使用token
        /// </summary>
        public  async Task<string> UseAccessToken(string AccessToken)
        {
            HttpClient apiClient = _httpClientFactory.CreateClient();
            // 1、设置token到请求头 格式：Bearer access_token
            apiClient.SetBearerToken(AccessToken);
            // 2、访问团队服务
            HttpResponseMessage response = await apiClient.GetAsync("http://localhost:5000/api/teams");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Request Error, StatusCode is : {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("");
                Console.WriteLine($"Result: {JArray.Parse(content)}");

                // 3、输出结果到页面
                return JArray.Parse(content).ToString();
            }
            return "";

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
