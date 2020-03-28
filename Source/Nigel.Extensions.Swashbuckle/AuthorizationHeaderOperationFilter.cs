using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Nigel.Extensions.Swashbuckle
{
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        private readonly IHttpContextAccessor _iHttpContextAccessor;

        public AuthorizationHeaderOperationFilter(IHttpContextAccessor iHttpContextAccessor)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            List<string> lstIPAddress = new List<string>();
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipa in IpEntry.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    lstIPAddress.Add(ipa.ToString());
                }
            }
        }
    }
}