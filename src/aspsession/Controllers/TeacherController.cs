using aspsession.Contexts;
using aspsession.ViewModels.Dean;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspsession.Controllers;

/// <summary>
/// Контроллер преподавателя
/// </summary>
[Authorize(Roles = "Преподаватель")]
public class TeacherController : Controller
{
    private MySessionContext _context;

    /// <summary>
    /// Конструктор класса <see cref="TeacherController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public TeacherController(MySessionContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Sheets");
    }

    /// <summary>
    /// Страница с ведомостями преподавателя
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Sheets()
    {
        var user = _context.Users.ToList().First(x => x.Email == User.Identity.Name);
        var teacher = _context.Teachers.ToList().First(x => x.Name == user.Name);
        var sheets = _context.Sheets.Where(sheet => sheet.TeacherId == teacher.Id).ToList();

        IList<SheetViewModel> model = null;

        if (sheets.Any())
        {
            var statuses = _context.SheetStatuses.AsEnumerable().ToDictionary(status => status.Id, status => status.Name);
            var histories = _context.SheetHistories.AsEnumerable()
                .GroupBy(x => x.SheetId)
                .ToDictionary(x => x.Key, x => x.ToList());

            model = sheets.Select(sheet => new SheetViewModel
            {
                Id = sheet.Id,
                Type = _context.SheetTypes.ToList().FirstOrDefault(type => type.Id == sheet.TypeId).Name,
                Term = sheet.TermNumber == 1 ? "Осенний" : "Весенний",
                Year = sheet.Year,
                Group = UniversityHierarchyByGroupId(sheet.GroupId).Group,
                Discipline = _context.Disciplines.ToList().FirstOrDefault(disc => disc.Id == sheet.DisciplineId).Name,
                Status = statuses[histories[sheet.Id].Last().StatusId]
            }).ToList();
        }

        return View(model);
    }

    private string GetGroupNameById(int groupId)
    {
        var group = _context.Groups.ToList().FirstOrDefault(group => group.Id == groupId);
        var direction = _context.Directions.ToList().FirstOrDefault(direc => direc.Id == group.DirectionId);
        string groupName = $"{direction.ShortName}-{group.Course}1";

        return groupName;
    }

    private (string Group, string Departments, string Institute) UniversityHierarchyByGroupId(int groupId)
    {
        var group = _context.Groups.ToList().FirstOrDefault(group => group.Id == groupId);
        var direction = _context.Directions.ToList().FirstOrDefault(direc => direc.Id == group.DirectionId);
        var departments = _context.Departments.ToList().FirstOrDefault(dep => dep.Id == direction.DepartmentId);
        var institute = _context.Institutes.ToList().FirstOrDefault(inst => inst.Id == departments.InstituteId);

        string groupName = $"{direction.ShortName}-{group.Course}1";
        return (groupName, departments.Name, institute.Name);
    }
}