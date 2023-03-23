using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;


namespace AW_lab10.ViewModels
{
    public class ArticleViewModelCreate
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "To short name")]
        [MaxLength(255, ErrorMessage = " To long name, do not exceed")]
        public string Name { get; set; }
        [Required]
        [Range(0, 1000000)]
        public double Price { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile? FormFile { get; set; }
    }
}
