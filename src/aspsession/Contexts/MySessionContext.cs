using aspsession.Models;
using Microsoft.EntityFrameworkCore;

namespace aspsession.Contexts;

/// <summary>
/// Контекст базы данных
/// </summary>
public class MySessionContext : DbContext
{
    /// <summary>
    /// Конструктор класса <see cref="MySessionContext"/>
    /// </summary>
    /// <param name="options">Опции контекста базы данных</param>
    public MySessionContext(DbContextOptions<MySessionContext> options)
        : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    /// <summary>
    /// Коллекция пользователей
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Коллекция ролей пользователей в системе
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Коллекция ведомостей
    /// </summary>
    public DbSet<Sheet> Sheets { get; set; }

    /// <summary>
    /// Коллекция дисциплин
    /// </summary>
    public DbSet<Discipline> Disciplines { get; set; }

    /// <summary>
    /// Коллекция направлений
    /// </summary>
    public DbSet<Direction> Directions { get; set; }

    /// <summary>
    /// Коллекция групп
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Коллекция отношений студент-ведомость
    /// </summary>
    public DbSet<StudentSheetRelation> StudentSheetRelations { get; set; }

    /// <summary>
    /// Коллекция историй ведомостей
    /// </summary>
    public DbSet<SheetHistory> SheetHistories { get; set; }

    /// <summary>
    /// Коллекция статусов ведомостей
    /// </summary>
    public DbSet<SheetStatus> SheetStatuses { get; set; }

    /// <summary>
    /// Коллекция типов ведомостей
    /// </summary>
    public DbSet<SheetType> SheetTypes { get; set; }

    /// <summary>
    /// Коллекция преподавателей
    /// </summary>
    public DbSet<Teacher> Teachers { get; set; }

    /// <summary>
    /// Коллекция отношений преподаватель-дисциплина
    /// </summary>
    public DbSet<TeacherDisciplinesRelation> TeacherDisciplinesRelations { get; set; }

    /// <summary>
    /// Коллекция студентов
    /// </summary>
    public DbSet<Student> Students { get; set; }

    /// <summary>
    /// Коллекция институтов
    /// </summary>
    public DbSet<Institute> Institutes { get; set; }

    /// <summary>
    /// Коллекция кафедр
    /// </summary>
    public DbSet<Department> Departments { get; set; }
}