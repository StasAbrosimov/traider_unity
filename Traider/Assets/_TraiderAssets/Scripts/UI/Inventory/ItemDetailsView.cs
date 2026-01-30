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

    public void InitializeView(InventoryItemModel model, ItemDetailsViewState state, int totalBudget)
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
        
        if(State != ItemDetailsViewState.View)
        {
            _actionButtonText.text = State == ItemDetailsViewState.Sell ? "Sell" : "By";
        }

        UpdateTotalPrice(0);
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
        }
        else if(newCount >= Model.Quantity && !Model.InfiniteQuantity)
        {
            newCount = Model.Quantity;
            _moreBtn.interactable = false;
            _currentCount = Model.Quantity;
        }

        _currentCount = newCount;

        _totalprice = _currentCount * Model.BasePrice;
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

        _actionBtn.interactable = (_totalprice > 0 || _currentCount > 0) && (_totalprice <= _totalBudget || State == ItemDetailsViewState.Sell);
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
        Sell,
        By,
    }
}
