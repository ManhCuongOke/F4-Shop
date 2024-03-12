using System.Collections;

public class CartViewModel {
    public IEnumerable<CartDetail> Carts { get; set; }
    public int CartCount { get; set; }
}