using System;

namespace Challenge.Api.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int price { get; set; }
        public string FileImageName { get; set; }
        public string ImageFullPath => String.IsNullOrEmpty(FileImageName)
         ? null
         : $"https://localhost:5001{FileImageName.Substring(1)}";
    }
}