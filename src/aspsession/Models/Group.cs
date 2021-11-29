namespace aspsession.Models;

/// <summary>
/// Группа
/// </summary>
public class Group
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    public int DepartmentsId { get; set; }
}