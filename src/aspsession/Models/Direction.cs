namespace aspsession.Models;

/// <summary>
/// Направление
/// </summary>
public class Direction
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id {  get; set; }

    /// <summary>
    /// Название направления
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Сокращенное название
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Идентификатор кафедры
    /// </summary>
    public int DepartmentId { get; set; }
}