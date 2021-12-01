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
    /// Идентификатор направления
    /// </summary>
    public int DirectionId { get; set; }

    /// <summary>
    /// Курс
    /// </summary>
    public int Course { get; set; }
}