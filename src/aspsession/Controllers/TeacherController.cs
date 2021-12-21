using aspsession.Contexts;
using aspsession.Models;
using aspsession.ViewModels.Shared;
using aspsession.ViewModels.Teacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            var fixmodel = model.ToList();
            fixmodel.RemoveAll(x => x.Status == "Создана");
            model = fixmodel;
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ConfirmSheets(string forms)
    {
        var sheets = JsonConvert.DeserializeObject<IList<SheetViewModel>>(forms);
        var user = _context.Users.ToList().First(x => x.Email == User.Identity.Name);
        var teacher = _context.Teachers.ToList().First(x => x.Name == user.Name);

        foreach (var sheet in sheets)
        {
            var new_history = new SheetHistory 
            {
                SheetId = sheet.Id,
                StatusId = 3, // Статус - Получение подтверждено
                UserId = user.Id,
                DateCreated = DateTime.Now.ToString("f"),
            };

            _context.SheetHistories.Add(new_history);
        }
        _context.SaveChanges();

        return RedirectToAction("Sheets");
    }

    [HttpGet]
    public IActionResult FillSheet(int id)
    {
        var sheet = _context.Sheets.ToList().FirstOrDefault(x => x.Id == id);

        var marks = _context.StudentSheetRelations
            .Where(relation => relation.SheetId == sheet.Id)
            .ToDictionary(x => x.StudentId, x => x.Mark);
        var students = _context.Students.Where(student => student.GroupId == sheet.GroupId).ToList();
        var studentsvm = students.Select(student => new StudentSheetViewModel
        {
            Id = student.Id,
            BookNumber = student.BookNumber,
            Name = student.Name,
            Mark = marks.ContainsKey(student.Id) ? marks[student.Id] : -1
        }).ToList();

        var model = new FillSheetViewModel
        {
            Id = sheet.Id,
            Type = _context.SheetTypes.ToList().FirstOrDefault(type => type.Id == sheet.TypeId).Name,
            Term = sheet.TermNumber == 1 ? "Осенний" : "Весенний",
            Year = sheet.Year,
            Group = GetGroupNameById(sheet.GroupId),
            Students = studentsvm,
            Discipline = _context.Disciplines.ToList().FirstOrDefault(disc => disc.Id == sheet.DisciplineId).Name,
            Teacher = _context.Teachers.ToList().FirstOrDefault(teacher => teacher.Id == sheet.TeacherId).Name,
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult FillSheet(int id, List<int> marks)
    {
        var sheet = _context.Sheets.ToList().FirstOrDefault(x => x.Id == id);
        var students = _context.Students.Where(student => student.GroupId == sheet.GroupId).ToList();

        foreach (var (item, index) in students.Select((item, index) => (item, index)))
        {
            var relation = new StudentSheetRelation 
            {
                StudentId = item.Id,
                SheetId = sheet.Id,
                Mark = marks[index],
            };

            if (_context.StudentSheetRelations.FirstOrDefault(x => x.SheetId == id) == null)
            {
                _context.StudentSheetRelations.Add(relation);
            }
            else
            {
                relation = _context.StudentSheetRelations.First(x => x.StudentId == item.Id && x.SheetId == sheet.Id);
                relation.Mark = marks[index];
                _context.StudentSheetRelations.Update(relation);
            }
        }

        _context.SaveChanges();
        return RedirectToAction("Sheets");
    }

    [HttpGet]
    public IActionResult UploadSheet(int id)
    {
        var sheet = _context.Sheets.ToList().FirstOrDefault(x => x.Id == id);
        var history = new SheetHistory
        {
            SheetId = sheet.Id,
            StatusId = 4,
            UserId = _context.Users.ToList().FirstOrDefault(user => user.Email == User.Identity.Name).Id,
            DateCreated = DateTime.Now.ToString("f"),
        };

        _context.SheetHistories.Add(history);
        _context.SaveChanges();

        return RedirectToAction("Sheets");
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