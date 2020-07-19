using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using QRTest.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace QRTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        public UploadFileController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly IConfiguration Configuration;
        [RequestSizeLimit(1048576)]
        public async Task<List<UploadFileResponse>> Upload([FromForm]IFormFile file)
        {
            string url = Configuration["RemoteServiceURL"];
            byte[] fileBytes;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    client.BaseAddress = new Uri(url);
                    using (var content = new MultipartFormDataContent())
                    {
                        var imageContent = new ByteArrayContent(fileBytes);
                        content.Add(imageContent, file.Name, file.FileName);
                        var responseMessage = await client.PostAsync(url, content);
                        var response = await responseMessage.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<UploadFileResponse>>(response);
                    }
                }
            }
            catch(Exception ex)
            {
                return new List<UploadFileResponse>
                { 
                    new UploadFileResponse
                    {
                        Symbol = new List<UploadFileResponse.ResponseSymbol>
                        {
                            new UploadFileResponse.ResponseSymbol
                            {
                                Error ="Server Error"
                            }
                        }
                    }                    
                };
            }
        }        
    }
}
