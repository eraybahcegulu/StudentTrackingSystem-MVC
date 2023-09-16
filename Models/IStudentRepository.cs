namespace StudentTrackingSystem.Models
{
    public interface IStudentRepository : IRepository<Student>
    {
        void Update(Student student);
        void Save();
    }
}
