public interface IProductResponsitory {
    IEnumerable<Product> getProductsByCategoryID(int categoryID);
    IEnumerable<Product> getProductByID(int productID);
}