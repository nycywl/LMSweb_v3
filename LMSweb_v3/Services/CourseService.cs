using LMSweb_v3.ViewModels;
using LMSweb_v3.ViewModels.Course;
using LMSwebDB.Models;
using LMSwebDB.Repositories;

namespace LMSweb_v3.Services
{
    public class CourseService
    {
        private readonly LMSRepository _repository;

        public CourseService(LMSRepository repository)
        {
            _repository = repository;
        }

        public List<CourseViewModel> GetCourses(string teacherId)
        {
            return _repository.Query<Course>()
                .Where(c => c.TeacherId == teacherId)
                .Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName
                })
                .ToList();
        }

        public string CreateCourse(string teacherId, string courseName)
        {
            var course = new Course
            {
                CourseId = Guid.NewGuid().ToString(),
                TeacherId = teacherId,
                CourseName = courseName,
                CreateTime = DateTime.Now
            };
            _repository.Create(course);
            _repository.SaveChanges();
            return course.CourseId;
        }

        public CourseEditViewModel GetCourseEditViewModel(string courseId)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == courseId);
            if (course == null) return null;

            return new CourseEditViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                TeacherId = course.TeacherId,
                SystemPrompt = course.SystemPrompt,
                UserPrompt = course.UserPrompt,
                GreetingMessage = course.GreetingMessage,
                Temperature = course.Temperature,
                IsNeedContext = course.IsNeedContext,
                LLMModel = course.LLMModel
            };
        }

        public void EditCourse(string courseId, CourseEditViewModel model)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == courseId);
            if (course == null) return;

            course.CourseName = model.CourseName;
            course.SystemPrompt = model.SystemPrompt;
            course.UserPrompt = model.UserPrompt;
            course.GreetingMessage = model.GreetingMessage;
            course.Temperature = model.Temperature;
            course.IsNeedContext = model.IsNeedContext;
            course.LLMModel = model.LLMModel;

            _repository.Update(course);
            _repository.SaveChanges();
        }

        public CourseDeleteViewModel GetWillBeDeleteCourse(string courseId, string teacherId)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == courseId && c.TeacherId == teacherId);
            if (course == null) return null;

            return new CourseDeleteViewModel
            {
                CourseID = course.CourseId,
                CourseName = course.CourseName
            };
        }

        public void DeleteCourse(string courseId)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == courseId);
            if (course == null) return;

            _repository.Delete(course);
            _repository.SaveChanges();
        }

        public PromptManageViewModel GetCourseDefaultPrompt(string courseId)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == courseId);
            if (course == null) return null;

            return new PromptManageViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                SystemPrompt = course.SystemPrompt,
                UserPrompt = course.UserPrompt,
                Greeting = course.GreetingMessage,
                Temperature = course.Temperature,
                IsNeedContext = course.IsNeedContext,
                LLMModel = course.LLMModel
            };
        }

        public bool UpdateDefaultPrompt(PromptManageViewModel model)
        {
            var course = _repository.Query<Course>().FirstOrDefault(c => c.CourseId == model.CourseId);
            if (course == null) return false;

            course.SystemPrompt = model.SystemPrompt;
            course.UserPrompt = model.UserPrompt;
            course.GreetingMessage = model.Greeting;
            course.Temperature = model.Temperature;
            course.IsNeedContext = model.IsNeedContext;
            course.LLMModel = model.LLMModel;

            _repository.Update(course);
            _repository.SaveChanges();
            return true;
        }
    }
}
