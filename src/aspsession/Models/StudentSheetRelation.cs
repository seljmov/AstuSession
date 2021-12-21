namespace aspsession.Models;

/// <summary>
/// Связь студент-ведомость
/// </summary>
public class StudentSheetRelation
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор студента
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Идентификатор ведомости
    /// </summary>
    public int SheetId { get; set; }

    /// <summary>
    /// Полученная оценка
    /// </summary>
    public int Mark { get; set; }
}