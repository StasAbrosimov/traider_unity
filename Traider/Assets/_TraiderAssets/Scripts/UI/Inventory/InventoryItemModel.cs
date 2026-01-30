using UnityEngine;

public class InventoryItemModel
{
    public string ItemId;
    public Sprite Image;
    public string Name;
    public string Description;
    public int BasePrice;

    public int ShopPrice;
    public bool PriceDisabled;
    public int Quantity;
    public int BaseQuantity;
    public bool InfiniteQuantity;
    
    public bool IsDisabled;
    public int LevelThreshold;

    public string ShopPriceStr { get => !PriceDisabled ? ShopPrice.ToString() : string.Empty; }
}
