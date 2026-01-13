namespace FitnessMVC201.ViewModels.TrainerViewModels
{
    public class TrainerCreateVm
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile ImageUrl { get; set; }

        public int CategoryId { get; set; }

    }
}
