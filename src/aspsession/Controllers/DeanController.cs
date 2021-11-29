using aspsession.Contexts;
using aspsession.ViewModels.Dean;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspsession.Controllers;

/// <summary>
/// Контроллер сотрудника деканата
/// </summary>
[Authorize(Roles = "Сотрудник деканата")]
public class DeanController : Controller
{
    private MySessionContext _context;

    /// <summary>
    /// Конструктор класса <see cref="DeanController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public DeanController(MySessionContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Начальная страница
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Sheets", "Dean");
    }

    /// <summary>
    /// Страница ведомостей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Sheets()
    {
        var sheets = _context.Sheets.ToList();

        IList<SheetViewModel> model = null;

        if (sheets.Any())
        {
            // model = new List<SheetViewModel>();
            model = sheets.Select(sheet => new SheetViewModel 
            {
                Id = sheet.Id,
                Type = _context.SheetTypes.ToList().FirstOrDefault(type => type.Id == sheet.TypeId).Name,
                TermNumber = sheet.TermNumber,
                Year = sheet.Year,
                Group = _context.Groups.ToList().FirstOrDefault(group => group.Id == sheet.GroupId).Name,
                Discipline = _context.Disciplines.ToList().FirstOrDefault(disc => disc.Id == sheet.DisciplineId).Name,
                Teacher = _context.Teachers.ToList().FirstOrDefault(teacher => teacher.Id == sheet.TeacherId).Name,
            }).ToList();
        }

        return View(model);
    }

    /// <summary>
    /// Страница преподавателей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Teachers()
    {
        var teachers = _context.Teachers.ToList();

        var model = teachers.Select(teacher => new TeacherViewModel
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Departments = UniversityHierarchyByDepartmentId(teacher.DepartmentsId).Departments,
            Institute = UniversityHierarchyByDepartmentId(teacher.DepartmentsId).Institute,
        }).ToList();

        return View(model);
    }

    /// <summary>
    /// Страница студентов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Students()
    {
        var students = _context.Students.ToList();

        var model = students.Select(student => new StudentViewModel 
        { 
            Id = student.Id,
            BookNumber = student.BookNumber,
            Name = student.Name,
            Group = UniversityHierarchyByGroupId(student.GroupId).Group,
            Departments = UniversityHierarchyByGroupId(student.GroupId).Departments,
            Institute = UniversityHierarchyByGroupId(student.GroupId).Institute,
        }).ToList();

        return View(model);
    }

    private (string Group, string Departments, string Institute) UniversityHierarchyByGroupId(int groupId)
    {
        var group = _context.Groups.ToList().FirstOrDefault(group => group.Id == groupId);
        var departments = _context.Departments.ToList().FirstOrDefault(dep => dep.Id == group.DepartmentsId);
        var institute = _context.Institutes.ToList().FirstOrDefault(inst => inst.Id == departments.InstituteId);

        return (group.Name, departments.Name, institute.Name);
    }

    private (string Departments, string Institute) UniversityHierarchyByDepartmentId(int departmentId)
    {
        var departments = _context.Departments.ToList().FirstOrDefault(dep => dep.Id == departmentId);
        var institute = _context.Institutes.ToList().FirstOrDefault(inst => inst.Id == departments.InstituteId);

        return (departments.Name, institute.Name);
    }
}