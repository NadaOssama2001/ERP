using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        public DateTime StartDate { get; set; } 
        public int Duration { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public Category Categories { get; set; }



    }
}
