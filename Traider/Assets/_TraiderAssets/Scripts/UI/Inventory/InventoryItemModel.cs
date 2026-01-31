using UnityEngine;

public class InventoryItemModel
{
    public InventoryItemModel()
    {

    }

    public InventoryItemModel(InventoryItemModel item, bool fullCopy = false)
    {
        this.ItemId = item.ItemId;
        this.Image = item.Image;
        this.Name = item.Name;
        this.Description = item.Description;
        this.BasePrice = item.BasePrice;
        if(fullCopy)
        {
            this.ShopPrice = item.ShopPrice;
            this.PriceDisabled = item.PriceDisabled;
            this.Quantity = item.Quantity;
            this.BaseQuantity = item.BaseQuantity;
            this.InfiniteQuantity = item.InfiniteQuantity;
            this.Disabled = item.Disabled;
            this.LevelThreshold = item.LevelThreshold;
        }
    }
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
    
    public bool Disabled;
    public int LevelThreshold;

    public string ShopPriceStr { get => !PriceDisabled ? ShopPrice.ToString() : string.Empty; }
}
