namespace aspsession.Models;

/// <summary>
/// Кафедра
/// </summary>
public class Department
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
    /// Идентификатор института
    /// </summary>
    public int InstituteId { get; set; }
}