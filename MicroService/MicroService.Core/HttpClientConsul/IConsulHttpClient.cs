using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.HttpClientConsul
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string Serviceshcme, string ServiceName, string serviceLink);
    }
}
