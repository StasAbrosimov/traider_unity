using System.Collections.Generic;
using UnityEngine;

public class ShopModel
{
    public string ShopId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Image { get; set; }
    public float SellItemModification { get; set; }
    public float ByItemModification { get; set; }
    public float CashTraded { get; set; }

    public List<ShopItemsLevelModel> LevelsToSell { get; set; }
    public List<ShopItemsLevelModel> LevelsToBy { get; set; }

    public List<InventoryItemModel> AllItemsToSell { get; set; }
    public List<InventoryItemModel> AllItemsToBy { get; set; }
    public Dictionary<string, InventoryItemModel> AllItemsToByDict { get; set; }
}

public class ShopItemsLevelModel
{
    public List<InventoryItemModel> ShopItems { get; set; }
    public int LevelThreshold { get; set; }
}