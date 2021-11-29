using aspsession.Contexts;
using aspsession.Models;
using aspsession.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspsession.Controllers
{
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

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Users", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
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

                await _context.SaveChangesAsync();
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

        [HttpGet]
        public IActionResult Create()
        {
            var user = new User();
            return PartialView("_CreateUserModalPartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return PartialView("_CreateUserModalPartial", user);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);
            return PartialView("_EditUserModalPartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return PartialView("_EditUserModalPartial", user);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);
            return PartialView("_DeleteUserModalPartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return PartialView("_DeleteUserModalPartial", user);
        }
    }
}
