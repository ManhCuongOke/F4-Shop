public interface IProductResponsitory {
    IEnumerable<Product> getProductsByCategoryID(int categoryID);
    IEnumerable<Product> getProductByID(int productID);
    IEnumerable<Product> getProductsByCategoryIDAndSortIncre(int categoryID);
    IEnumerable<Product> getProductsByCategoryIDAndSortReduce(int categoryID);
}