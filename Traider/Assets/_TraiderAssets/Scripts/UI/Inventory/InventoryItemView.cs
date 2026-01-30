using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemName;

    [SerializeField]
    private TMPro.TextMeshProUGUI _itemPrice;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemQuantity;
    [SerializeField]
    private Button _itemButton;

    [SerializeField]
    private Color _normalButtonBGColor = Color.clear;
    [SerializeField]
    private Color _disabeledButtonBGColor = Color.black;

    [SerializeField]
    private Image _buttonBGImage;

    public UnityEvent<InventoryItemView> ItemSelectedEvent = new UnityEvent<InventoryItemView>();
    public InventoryItemModel Model { get; private set; }

    public bool IsDisabled
    {
        get => (Model?.IsDisabled).GetValueOrDefault(false);
        private set
        {
            if(Model != null && Model.IsDisabled != value)
            {
                Model.IsDisabled = value;
                InitializeView(Model);
            }
        }
    }

    public void AddQuantity(int delta)
    {
        Model.Quantity += delta;
        _itemQuantity.text = Model.Quantity.ToString();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _itemButton.onClick.RemoveListener(OnItemCliked);
        _itemButton.onClick.AddListener(OnItemCliked);
    }

    private void OnItemCliked()
    {
        ItemSelectedEvent?.Invoke(this);
    }

    public void InitializeView(InventoryItemModel model)
    {
        Model = model;
        if (Model != null)
        {
            _image.sprite = Model.Image;
            _itemName.text = Model.Name;
            _itemQuantity.gameObject.SetActive(!Model.InfiniteQuantity);
            _itemQuantity.text = Model.Quantity.ToString();
            _itemPrice.text = Model.ShopPriceStr;
            _buttonBGImage.color = model.IsDisabled ? _disabeledButtonBGColor : _normalButtonBGColor;
            _itemButton.interactable = !model.IsDisabled;
        }
        else
        {
            _image.sprite = null;
            _itemName.text = null;
            _itemQuantity.gameObject.SetActive(false);
            _itemPrice.text = null;
            _buttonBGImage.color = _disabeledButtonBGColor;
            _itemButton.interactable = false;
        }

        _itemPrice.gameObject.SetActive(Model == null ? false : !Model.PriceDisabled);
    }

    private void OnDestroy()
    {
        ItemSelectedEvent.RemoveAllListeners();
    }
}
