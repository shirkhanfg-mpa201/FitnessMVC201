using FitnessMVC201.Models.Common;

namespace FitnessMVC201.Models
{
    public class Trainer:BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
