using StudentTrackingSystem.Utility;

namespace StudentTrackingSystem.Models
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private ApplicationDbContext _applicationDbContext;
        public StudentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(Student student)
        {
            _applicationDbContext.Update(student);
        }

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
