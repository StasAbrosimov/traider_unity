using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopsManager : MonoBehaviour
{
    [SerializeField]
    private ShopSO[] _shopsData;

    [SerializeField]
    private ShopsListView _shopsListView;

    [SerializeField]
    private ShopView _shopView;
    
    private List<ShopModel> _ShopsList;

    private const string UserInventoryID = "userInventoryID";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeShops();
    }
    private bool isInitialized = false;
    public void InitializeShops()
    {
        if (isInitialized) return;

        isInitialized = true;

        _ShopsList = new List<ShopModel>(_shopsData.Length + 1);

        _ShopsList.Add(new ShopModel()
        {
            ShopId = UserInventoryID,
            Name = "My Inventory"
        });

        foreach (var shop in _shopsData)
        {
            _ShopsList.Add(shop.GetShopModel());
        }

        _shopsListView.InitializeList(_ShopsList);
        _shopsListView.OnClickEvent.RemoveListener(OnShopSelected);
        _shopsListView.OnClickEvent.AddListener(OnShopSelected);
    }

    private void OnShopSelected(ShopModel arg0)
    {
        if(arg0.ShopId != UserInventoryID)
        {
            _shopView.InitializeShop(arg0);
        }
        else
        {
            _shopView.CloseView();
            //TODO: initialize user shop inventory
        }
    }
}
