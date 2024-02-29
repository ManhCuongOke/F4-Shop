using System.ComponentModel.DataAnnotations.Schema;

public class Product {
    public int PK_iProductID { get; set; }
    public int FK_iCategoryID { get; set; }
    public string sProductName { get; set; }
    public string sCategoryName { get; set; }
    public string sImageUrl { get; set; }
     public double dPrice { get; set; }
    public int iQuantity { get; set; }
    public string sProductDescription { get; set; }
    [NotMapped]
    public int iIsVisible { get; set; }
    [NotMapped]
    public DateTime dCreateTime {get; set;}

    [NotMapped]
    public DateTime dUpdateTime { get; set; }
    [NotMapped]
    public DateTime dDeleteTime {get; set;}
    
}