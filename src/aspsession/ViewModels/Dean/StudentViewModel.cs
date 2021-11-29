namespace aspsession.ViewModels.Dean;

/// <summary>
/// Модель студента для вывода
/// </summary>
public class StudentViewModel
{
    /// <summary>
    /// Идентификатор студента
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер зачетной книжки
    /// </summary>
    public string BookNumber { get; set; }

    /// <summary>
    /// Фамилия И.О. студента
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Группа
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    public string Departments { get; set; }

    /// <summary>
    /// Институт
    /// </summary>
    public string Institute { get; set; }
}