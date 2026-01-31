using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Treider/InventoryItemSO")]
public class InventoryItemSO : ScriptableObject
{
    public string ItemId;
    public Sprite Image;
    public string Name;
    [TextArea(2, 6)]
    public string Description;
    public int Price;

    public InventoryItemModel ItemModel => new InventoryItemModel()
    {
        Name = Name,
        Description = Description,
        BasePrice = Price,
        Image = Image,
        ItemId = ItemId
    };
}
