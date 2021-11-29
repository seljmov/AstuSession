namespace aspsession.ViewModels.Admin;

/// <summary>
/// Модель коллекции пользователей для вывода
/// </summary>
public class UsersListViewModel
{
    /// <summary>
    /// Коллекция пользователей для вывода
    /// </summary>
    public IList<UserViewModel> Users {  get; set; }
}