using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Challenge.Api.Models
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IFormFile> MyFile { get; set; }
    }
}