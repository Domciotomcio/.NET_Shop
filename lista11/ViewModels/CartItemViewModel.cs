using AW_lab10.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AW_lab10.ViewModels
{
    public class CartItemViewModel
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int Count { get; set; }
    }
}
