public interface IProductResponsitory {
    IEnumerable<Product> getProductsByCategoryID(int categoryID);
}