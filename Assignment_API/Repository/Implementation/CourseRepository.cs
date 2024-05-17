using Assignment_API.DataContext;
using Assignment_API.DataModals;
using Assignment_API.Repository.Interface;

namespace Assignment_API.Repository.Implementation
{
    public class CourseRepository : GenericRepository<Course> , ICourseRepository
    {
        public CourseRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
