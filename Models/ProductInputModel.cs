using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductInputModel
    {
        public int Id { get; set; } 

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
        public DateTime StartDate { get; set; } 
        public int Duration { get; set; }
        public IEnumerable<Category> Categories { get; set; }


    }
}
