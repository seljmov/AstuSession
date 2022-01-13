using Microsoft.AspNetCore.Mvc.Rendering;

namespace aspsession.ViewModels.Dean;

public class CreateSheetViewModal
{
    /// <summary>
    /// Идентификатор ведомости
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер семестра
    /// </summary>
    public int TermNumber { get; set; }

    /// <summary>
    /// Год
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Выбранный тип ведомости
    /// </summary>
    public int SelectedType { get; set; }

    /// <summary>
    /// Выбранная группа
    /// </summary>
    public int SelectedGroup { get; set; }

    /// <summary>
    /// Выбранная дисциплина
    /// </summary>
    public int SelectedDiscipline { get; set; }

    /// <summary>
    /// Выбранный преподаватель
    /// </summary>
    public int SelectedTeacher { get; set; }

    #region For Partial View

    /// <summary>
    /// Тип ведомости: экзаменационная или зачетная (дифф зачет или обычный)
    /// </summary>
    public IList<SelectListItem> Types { get; set; }

    /// <summary>
    /// Список учебных группа
    /// </summary>
    public IList<SelectListItem> Groups { get; set; }

    /// <summary>
    /// Список дисциплин
    /// </summary>
    public IList<SelectListItem> Disciplines { get; set; }

    /// <summary>
    /// Список преподавателей
    /// </summary>
    public IList<SelectListItem> Teachers { get; set; }

    #endregion
}