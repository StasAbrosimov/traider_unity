using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryGridView : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryItemPref;

    [SerializeField]
    private RectTransform _gridRectTransform;

    [SerializeField]
    private TMPro.TextMeshProUGUI _header;

    [SerializeField]
    private TMPro.TextMeshProUGUI _coisnCount;

    public UnityEvent<InventoryItemView> InventoryItemClickedEvent = new UnityEvent<InventoryItemView>();

    private List<InventoryItemView> _items;
    private int? _moneyAmount;

    public int? MoneyAmount => _moneyAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void InitializeTitle(string title = null, int? moneyAmount = null)
    {
        _moneyAmount = moneyAmount;
        ChangeMoneyAmount(0);
        _header.text = title;
    }

    public void InitializeItems(List<InventoryItemModel> items)
    {
        DestroyAllItems();
        if (items != null && items.Count > 0)
        {
            _items = new List<InventoryItemView>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                _items.Add(SpawnItemView(items[i]));
            }
        }
    }

    public void ChangeMoneyAmount(int moneyDiff, bool resetToDiff = false)
    {
        
        if(_moneyAmount.HasValue && !resetToDiff)
        {
            _moneyAmount += moneyDiff;
        }
        else if(resetToDiff)
        {
            _moneyAmount = _moneyAmount.GetValueOrDefault(0);
        }

        if(_coisnCount.gameObject.activeSelf != _moneyAmount.HasValue)
        {
            _coisnCount.gameObject.SetActive(_moneyAmount.HasValue);
        }
        _coisnCount.text = _moneyAmount.ToString();
    }

    private InventoryItemView SpawnItemView(InventoryItemModel item)
    {
        var itemGO = Instantiate(_inventoryItemPref, _gridRectTransform);
        var itemModel = itemGO.GetComponent<InventoryItemView>();
        itemModel.InitializeView(item);

        itemModel.ItemSelectedEvent.RemoveListener(OnItemClickedEventListener);
        itemModel.ItemSelectedEvent.AddListener(OnItemClickedEventListener);

        return itemModel;
    }

    private void OnItemClickedEventListener(InventoryItemView item)
    {
        InventoryItemClickedEvent?.Invoke(item);
    }

    private void DestroyAllItems()
    {
        for (int i = _gridRectTransform.childCount - 1; i >= 0; i--)
        {
            GameObject child = _gridRectTransform.GetChild(i).gameObject;
            Destroy(child);
        }

        if (_items == null || _items.Count == 0) return;
        _items.Clear();
    }
}
