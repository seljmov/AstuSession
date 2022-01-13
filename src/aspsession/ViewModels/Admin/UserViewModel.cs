namespace aspsession.ViewModels.Admin;

/// <summary>
/// Модель пользователя для вывода
/// </summary>
public class UserViewModel
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
    /// Роль в системе
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}