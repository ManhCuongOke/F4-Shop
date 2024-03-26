public interface ICartReponsitory {
    IEnumerable<CartDetail> getCartInfo(int userID);
    IEnumerable<Cart> checkCartIDExist();
    bool insertCart();
    bool insertCartDetail(int userID, int productID, int cartID, int quantity, double unitPrice);
    IEnumerable<CartDetail> checkProduct(int userID, int productID);
}