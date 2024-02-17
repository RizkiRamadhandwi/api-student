using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    // api/students
    public class StudentController:ControllerBase
    {
        private readonly AppDBContext _context;
        public StudentController(AppDBContext context)
        {
            _context=context;
        }


        [HttpGet]
        public async Task<IEnumerable<Student>> ListStudents()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();
            return students; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(student);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }

            return BadRequest("unable to created data");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if(student is null) return NotFound();
            

            return Ok(student);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(int id,Student student)
        {
            var studentFromDB = await _context.Students.FindAsync(id);
            if(studentFromDB is null) return NotFound("student not found");

            studentFromDB.Name = student.Name;
            studentFromDB.Email = student.Email;
            studentFromDB.Address = student.Address;
            studentFromDB.PhoneNumber = student.PhoneNumber;

            var result = await _context.SaveChangesAsync();

            if (result > 0) return Ok("Student update successfully");
            
            return BadRequest("unable to updated data");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if(student is null) return NotFound();
            

            _context.Remove(student);
            var result = await _context.SaveChangesAsync();
            if(result > 0) return Ok("student deleted");

            return NotFound("Student Not Found");
        }
    }
}