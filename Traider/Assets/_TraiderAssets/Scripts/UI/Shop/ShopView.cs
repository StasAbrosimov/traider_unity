using System;
using System.Collections.Generic;
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
    [SerializeField]
    private GameObject _nextLevelPanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI _nextLevel;

    private ShopModel _currentShop;
    private ShopItemsLevelModel _nextShopLevel = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _userItems.InventoryItemClickedEvent.RemoveListener(OnUserItemDetails);
        _userItems.InventoryItemClickedEvent.AddListener(OnUserItemDetails);

        _shopItems.InventoryItemClickedEvent.RemoveListener(OnShopItemDetails);
        _shopItems.InventoryItemClickedEvent.AddListener(OnShopItemDetails);

        _itemDetails.OnSubmitEvent.RemoveListener(OnSubmitTransaction);
        _itemDetails.OnSubmitEvent.AddListener(OnSubmitTransaction);
    }

    private void OnSubmitTransaction(ItemDetailsView.OnSubmitEventArgs arg0)
    {
        if (arg0.State != ItemDetailsView.ItemDetailsViewState.View)
        {
            var transactionCashAnout = arg0.Model.ShopPrice * arg0.TotalCount;
           var resultState = UserInventory.Instance.CommitTransaction(arg0.Model, arg0.TotalCount, transactionCashAnout, arg0.State == ItemDetailsView.ItemDetailsViewState.Sell);
            if(resultState == UserInventory.UserTransactionResult.Success)
            {
                _currentShop.CashTraded += transactionCashAnout;
                if (_nextShopLevel != null)
                {
                    _nextLevel.text = string.Format("{0}/{1}", _currentShop.CashTraded, _nextShopLevel.LevelThreshold);

                    if (_nextShopLevel.LevelThreshold <= _currentShop.CashTraded)
                    {
                        _itemDetails.InitializeView(null);
                        InitializeShop(_currentShop);
                    }
                }
                else
                {
                    InitializeSellItems();
                }

                _itemDetails.UpdateView(arg0.TotalCount, arg0.State == ItemDetailsView.ItemDetailsViewState.By ? 
                    UserInventory.Instance.UserMoney : 0);
            }
            else
            {
                InitializeSellItems();
            }
        }
    }

    private void OnUserItemDetails(InventoryItemView arg0)
    {
        _itemDetails.InitializeView(arg0.Model, ItemDetailsView.ItemDetailsViewState.Sell, 0);
    }

    private void OnShopItemDetails(InventoryItemView arg0)
    {
        _itemDetails.InitializeView(arg0.Model, ItemDetailsView.ItemDetailsViewState.By, UserInventory.Instance.UserMoney);
    }

    public void InitializeShop(ShopModel shopModel)
    {
        this.gameObject.SetActive(true);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);
        _currentShop = shopModel;

        _shopItems.InitializeTitle(_currentShop.Name, null);

        var currenttLevelIdex = shopModel.LevelsToSell.FindLastIndex(lvl => lvl.LevelThreshold <= shopModel.CashTraded);
        ShopItemsLevelModel currenttItemsLevel = null;
        if (currenttLevelIdex < 0) 
        {
            currenttLevelIdex = 0;
            currenttItemsLevel = new ShopItemsLevelModel()
            { LevelThreshold = 0 };
        }
        else
        {
            currenttItemsLevel = shopModel.LevelsToSell[currenttLevelIdex];
            _nextShopLevel = shopModel.LevelsToSell.Count - 1 <= currenttLevelIdex ?
                null :
                shopModel.LevelsToSell[currenttLevelIdex + 1];

            if(_nextShopLevel != null)
            {
                _nextLevelPanel.SetActive(true);
                _nextLevel.text = string.Format("{0}/{1}", shopModel.CashTraded, _nextShopLevel.LevelThreshold);
            }
            else
            {
                _nextLevelPanel.SetActive(false);
            }
        }

        _shopLevel.text = $"{currenttLevelIdex + 1}/{shopModel.LevelsToSell.Count}";

        _shopItems.InitializeItems(shopModel.AllItemsToSell.Where(sItem => sItem.LevelThreshold <= currenttItemsLevel.LevelThreshold).ToList());
        InitializeSellItems();
    }

    private void InitializeSellItems()
    {
        _userItems.InitializeTitle("Sell", UserInventory.Instance.UserMoney);
        LinkedList<InventoryItemModel> itemsToSell = new ();

        foreach (var item in UserInventory.Instance.UserItems)
        {
            var itemToSell =  item.GetCopy(true);

            itemToSell.PriceDisabled = true;
            itemToSell.Disabled = true;

            if (_currentShop.AllItemsToByDict.TryGetValue(item.ItemId, out var shopItem))
            {
                itemToSell.PriceDisabled = false;
                itemToSell.Disabled = false;
                itemToSell.ShopPrice = shopItem.ShopPrice;
                itemsToSell.AddFirst(itemToSell);
            }
            else
            {
                itemsToSell.AddLast(itemToSell);
            }
        }

        _userItems.InitializeItems(itemsToSell.ToList());
    }

    public void CloseView()
    {
        this.gameObject.SetActive(false);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);
    }
}
