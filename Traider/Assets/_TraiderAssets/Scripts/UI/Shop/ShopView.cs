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

    public virtual void InitializeShop()
    {

    }

    public virtual void CloseView()
    {
        this.gameObject.SetActive(false);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);
    }
}
