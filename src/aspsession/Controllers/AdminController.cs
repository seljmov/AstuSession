using aspsession.Contexts;
using aspsession.Models;
using aspsession.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace aspsession.Controllers;

/// <summary>
/// Контроллер администратора
/// </summary>
[Authorize(Roles = "Администратор")]
public class AdminController : Controller
{
    private MySessionContext _context;

    /// <summary>
    /// Конструктор класса <see cref="AdminController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public AdminController(MySessionContext context)
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
        return RedirectToAction("Users");
    }

    [HttpGet]
    public IActionResult Users()
    {
        var users = _context.Users.ToList();
        // Берем преподавателей, которых нет в таблице пользователей
        var teachers = _context.Teachers.ToList()
            .Where(teacher => users.FirstOrDefault(user => user.Name == teacher.Name) == null)
            .ToList();

        // Если такие преподаватели есть, то добавляем их в таблицу пользователей
        if (teachers.Any())
        {
            var teacherRoleId = _context.Roles.Single(role => role.Name == "Преподаватель");
            foreach (var teacher in teachers)
            {
                _context.Users.Add(new User
                {
                    Name = teacher.Name,
                    RoleId = teacherRoleId.Id,
                });
            }

            _context.SaveChanges();
            users = _context.Users.ToList();
        }

        var rolesById = _context.Roles.ToDictionary(role => role.Id, role => role.Name);
        var model = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Role = rolesById[user.RoleId],
            Email = user.Email,
            Password = user.Password,
        }).ToList();

        return View(model);
    }

    /// <summary>
    /// Страница создания пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult CreateUser()
    {
        var user = new User();
        return View(user);
    }

    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        try
        {
            _context.Database.ExecuteSqlInterpolated($"execute InsertUser @name = {user.Name}, @roleid = {user.RoleId}, @email = {user.Email}, @password = {user.Password}");
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            var message = ex.Message.Split("\r\n").First();
            ModelState.AddModelError("", message);
        }

        if (!ModelState.IsValid)
            return View(user);

        return RedirectToAction("Users");
    }

    /// <summary>
    /// Страница редактирования данных пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult EditUser(int id)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        return View(user);
    }

    /// <summary>
    /// Редактирования данных пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult EditUser(User user)
    {
        try
        {
            _context.Database.ExecuteSqlInterpolated($"execute UpdateUser @id = {user.Id}, @name = {user.Name}, @roleid = {user.RoleId}, @email = {user.Email}, @password = {user.Password}");
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            var message = ex.Message.Split("\r\n").First();
            ModelState.AddModelError("", message);
        }

        if (!ModelState.IsValid)
            return View(user);

        return RedirectToAction("Users");
    }

    /// <summary>
    /// Страница удаления пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        return PartialView("_DeleteUserModalPartial", user);
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Delete(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return PartialView("_DeleteUserModalPartial", user);
    }
}