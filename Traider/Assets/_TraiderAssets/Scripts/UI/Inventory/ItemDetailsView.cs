using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemDetailsView : MonoBehaviour
{
    [SerializeField]
    private Button _closeBtn;
    [SerializeField]
    private Button _actionBtn;
    [SerializeField]
    private TMPro.TextMeshProUGUI _actionButtonText;
    [SerializeField]
    private GameObject[] _actionGameObjects;

    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemName;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemDescription;
    [SerializeField]
    private TMPro.TMP_InputField _itemCount;
    [SerializeField]
    private Button _moreBtn;
    [SerializeField] 
    private Button _lessBtn;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemTotalPrice;

    [SerializeField]
    private GameObject _itemQuantityPanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI _itemQuantityText;

    public UnityEvent<OnSubmitEventArgs> OnSubmitEvent = new UnityEvent<OnSubmitEventArgs>();
    public UnityEvent<InventoryItemModel> OnCloseEvent = new UnityEvent<InventoryItemModel>();

    public InventoryItemModel Model { get; private set; }
    private int _totalprice;
    private int _currentCount;
    private int _totalBudget;
    public ItemDetailsViewState State { get; private set; } = ItemDetailsViewState.View;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actionBtn.onClick.RemoveListener(SubmitAction);
        _actionBtn.onClick.AddListener(SubmitAction);

        _closeBtn.onClick.RemoveListener(CloseDetails);
        _closeBtn.onClick.AddListener(CloseDetails);

        _moreBtn.onClick.RemoveListener(IncreaseItemCount);
        _moreBtn.onClick.AddListener(IncreaseItemCount);

        _lessBtn.onClick.RemoveListener(DecreaseItemCount);
        _lessBtn.onClick.AddListener(DecreaseItemCount);

        _itemCount.onValueChanged.RemoveListener(OnCountTextChanged);
        _itemCount.onValueChanged.AddListener(OnCountTextChanged);
    }

    public void InitializeView(InventoryItemModel model, ItemDetailsViewState state = ItemDetailsViewState.View, int totalBudget = 0)
    {
        if(model == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        Model = model;
        State = state;
        _totalBudget = totalBudget;
        _currentCount = 0;
        RefreshView();
    }

    public void UpdateView(int quantityDelta, int totalBudget = 0)
    {
        _totalBudget = totalBudget;

        if (State == ItemDetailsViewState.Sell)
        {
            Model.Quantity -= quantityDelta;
        }
        else if(!Model.InfiniteQuantity)
        {
            Model.Quantity -= quantityDelta;
        }

        RefreshView();
    }

    private void RefreshView()
    {
        for (int i = 0; i < _actionGameObjects.Length; i++)
        {
            _actionGameObjects[i].SetActive(State != ItemDetailsViewState.View);
        }

        _itemImage.sprite = Model.Image;
        _itemName.text = Model.Name;
        _itemDescription.text = Model.Description;
        _itemQuantityPanel.SetActive(!Model.InfiniteQuantity);
        _itemQuantityText.text = Model.Quantity.ToString();
        
        if(State != ItemDetailsViewState.View)
        {
            _actionButtonText.text = State == ItemDetailsViewState.Sell ? "Sell" : "By";
        }

        UpdateTotalPrice(_currentCount);
    }

    private void OnCountTextChanged(string arg0)
    {
        if(int.TryParse(arg0, out int newPrice))
        {
            UpdateTotalPrice(newPrice);
        }
        else
        {
            _itemCount.SetTextWithoutNotify(_currentCount.ToString());
        }
    }

    [SerializeField]
    private float _pressedTimer = 0.5f;

    private float _pressedTimerCurrentValue = 0.0f;
    private bool _isPressed = false;
    private bool? _isMorePressed = null;

    private void FixedUpdate()
    {
        if(_isPressed)
        {
            if(_pressedTimerCurrentValue < 0 && _isMorePressed.HasValue)
            {
                UpdateTotalPrice(_currentCount + (_isMorePressed.Value ? 1 : -1));
            }

            _pressedTimerCurrentValue -= Time.deltaTime;
        }
    }


    public void OnMorePressed()
    {
        PressStarted(true);
    }

    public void OnLessPressed()
    {
        PressStarted(false);
    }

    public void OnReleased()
    {
        PressReleased();
    }

    private void PressStarted(bool isMore)
    {
        _isMorePressed = isMore;
        _isPressed = true;
        _pressedTimerCurrentValue = _pressedTimer;

    }

    private void PressReleased()
    {
        _isMorePressed = null;
        _isPressed = false;
        _pressedTimerCurrentValue = 0.0f;
    }

    private void DecreaseItemCount()
    {
        UpdateTotalPrice(_currentCount - 1);
    }

    private void IncreaseItemCount()
    {
        UpdateTotalPrice(_currentCount + 1);
    }

    private void CloseDetails()
    {
        OnCloseEvent?.Invoke(Model);
        gameObject.SetActive(false);
    }

    private void SubmitAction()
    {
        OnSubmitEvent?.Invoke(new OnSubmitEventArgs()
        {
            Model = Model,
            State = State,
            TotalCount = _currentCount
        });
    }

    private void UpdateTotalPrice(int newCount)
    {
        _lessBtn.interactable = true;
        _moreBtn.interactable = true;

        if (newCount <= 0)
        {
            _lessBtn.interactable = false;
            newCount = 0;

            if (_isPressed) PressReleased();
        }
        else if(newCount >= Model.Quantity && !Model.InfiniteQuantity)
        {
            newCount = Model.Quantity;
            _moreBtn.interactable = false;
            _currentCount = Model.Quantity;
        }

        _currentCount = newCount;

        _totalprice = _currentCount * Model.ShopPrice;
        _itemTotalPrice.text = _totalprice.ToString();

        bool needUpdateCountText = true;
        if(int.TryParse(_itemCount.text, out int parsedCount))
        {
            needUpdateCountText = parsedCount != _currentCount;
        }

        if (needUpdateCountText)
        {
            _itemCount.SetTextWithoutNotify(_currentCount.ToString());
        }

        bool canHandleMore = ((_totalprice + Model.ShopPrice) <= _totalBudget || State == ItemDetailsViewState.Sell);
        _moreBtn.interactable = canHandleMore;

        if (!canHandleMore && _isPressed)
        {
            PressReleased();
        }

        bool iscanHandlePrice = (_totalprice <= _totalBudget || State == ItemDetailsViewState.Sell);

        if (!iscanHandlePrice)
        {
            var maxHandleCount = _totalBudget / Model.ShopPrice;
            UpdateTotalPrice(maxHandleCount);
        }

        _actionBtn.interactable = (_totalprice > 0 || _currentCount > 0);
    }

    public class OnSubmitEventArgs
    {
        public InventoryItemModel Model { get; set; }
        public int TotalCount { get; set; }
        public ItemDetailsViewState State { get; set; }
    }

    public enum ItemDetailsViewState
    {
        View,
        Sell, //Sell to shop
        By, // from shop
    }
}
