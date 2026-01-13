namespace FitnessMVC201.ViewModels.TrainerViewModels
{
    public class TrainerUpdateVm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile? ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
}
