using MicroService.AggregateService.Models;
using MicroService.MemberService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.AggregateService.Services
{
    public class HttpMemberServiceClient : IMemeberServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpMemberServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<List<Member>> GetMembersByTeamId(Guid teamId)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            string url = $"http://localhost:5001/api/members?teamId="+$"{teamId}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(json);
            return members;
        }
    }
}
