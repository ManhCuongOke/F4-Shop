using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Project.Models;

public class ProductViewModel {
    public IEnumerable<Product> Products {get; set;}
    public IEnumerable<Category> Categories {get; set;}
    public IEnumerable<CartDetail> CartDetails { get; set; }
    public int UserID { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int CurrentCategoryID { get; set; }
}