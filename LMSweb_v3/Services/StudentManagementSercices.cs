using LMSweb_v3.ViewModels.StudentManagement;
using LMSweb_v3.ViewModels;
using LMSwebDB.Models;
using LMSwebDB.Repositories;

namespace LMSweb_v3.Services;

public class StudentManagementSercices
{
    private readonly LMSRepository _repository;

    public StudentManagementSercices(LMSRepository repository)
    {
        _repository = repository;
    }

    public void AddStudents(List<EnrolledStudent> students, string courseId)
    {
        var studentCourses = new List<StudentCourse>();
        foreach (var student in students)
        {
            // 確保學生在 Students 表中已經存在或創建新學生
            var existingStudent = _repository.Query<Student>().FirstOrDefault(s => s.StudentId == student.StudentId);
            if (existingStudent == null)
            {
                var newStudent = new Student
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    Gender = student.StudentSex
                };
                _repository.Create(newStudent);
            }
            studentCourses.Add(new StudentCourse
            {
                StudentId = student.StudentId,
                CourseId = courseId
            });
        }

        _repository.CreateMany(studentCourses);
        _repository.SaveChanges();
    }

    public StudentManagementViewModel? GetStudents(string courseId)
    {
        var course = _repository.Query<Course>()
            .Where(c => c.CourseId == courseId)
            .Select(c => new StudentManagementViewModel
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Students = c.StudentCourses.Select(sc => new EnrolledStudent
                {
                    StudentId = sc.Student.StudentId,
                    StudentName = sc.Student.StudentName,
                    StudentSex = sc.Student.Gender
                }).ToList()
            }).FirstOrDefault();

        return course;
    }

    public StudentEditViewModel? GetStudent(string courseId, string studentId)
    {
        var student = _repository.Query<StudentCourse>()
            .Where(sc => sc.CourseId == courseId && sc.StudentId == studentId)
            .Select(sc => new StudentEditViewModel
            {
                CourseId = sc.CourseId,
                StudentId = sc.Student.StudentId,
                StudentName = sc.Student.StudentName,
                StudentSex = sc.Student.Gender
            }).FirstOrDefault();

        return student;
    }

    public bool IsStudentExist(string studentId)
    {
        return _repository.Query<Student>().Any(s => s.StudentId == studentId);
    }

    public void UpdateStudent(StudentEditViewModel model)
    {
        var student = _repository.Query<Student>().FirstOrDefault(s => s.StudentId == model.StudentId);
        if (student != null)
        {
            student.StudentName = model.StudentName;
            student.Gender = model.StudentSex;
            _repository.Update(student);
            _repository.SaveChanges();
        }
    }

    public void DeleteStudent(string studentId, string courseId)
    {
        var studentCourse = _repository.Query<StudentCourse>()
            .FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        if (studentCourse != null)
        {
            _repository.Delete(studentCourse);
            _repository.SaveChanges();
        }
    }

    public StudentHomeViewModel? GetCourseForStudent(string studentId)
    {
        var course = _repository.Query<StudentCourse>()
            .Where(sc => sc.StudentId == studentId)
            .Select(sc => new StudentHomeViewModel
            {
                CourseID = sc.Course.CourseId,
                CourseName = sc.Course.CourseName,
                TeacherName = sc.Course.Teacher.TeacherName
            }).FirstOrDefault();

        return course;
    }
}
