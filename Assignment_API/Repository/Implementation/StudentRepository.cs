using Assignment_API.DataContext;
using Assignment_API.DataModals;
using Assignment_API.Repository.Interface;

namespace Assignment_API.Repository.Implementation
{
    public class StudentRepository : GenericRepository<Student> , IStudentRepository
    {
        public StudentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
