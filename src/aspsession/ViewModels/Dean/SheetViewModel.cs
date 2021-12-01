namespace aspsession.ViewModels.Dean;

/// <summary>
/// Модель ведомости для вывода
/// </summary>
public class SheetViewModel
{
    /// <summary>
    /// Идентификатор ведомости
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип ведомости: экзаменационная или зачетная (дифф зачет или обычный)
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Семестр (Осенний/весенний)
    /// </summary>
    public string Term { get; set; }

    /// <summary>
    /// Год
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Экзаменуемая учебная группа
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Дисциплина
    /// </summary>
    public string Discipline { get; set; }

    /// <summary>
    /// Преподаватель
    /// </summary>
    public string Teacher { get; set; }

    /// <summary>
    /// Актуальный статус заявки
    /// </summary>
    public string Status { get; set; }
}