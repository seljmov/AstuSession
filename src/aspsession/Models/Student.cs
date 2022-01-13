namespace aspsession.Models;

/// <summary>
/// Модель студента в системе
/// </summary>
public class Student
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
    public int GroupId { get; set; }

    #region To Generate a Sheet

    /// <summary>
    /// Количество свойств студента
    /// </summary>
    public static int FieldCount { get; } = 4;

    /// <summary>
    /// Свойства студента для вывода в таблицу ведомости
    /// </summary>
    /// <returns>Список свойств</returns>
    public IList<object> PropsForTable()
    {
        return new List<object>
            {
                Id, Name, BookNumber, ""
            };
    }

    #endregion
}