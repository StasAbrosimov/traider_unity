using System;
using System.Linq;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField]
    private InventoryGridView _shopItems;
    [SerializeField]
    private InventoryGridView _userItems;
    [SerializeField]
    private ItemDetailsView _itemDetails;
    [SerializeField]
    private TMPro.TextMeshProUGUI _title;
    [SerializeField]
    private TMPro.TextMeshProUGUI _shopLevel;

    private ShopModel _currentShop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _userItems.InventoryItemClickedEvent.RemoveListener(OnUserItemDetails);
        _userItems.InventoryItemClickedEvent.AddListener(OnUserItemDetails);

        _shopItems.InventoryItemClickedEvent.RemoveListener(OnShopItemDetails);
        _shopItems.InventoryItemClickedEvent.AddListener(OnShopItemDetails);
    }

    private void OnUserItemDetails(InventoryItemView arg0)
    {
        _itemDetails.InitializeView(arg0.Model, ItemDetailsView.ItemDetailsViewState.View, 0);
    }

    private void OnShopItemDetails(InventoryItemView arg0)
    {
        _itemDetails.InitializeView(arg0.Model, ItemDetailsView.ItemDetailsViewState.By, 0);
    }

    public virtual void InitializeShop(ShopModel shopModel)
    {
        this.gameObject.SetActive(true);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);
        _currentShop = shopModel;

        _userItems.InitializeItems(new System.Collections.Generic.List<InventoryItemModel>());
        _userItems.InitializeTitle("Sell", 0);

        _shopItems.InitializeTitle(_currentShop.Name, 0);

        var currenttItemsLevel = shopModel.LevelsToBy.FirstOrDefault(lvl => lvl.LevelThreshold >= shopModel.CashTraded);
        if (currenttItemsLevel == null) 
        {
            currenttItemsLevel = new ShopItemsLevelModel()
            { LevelThreshold = 0 };
        };

        _shopItems.InitializeItems(shopModel.AllItemsToSell.Where(sItem => sItem.LevelThreshold >= currenttItemsLevel.LevelThreshold).ToList());
    }

    public virtual void CloseView()
    {
        this.gameObject.SetActive(false);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);
    }
}
