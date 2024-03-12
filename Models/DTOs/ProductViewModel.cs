using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

public class ProductViewModel {
    public IEnumerable<Product> Products {get; set;}
    public IEnumerable<Category> Categories {get; set;}
    public IEnumerable<CartDetail> CartDetails { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}