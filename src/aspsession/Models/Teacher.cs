namespace aspsession.Models;

/// <summary>
/// Модель преподавателя в системе
/// </summary>
public class Teacher
{
    /// <summary>
    /// Идентификатор преподавателя
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Фамилия И.О. преподавателя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    public int DepartmentsId { get; set; }
}