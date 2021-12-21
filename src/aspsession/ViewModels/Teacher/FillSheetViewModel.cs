using aspsession.ViewModels.Dean;

namespace aspsession.ViewModels.Teacher;

public class FillSheetViewModel
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
    /// Список студентов в ведомости
    /// </summary>
    public IList<StudentSheetViewModel> Students { get; set; }

    /// <summary>
    /// Дисциплина
    /// </summary>
    public string Discipline { get; set; }

    /// <summary>
    /// Преподаватель
    /// </summary>
    public string Teacher { get; set; }

    /// <summary>
    /// История ведомости
    /// </summary>
    public IList<SheetHistoryViewModel> Histories { get; set; }
}