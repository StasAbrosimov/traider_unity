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
    
    public bool Disabled;
    public int LevelThreshold;

    public string ShopPriceStr { get => !PriceDisabled ? ShopPrice.ToString() : string.Empty; }

    public virtual InventoryItemModel GetCopy(bool fullCopy = false)
    {
        return this.GetCopy(this, fullCopy);
    }
    public virtual InventoryItemModel GetCopy(InventoryItemModel item, bool fullCopy = false)
    {
        var copy = new InventoryItemModel();
        copy.ItemId = item.ItemId;
        copy.Image = item.Image;
        copy.Name = item.Name;
        copy.Description = item.Description;
        copy.BasePrice = item.BasePrice;
        if (fullCopy)
        {
            copy.ShopPrice = item.ShopPrice;
            copy.PriceDisabled = item.PriceDisabled;
            copy.Quantity = item.Quantity;
            copy.BaseQuantity = item.BaseQuantity;
            copy.InfiniteQuantity = item.InfiniteQuantity;
            copy.Disabled = item.Disabled;
            copy.LevelThreshold = item.LevelThreshold;
        }

        return copy;
    }
}
