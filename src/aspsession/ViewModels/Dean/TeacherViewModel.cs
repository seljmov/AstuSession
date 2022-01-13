namespace aspsession.ViewModels.Dean;

/// <summary>
/// Модель преподавателя для вывода
/// </summary>
public class TeacherViewModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Фамилия И.О.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    public string Departments { get; set; }

    /// <summary>
    /// Институт
    /// </summary>
    public string Institute { get; set; }
}