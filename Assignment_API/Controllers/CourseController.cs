using Assignment_API.DataModals;
using Assignment_API.Models.Dto;
using Assignment_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Assignment_API.Repository.Interface;
using AutoMapper;
using Assignment_API.Repository.Implementation;

namespace Assignment_API.Controllers
{
    [Route("StudentApi/Course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public CourseController(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllCourses()
        {
            try
            {

                IEnumerable<Course> courses = await _courseRepository.GetAll();
                _response.result = _mapper.Map<List<CourseModelDTO>>(courses);
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

        [HttpGet("{id:int}", Name = "GetSpecificCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSpecificCourse(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var course = await _courseRepository.GetFirstOfDefault(x => x.Id == id);
                if (course == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _response.result = course;
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
        public async Task<ActionResult<APIResponse>> CreateCourse([FromBody] CourseCreateDto courseCreateDto)
        {
            try
            {

                if (await _courseRepository.GetFirstOfDefault(x => x.Name.ToLower() == courseCreateDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("", "Student Already Exist");
                    return BadRequest(ModelState);
                }
                if (courseCreateDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                Course course = _mapper.Map<Course>(courseCreateDto);
                _ = _courseRepository.Add(course);
                _response.result = course;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetSpecificCourse", new { id = course.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErroMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteCourse")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteCourse(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var course = await _courseRepository.GetFirstOfDefault(x => x.Id == id);
                if (course == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _ = _courseRepository.Remove(course);
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
        public async Task<ActionResult<APIResponse>> UpdateCourse(int id, [FromBody] CourseUpdateDto courseUpdateDto)
        {
            try
            {

                if (id == 0 || courseUpdateDto.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var course = await _courseRepository.GetFirstOfDefault(x => x.Id == id);
                if (course == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                course = _mapper.Map<Course>(courseUpdateDto);
                _ =_courseRepository.Update(course);
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
    }
}
