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

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Sheets", "Dean");
    }

    [HttpGet]
    public IActionResult Sheets()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Teachers()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Students()
    {
        var students = _context.Students.ToList();

        var model = students.Select(student => new StudentViewModel 
        { 
            Id = student.Id,
            BookNumber = student.BookNumber,
            Name = student.Name,
            Group = UniversityHierarchyByGroup(student.GroupId).Group,
            Departments = UniversityHierarchyByGroup(student.GroupId).Departments,
            Institute = UniversityHierarchyByGroup(student.GroupId).Institute,
        }).ToList();

        return View(model);
    }

    private (string Group, string Departments, string Institute) UniversityHierarchyByGroup(int groupId)
    {
        var group = _context.Groups.ToList().FirstOrDefault(group => group.Id == groupId);
        var departments = _context.Departments.ToList().FirstOrDefault(dep => dep.Id == group.DepartmentsId);
        var institute = _context.Institutes.ToList().FirstOrDefault(inst => inst.Id == departments.InstituteId);

        return (group.Name, departments.Name, institute.Name);
    }
}