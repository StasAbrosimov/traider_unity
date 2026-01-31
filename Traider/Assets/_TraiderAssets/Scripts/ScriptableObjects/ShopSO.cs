using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDescription", menuName = "Treider/ShopSO")]
public class ShopSO : ScriptableObject
{
    public string ShopId;
    public string Name;
    public string Description;
    public Sprite Image;
    public float SellItemModification;
    public float ByItemModification;
    public ShopItemLevel[] ItemsToSell;
    public ShopItemLevel[] ItemsToBy;

    public ShopModel GetShopModel()
    {
        var result = new ShopModel();

        result.ShopId = ShopId;
        result.Name = Name;
        result.Description = Description;
        result.Image = Image;

        result.SellItemModification = SellItemModification;
        result.ByItemModification = ByItemModification;

        result.LevelsToSell = new List<ShopItemsLevelModel>(ItemsToSell.Length);
        result.LevelsToBy = new List<ShopItemsLevelModel>(ItemsToBy.Length);

        result.AllItemsToSell = FillModelItemsList(result.LevelsToSell, ItemsToSell, SellItemModification);
        result.AllItemsToBy = FillModelItemsList(result.LevelsToBy, ItemsToBy, ByItemModification);
        result.AllItemsToByDict = new Dictionary<string, InventoryItemModel>();

        foreach (var item in result.AllItemsToBy)
        {
            result.AllItemsToByDict.Add(item.ItemId, item);
        }

        return result;
    }

    private List<InventoryItemModel> FillModelItemsList(List<ShopItemsLevelModel> items, ShopItemLevel[] dataItems, float priceModificator)
    {
        var result = new LinkedList<InventoryItemModel>();

        foreach (var itemLevel in dataItems)
        {
            ShopItemsLevelModel level = new ShopItemsLevelModel();
            items.Add(level);
            level.ShopItems = new List<InventoryItemModel>(itemLevel.shopLevelItems.Length);
            level.LevelThreshold = itemLevel.ShopLevelThreshold;

            InventoryItemModel model = null;
            foreach (var soItem in itemLevel.shopLevelItems)
            {
                model = soItem.shopItem.ItemModel;

                var curModificator = soItem.OverridePrice ? soItem.OverrideModification : priceModificator;
                model.ShopPrice = Mathf.CeilToInt(model.BasePrice * curModificator);
                model.Quantity = model.BaseQuantity = soItem.Quantity;
                model.InfiniteQuantity = soItem.InfiniteQuantity;
                model.LevelThreshold = level.LevelThreshold;
                level.ShopItems.Add(model);
                result.AddLast(model);
            }
        }

        return result.ToList();
    }
}


[System.Serializable]
public class ShopItemLevel
{
    public ShopItem[] shopLevelItems;
    public int ShopLevelThreshold; // the amount of traded to open the level.
}

[System.Serializable]
public class ShopItem
{
    public InventoryItemSO shopItem;
    public bool OverridePrice;
    public float OverrideModification;
    public int Quantity;
    public bool InfiniteQuantity;
}

