namespace aspsession.Models;

/// <summary>
/// Истории ведомости
/// </summary>
public class SheetHistory
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор ведомости
    /// </summary>
    public int SheetId { get; set; }

    /// <summary>
    /// Идентификатор статуса
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTime DateCreated { get; set; }
}