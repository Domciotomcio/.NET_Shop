using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AW_lab10.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Too long name")]
        [MinLength(3, ErrorMessage = "To short name")]
        public string Name { get; set; }

        public List<Article> Articles { get; set; }

        public Category()
        {

        }

        public Category(int id, string name)
        {
            int Id = id;
            string Name = name;
        }
    }
}
