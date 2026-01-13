using FitnessMVC201.Models.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FitnessMVC201.Models
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Trainer> Trainer { get; set; }
    }
}
