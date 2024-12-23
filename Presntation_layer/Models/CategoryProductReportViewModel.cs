using DTOs;

namespace Presntation_layer.Models
{
    public class CategoryProductReportViewModel
    {
        public List<CategoryDTO> Categories { get; set; }
        public List<ProductDTO> Products { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
