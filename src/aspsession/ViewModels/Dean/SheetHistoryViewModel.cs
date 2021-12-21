namespace aspsession.ViewModels.Dean;

public class SheetHistoryViewModel
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// Пользователь, сменивший статус
    /// </summary>
    public string UserEmail { get; set; }
    
    /// <summary>
    /// Дата созания записи
    /// </summary>
    public string DateCreated { get; set; }
}