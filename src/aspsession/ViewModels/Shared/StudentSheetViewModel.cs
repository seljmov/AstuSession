namespace aspsession.ViewModels.Shared;

public class StudentSheetViewModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер зачетной книжки
    /// </summary>
    public string BookNumber { get; set; }

    /// <summary>
    /// Фамилия И.О.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Полученная оценка
    /// </summary>
    public int Mark { get; set; }
}