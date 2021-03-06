using aspsession.Contexts;
using aspsession.Models;
using aspsession.ViewModels.Dean;
using aspsession.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        return RedirectToAction("Sheets");
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
                Teacher = _context.Teachers.ToList().FirstOrDefault(teacher => teacher.Id == sheet.TeacherId).Name,
                Status = statuses[histories[sheet.Id].Last().StatusId]
            }).ToList();
        }

        return View(model);
    }

    /// <summary>
    /// Страница создания ведомости
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult CreateSheet()
    {
        var types = _context.SheetTypes
            .ToDictionary(type => type.Id, type => type.Name)
            .Select(type => new SelectListItem { Value = type.Key.ToString(), Text = type.Value })
            .ToList();

        var groups = _context.Groups
            .ToDictionary(group => group.Id, group => GetGroupNameById(group.Id))
            .Select(group => new SelectListItem { Value = group.Key.ToString(), Text = group.Value })
            .ToList();

        var disciplines = _context.Disciplines
            .ToDictionary(disc => disc.Id, disc => disc.Name)
            .Select(disc => new SelectListItem { Value = disc.Key.ToString(), Text = disc.Value })
            .ToList();

        var teachers = _context.Teachers
            .ToDictionary(teacher => teacher.Id, teacher => teacher.Name)
            .Select(teacher => new SelectListItem { Value = teacher.Key.ToString(), Text = teacher.Value })
            .ToList();

        var currMonth = Convert.ToInt32(DateTime.Now.ToString("MM"));
        var currYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

        var model = new CreateSheetViewModal 
        { 
            TermNumber = currMonth >= 2 && currMonth <= 6 ? 2 : 1, // Если месяц в промежутке февраль-июнь, то 2 семестр, иначе - 1
            Types = types,
            Year = currYear,
            Groups = groups,
            Disciplines = disciplines,
            Teachers = teachers
        };

        return View(model);
    }

    /// <summary>
    /// Страница создания ведомости
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateSheet(CreateSheetViewModal createSheet)
    {
        try
        {
            _context.Database.ExecuteSqlInterpolated($"execute InsertSheet @type = {createSheet.SelectedType}, @term = {createSheet.TermNumber}, @year = {createSheet.Year}, @group = {createSheet.SelectedGroup}, @disc = {createSheet.SelectedDiscipline}, @teacher = {createSheet.SelectedTeacher}, @user = {_context.Users.ToList().FirstOrDefault(user => user.Email == User.Identity.Name).Id}");
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            var message = ex.Message.Split("\r\n").First();
            ModelState.AddModelError("", message);
        }

        if (!ModelState.IsValid)
            return View(createSheet);

        return RedirectToAction("Sheets");
    }

    /// <summary>
    /// Подтверждение ведомости
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ConfirmSheet(int id)
    {
        var user = _context.Users.ToList().First(x => x.Email == User.Identity.Name);

        var history = new SheetHistory
        {
            SheetId = id,
            StatusId = 5, // Статус - Подтверждено
            UserId = user.Id,
            DateCreated = DateTime.Now.ToString("f"),
        };

        _context.SheetHistories.Add(history);
        _context.SaveChanges();

        return RedirectToAction("Sheets");
    }

    [HttpGet]
    public IActionResult UploadSheet(int id)
    {
        var userid = _context.Users.ToList().FirstOrDefault(user => user.Email == User.Identity.Name).Id;

        _context.Database.ExecuteSqlInterpolated($"execute InsertSheetHistory @sheetid = {id}, @statusid = {2}, @userid = {userid};");
        _context.SaveChanges();

        return RedirectToAction("Sheets");
    }

    [HttpGet]
    public IActionResult DetailsSheet(int id)
    {
        var sheet = _context.Sheets.ToList().FirstOrDefault(x => x.Id == id);
        var histories = _context.SheetHistories.Where(x => x.SheetId == id).ToList();
        var historiesvm = histories.Select(history => new SheetHistoryViewModel
        {
            Id = history.Id,
            Status = _context.SheetStatuses.ToList().FirstOrDefault(status => status.Id == history.StatusId).Name,
            UserEmail = _context.Users.ToList().FirstOrDefault(user => user.Id == history.UserId).Email,
            DateCreated = history.DateCreated,
        }).ToList();

        var marks = _context.StudentSheetRelations
            .Where(relation => relation.SheetId == sheet.Id)
            .ToDictionary(x => x.StudentId, x => x.Mark);
        var students = _context.Students.Where(student => student.GroupId == sheet.GroupId).ToList();
        var studentsvm = students.Select(student => new StudentSheetViewModel
        {
            Id = student.Id,
            BookNumber = student.BookNumber,
            Name = student.Name,
            Mark = marks.ContainsKey(student.Id) ? marks[student.Id] : 0
        }).ToList();

        var model = new DetailSheetViewModel 
        {
            Id = sheet.Id,
            Type = _context.SheetTypes.ToList().FirstOrDefault(type => type.Id == sheet.TypeId).Name,
            Term = sheet.TermNumber == 1 ? "Осенний" : "Весенний",
            Year = sheet.Year,
            Group = GetGroupNameById(sheet.GroupId),
            Students = studentsvm,
            Discipline = _context.Disciplines.ToList().FirstOrDefault(disc => disc.Id == sheet.DisciplineId).Name,
            Teacher = _context.Teachers.ToList().FirstOrDefault(teacher => teacher.Id == sheet.TeacherId).Name,
            Histories = historiesvm,
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult EditSheet(int id)
    {
        var sheet = _context.Sheets.ToList().FirstOrDefault(x => x.Id == id);
        var types = _context.SheetTypes
            .ToDictionary(type => type.Id, type => type.Name)
            .Select(type => new SelectListItem { Value = type.Key.ToString(), Text = type.Value })
            .ToList();

        var groups = _context.Groups
            .ToDictionary(group => group.Id, group => GetGroupNameById(group.Id))
            .Select(group => new SelectListItem { Value = group.Key.ToString(), Text = group.Value })
            .ToList();

        var disciplines = _context.Disciplines
            .ToDictionary(disc => disc.Id, disc => disc.Name)
            .Select(disc => new SelectListItem { Value = disc.Key.ToString(), Text = disc.Value })
            .ToList();

        var teachers = _context.Teachers
            .ToDictionary(teacher => teacher.Id, teacher => teacher.Name)
            .Select(teacher => new SelectListItem { Value = teacher.Key.ToString(), Text = teacher.Value })
            .ToList();

        var currMonth = Convert.ToInt32(DateTime.Now.ToString("MM"));
        var currYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

        var model = new CreateSheetViewModal
        {
            TermNumber = currMonth >= 2 && currMonth <= 6 ? 2 : 1, // Если месяц в промежутке февраль-июнь, то 2 семестр, иначе - 1
            Types = types,
            Year = currYear,
            Groups = groups,
            Disciplines = disciplines,
            Teachers = teachers,
            SelectedType = sheet.TypeId,
            SelectedGroup = sheet.GroupId,
            SelectedDiscipline = sheet.DisciplineId,
            SelectedTeacher = sheet.TeacherId,
        };

        return View(model);
    }

    public IActionResult EditSheet(CreateSheetViewModal editsheet)
    {
        var updsheet = new Sheet
        {
            Id = editsheet.Id,
            TypeId = editsheet.SelectedType,
            TermNumber = editsheet.TermNumber,
            Year = editsheet.Year,
            GroupId = editsheet.SelectedGroup,
            DisciplineId = editsheet.SelectedDiscipline,
            TeacherId = editsheet.SelectedTeacher
        };

        _context.Sheets.Update(updsheet);
        _context.SaveChanges();

        return RedirectToAction("Sheets");
    }

    [HttpGet]
    public IActionResult DeleteSheet(int id)
    {
        _context.Database.ExecuteSqlInterpolated($"execute DeleteSheet @id = {id}");
        _context.SaveChanges();

        return RedirectToAction("Sheets");
    }

    public IActionResult ShowReport(string criteria)
    {
        var sheets = _context.Sheets.ToList();

        var criteriasvm = sheets.Select(sheet => new SelectListItem 
        {
            Value = GetTitleTermOfYear(sheet.TermNumber, sheet.Year),
            Text = GetTitleTermOfYear(sheet.TermNumber, sheet.Year),
        });

        IEnumerable<SheetViewModel> sheetsvm = null;

        if (!string.IsNullOrEmpty(criteria))
        {
            var suitable = sheets
                .Where(sheet => criteria == GetTitleTermOfYear(sheet.TermNumber, sheet.Year))
                .ToList();

            var statuses = _context.SheetStatuses.AsEnumerable().ToDictionary(status => status.Id, status => status.Name);
            var histories = _context.SheetHistories.AsEnumerable()
                .GroupBy(x => x.SheetId)
                .ToDictionary(x => x.Key, x => x.ToList());

            sheetsvm = suitable.Select(sheet => new SheetViewModel
            {
                Id = sheet.Id,
                Type = _context.SheetTypes.ToList().FirstOrDefault(type => type.Id == sheet.TypeId).Name,
                Term = sheet.TermNumber == 1 ? "Осенний" : "Весенний",
                Year = sheet.Year,
                Group = UniversityHierarchyByGroupId(sheet.GroupId).Group,
                Discipline = _context.Disciplines.ToList().FirstOrDefault(disc => disc.Id == sheet.DisciplineId).Name,
                Teacher = _context.Teachers.ToList().FirstOrDefault(teacher => teacher.Id == sheet.TeacherId).Name,
                Status = statuses[histories[sheet.Id].Last().StatusId]
            }).ToList();
        }

        var model = new ShowReportViewModel
        {
            ReportCriterias = criteriasvm,
            Sheets = sheetsvm
        };

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
            Departments = UniversityHierarchyByDepartmentId(teacher.DepartmentId).Departments,
            Institute = UniversityHierarchyByDepartmentId(teacher.DepartmentId).Institute,
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

    private string GetTitleTermOfYear(int term, int year)
    {
        var title = term == 1 ? "Осенний" : "Весенний";
        return $"{title} семестр {year} года";
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

    private (string Departments, string Institute) UniversityHierarchyByDepartmentId(int departmentId)
    {
        var departments = _context.Departments.ToList().FirstOrDefault(dep => dep.Id == departmentId);
        var institute = _context.Institutes.ToList().FirstOrDefault(inst => inst.Id == departments.InstituteId);

        return (departments.Name, institute.Name);
    }
}