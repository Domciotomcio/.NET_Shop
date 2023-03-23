using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AW_lab10.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Too long name")]
        [MinLength(3, ErrorMessage = "To short name")]
        public String Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public Double Price { get; set; }

        public string? Image { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        [NotMapped]
        public string ImagePath => Image is { } path ? $"/Images/{path}" : "/Images/no-image-icon.png";
        public override string ToString()
        {
            return "Article: " + Id + " , Name: " + Name + " , Price: " + " , Category: " + Category;
        }

        public void converter(Article article)
        {
            Name = article.Name;
            Price = article.Price;
            Category = article.Category;
        }

    }
}
