namespace aspsession.Models;

/// <summary>
/// Связь студент-ведомость
/// </summary>
public class TeacherDisciplinesRelation
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор преподавателя
    /// </summary>
    public int TeacherId { get; set; }

    /// <summary>
    /// Идентификатор дисциплины
    /// </summary>
    public int DisciplineID { get; set; }
}