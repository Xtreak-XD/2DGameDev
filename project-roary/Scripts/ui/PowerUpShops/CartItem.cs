public class CartItem 
{
    public IndividualItem Item { get; set; }
    public int Quantity { get; set; }
    public int MaxQuantity { get; set; }
    
    public int TotalPrice => Item.shopPrice * Quantity;

    public CartItem(IndividualItem item, int quantity, int maxQuantity)
    {
        Item = item;
        Quantity = quantity;
        MaxQuantity = maxQuantity;
    }
}