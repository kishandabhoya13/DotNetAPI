using Assignment.Models.Dto;
using Assignment.Services.Interface;
using Assignment_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentServices;
        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }
        public IActionResult Index()
        {
            StudentDataSecondDTO studentDataSecondDTO = new StudentDataSecondDTO();
            return View(studentDataSecondDTO);
        }

        [HttpGet]
        public async Task<IActionResult> FetchStudentData(PaginationDTO filter)
        {
            List<StudentDataModelDto> list = new();
            APIResponse response = new();
            StudentDataSecondDTO studentDataSecondDTO = new();
            try
            {
                response = await _studentServices.GetAllAsync<APIResponse>(filter);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
            finally
            {
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<StudentDataModelDto>>(Convert.ToString(response.result) ?? "") ?? new();
                }
                studentDataSecondDTO.Students = list;
                studentDataSecondDTO.ItemsPerPage = filter.PageSize;
                studentDataSecondDTO.PageNumber = filter.PageNumber;
            }
            return PartialView("_StudentListPartialView", studentDataSecondDTO);
        }

        public async Task<IActionResult> AddEditStudentModal(int? StudentId)
        {
            StudentDataModelDto studentDataViewModel = new()
            {
                genders = new List<DropdownViewModel>
                {
                    new DropdownViewModel {Id = 1, Name = "Male"},
                    new DropdownViewModel {Id = 2, Name = "Female"},
                    new DropdownViewModel {Id = 3, Name = "Other"}
                }
            };
            if (StudentId != null && StudentId != 0)
            {
                var studentresponse = await _studentServices.GetAsync<APIResponse>(StudentId ?? 0);
                if (studentresponse != null && studentresponse.IsSuccess)
                {
                    studentDataViewModel = JsonConvert.DeserializeObject<StudentDataModelDto>(Convert.ToString(studentresponse.result) ?? "") ?? new();
                }
                studentDataViewModel.genders = new List<DropdownViewModel>{
                    new DropdownViewModel {Id = 1, Name = "Male"},
                    new DropdownViewModel {Id = 2, Name = "Female"},
                    new DropdownViewModel {Id = 3, Name = "Other"}
                };
            }
            return View(studentDataViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertStudentData(StudentDataModelDto studentDataModelDto)
        {
            StudentCreateDto studentCreateDto = new()
            {
                CourseName = studentDataModelDto.Course,
                DateOfBirth = studentDataModelDto.DateOfBirth,
                Email = studentDataModelDto.Email,
                FirstName = studentDataModelDto.FirstName,
                LastName = studentDataModelDto.LastName,
                GenderId = studentDataModelDto.Gender,
                GradeId = studentDataModelDto.Grade
            };

            StudentUpdateDto studentUpdateDto = new()
            {
                StudentId = studentDataModelDto.StudentId,
                CourseName = studentDataModelDto.Course,
                DateOfBirth = studentDataModelDto.DateOfBirth,
                Email = studentDataModelDto.Email,
                FirstName = studentDataModelDto.FirstName,
                LastName = studentDataModelDto.LastName,
                GenderId = studentDataModelDto.Gender,
                GradeId = studentDataModelDto.Grade
            };
            if (studentDataModelDto.StudentId != 0)
            {
                var response = await _studentServices.UpdateAsync<APIResponse>(studentUpdateDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = "Update Record Successfully";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("AddEditStudentModal");
            }
            else
            {
                var response = await _studentServices.CreateAsync<APIResponse>(studentCreateDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = "Add Record Successfully";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("AddEditStudentModal");
            }
        }

        public async Task<IActionResult> DeleteRecord(int? id)
        {
            if (id != null && id > 0)
            {
                var response = await _studentServices.DeleteAsync<APIResponse>(id ?? 0);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = "Delete Record Successfully";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
