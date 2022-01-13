namespace aspsession.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User
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
    public int RoleId { get; set; }

    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}