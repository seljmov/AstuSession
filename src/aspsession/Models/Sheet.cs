namespace aspsession.Models;

/// <summary>
/// Модель ведомости в системе
/// </summary>
public class Sheet
{
    /// <summary>
    /// Идентификатор ведомости
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип ведомости: экзаменационная или зачетная (дифф зачет или обычный)
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Номер семестра
    /// </summary>
    public int TermNumber { get; set; }

    /// <summary>
    /// Год
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Экзаменуемая учебная группа
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Дисциплина
    /// </summary>
    public int DisciplineId { get; set; }

    /// <summary>
    /// Преподаватель
    /// </summary>
    public int TeacherId { get; set; }
}