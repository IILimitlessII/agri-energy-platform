using System.ComponentModel.DataAnnotations;
using AgriEnergyPlatform.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriEnergyPlatform.Web.ViewModels;

public class ProductFilterViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    [Display(Name = "Category")]
    public int SelectedCategoryId { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

    [Display(Name = "Start Date")]
    public DateOnly? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateOnly? EndDate { get; set; }
    
    public string SearchString { get; set; } = string.Empty;
}