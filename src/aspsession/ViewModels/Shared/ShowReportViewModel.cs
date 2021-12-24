using aspsession.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace aspsession.ViewModels.Shared;

/// <summary>
/// Модель для просмотра отчета
/// </summary>
public class ShowReportViewModel
{
    /// <summary>
    /// Критерии фильтрации
    /// </summary>
    public IEnumerable<SelectListItem> ReportCriterias { get; set; }

    /// <summary>
    /// Ведомости по критерию
    /// </summary>
    public IEnumerable<SheetViewModel> Sheets { get; set; }
}