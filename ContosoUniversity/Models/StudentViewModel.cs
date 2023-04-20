using ContosoUniversity.Entities;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class StudentViewModel : Entity
    {
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string FirstMidName { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }

        readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();


        [DataType(DataType.Upload)]
        [Display(Name = "LabelPhoto", Prompt = "LabelPhotoPrompt", ResourceType = typeof(Properties.Resources))]
        public IFormFile Photo { get; set; }

    }
}
