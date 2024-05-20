using Assignment_API.DataModals;
using Assignment_API.Models;
using Assignment_API.Models.Dto;
using Assignment_API.Repository.Interface;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace Assignment_API.Controllers
{
    [Route("StudentApi/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public StudentController(
            ICourseRepository courseRepository, IStudentRepository studentRepository,
            IMapper mapper)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            this._response = new();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetStudentsDataWithPagination([FromQuery] PaginationDTO paginationDTO)
        {
            try
            {
                Expression<Func<Student, bool>> whereCluaseSntex = PredicateBuilder.New<Student>();
                if (paginationDTO.searchQuery != null)
                {
                    whereCluaseSntex = x => x.FirstName.ToLower().Contains(paginationDTO.searchQuery) || x.LastName.ToLower().Contains(paginationDTO.searchQuery);
                }
                else
                {
                    whereCluaseSntex = x => x.StudentId != 0;
                }
                var dataTable = await _studentRepository.GetAllDataWithPagination(x => new StudentDataModelDto
                {
                    CourseId = x.CourseId,
                    Course = x.Course,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email ?? "",
                    FirstName = x.FirstName,
                    Gender = x.Gender,
                    StudentId = x.StudentId,
                    Grade = x.Grade ?? 0,
                    GradeName = x.Grade == 1 ? "1st-3rd" : x.Grade == 2 ? "4th-6th" : x.Grade == 3 ? "7th-8th" : "9th-12th",
                    GenderName = x.Gender == 1 ? "Male" : x.Gender == 2 ? "Female" : "Other",
                    LastName = x.LastName,
                }, whereCluaseSntex, paginationDTO.PageNumber, paginationDTO.PageSize, x => x.StudentId, paginationDTO.isAcending);
                List<StudentDataModelDto> students = new List<StudentDataModelDto>();
                foreach (StudentDataModelDto item in dataTable)
                {
                    students.Add(item);
                }
                _response.result = _mapper.Map<List<StudentDataModelDto>>(students);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet(Name = "GetStudentsDataWithoutPagination")]
        //public async Task<ActionResult<List<StudentDataModelDto>>> GetStudentsDataWithoutPagination()
        //{
        //    var dataTable = await _studentRepository.GetAllDataWithoutPagination(x => new StudentDataModelDto
        //    {
        //        CourseId = x.CourseId,
        //        CourseName = x.Course,
        //        DateOfBirth = x.DateOfBirth,
        //        Email = x.Email ?? "",
        //        FirstName = x.FirstName,
        //        GenderId = x.Gender,
        //        Id = x.StudentId,
        //        GradeId = (int)x.Grade,
        //        LastName = x.LastName,
        //    }, where);
        //    List<StudentDataModelDto> students = new List<StudentDataModelDto>();
        //    foreach (StudentDataModelDto item in dataTable)
        //    {
        //        students.Add(item);
        //    }
        //    return Ok(students);
        //}


        [HttpGet("{id:int}", Name = "GetStudentData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetStudentData(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var student = await _studentRepository.GetFirstOfDefault(x => x.StudentId == id);
                StudentDataModelDto studentDataModelDto = _mapper.Map<StudentDataModelDto>(student);
                if (student == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _response.result = studentDataModelDto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateStudent([FromBody] StudentCreateDto student)
        {
            try
            {

                if (student.FirstName != null && await _studentRepository.GetFirstOfDefault(x => x.FirstName.ToLower() == student.FirstName.ToLower()) != null)
                {
                    ModelState.AddModelError("", "Student Already Exist");
                    return BadRequest(ModelState);
                }
                if (student == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                Student student1 = new()
                {
                    FirstName = student.FirstName ?? "",
                    LastName = student.LastName ?? "",
                    DateOfBirth = student.DateOfBirth,
                    Age = DateTime.Now.Year - (student.DateOfBirth != null ? student.DateOfBirth.Value.Year : 0),
                    Email = student.Email,
                    Gender = student.GenderId,
                    Grade = student.GradeId,

                };
                Course? course = await _courseRepository.GetFirstOfDefault(x => x.Name == student.CourseName);
                if (course != null)
                {
                    student1.CourseId = course.Id;
                    student1.Course = course.Name;
                }
                else
                {
                    Course course1 = new()
                    {
                        Name = student.CourseName ?? "",
                    };
                    _ = _courseRepository.Add(course1);

                    student1.CourseId = course1.Id;
                    student1.Course = course1.Name;
                }
                _ = _studentRepository.Add(student1);
                _response.result = student1;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetStudentData", new { id = student1.StudentId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteStudentDetails(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var Student = await _studentRepository.GetFirstOfDefault(x => x.StudentId == id);
                if (Student == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _ = _studentRepository.Remove(Student);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateStudentDetails(int id, [FromBody] StudentUpdateDto studentDataModelDto)
        {
            try
            {

                if (id == 0 || studentDataModelDto.StudentId != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var student = await _studentRepository.GetFirstOfDefault(x => x.StudentId == id);
                if (student == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                student.FirstName = studentDataModelDto.FirstName ?? "";
                student.LastName = studentDataModelDto.LastName ?? "";
                student.DateOfBirth = studentDataModelDto.DateOfBirth;
                student.Age = DateTime.Now.Year - (studentDataModelDto.DateOfBirth != null ? studentDataModelDto.DateOfBirth.Value.Year : 0);
                student.Email = studentDataModelDto.Email;
                student.Gender = studentDataModelDto.GenderId;
                student.Grade = studentDataModelDto.GradeId;
                Course? course = await _courseRepository.GetFirstOfDefault(x => x.Name == studentDataModelDto.CourseName);
                if (course != null)
                {
                    student.CourseId = course.Id;
                    student.Course = course.Name;
                }
                else
                {
                    Course course1 = new()
                    {
                        Name = studentDataModelDto.CourseName ?? "",
                    };
                    _ = _courseRepository.Add(course1);

                    student.CourseId = course1.Id;
                    student.Course = course1.Name;
                }
                _ = _studentRepository.Update(student);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialStudentDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialStudentDetails(int id, JsonPatchDocument<StudentUpdateDto> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }

            var student = await _studentRepository.GetFirstOfDefault(x => x.StudentId == id);
            if (student == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }
            StudentUpdateDto studentDataModelDto = new()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                CourseName = student.Course ?? "",
                Email = student.Email ?? "",
                GenderId = student.Gender,
                GradeId = student.Grade ?? 0,
            };
            patchDto.ApplyTo(studentDataModelDto);
            Student student1 = new()
            {
                StudentId = studentDataModelDto.StudentId,
                FirstName = studentDataModelDto.FirstName,
                LastName = studentDataModelDto.LastName,
                DateOfBirth = studentDataModelDto.DateOfBirth,
                Age = DateTime.Now.Year - (studentDataModelDto.DateOfBirth != null ? studentDataModelDto.DateOfBirth.Value.Year : 0),
                Email = studentDataModelDto.Email,
                Gender = studentDataModelDto.GenderId,
                Grade = studentDataModelDto.GradeId
            };
            Course? course = await _courseRepository.GetFirstOfDefault(x => x.Name == studentDataModelDto.CourseName);
            if (course != null)
            {
                student1.CourseId = course.Id;
                student1.Course = course.Name;
            }
            else
            {
                Course course1 = new()
                {
                    Name = studentDataModelDto.CourseName,
                };
                _ = _courseRepository.Add(course1);
                student1.CourseId = course1.Id;
                student1.Course = course1.Name;
            }
            _ = _studentRepository.Update(student1);
            return NoContent();
        }
    }
}
