using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UserInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject _userInventoryPanel;

    [SerializeField]
    private InventoryGridView _inventoryGridView;

    [SerializeField]
    private ItemDetailsView _itemDetails;

    public static UserInventory Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null )
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _inventoryGridView.InventoryItemClickedEvent.RemoveListener(OnUserItemDetails);
        _inventoryGridView.InventoryItemClickedEvent.AddListener(OnUserItemDetails);
    }

    private void OnUserItemDetails(InventoryItemView arg0)
    {
        _itemDetails.InitializeView(arg0.Model, ItemDetailsView.ItemDetailsViewState.View, 0);
    }


    public List<InventoryItemModel> UserItems { get; set; } = new List<InventoryItemModel>();
    public int UserMoney { get; set; } = 100000;

    public void ShowUserInventory()
    {
        _userInventoryPanel.SetActive(true);
        _itemDetails.InitializeView(null, ItemDetailsView.ItemDetailsViewState.View, 0);

        foreach (InventoryItemModel item in UserItems)
        {
            item.PriceDisabled = true;
            item.Disabled = false;
        }

        _inventoryGridView.InitializeItems(UserItems);
        _inventoryGridView.InitializeTitle("My Items", UserMoney);
    }

    public void Close()
    {
        _userInventoryPanel.SetActive(false);
    }

    /// <summary>
    /// Update user balance
    /// </summary>
    /// <param name="model">What</param>
    /// <param name="quantity">How many</param>
    /// <param name="dealPrice">what total price</param>
    /// <param name="userSelling">user sell or by</param>
    /// <returns></returns>
    public UserTransactionResult CommitTransaction(InventoryItemModel model, int quantity, int dealPrice, bool userSelling)
    {
        var userBalance = 0;
        UserTransactionResult state = UserTransactionResult.UnnounFail;

        var deltaP = userSelling ? dealPrice : -1 * dealPrice;

        if (UserMoney + deltaP < 0)
        {
            return UserTransactionResult.NotEnufMoney;
        }

        var targetItem = UserItems.FirstOrDefault(i => i.ItemId == model.ItemId);

        UserTransactionResult __UpdateUserData(InventoryItemModel target_, int deltaP_, int quantity_, bool userSelling_)
        {
            
            var state_ = UserTransactionResult.NotEnufMoney;

            var deltaQ = userSelling_ ? -1 * quantity_ : quantity_;

            if (target_.Quantity + deltaQ >= 0)
            {
                targetItem.Quantity += deltaQ;
                UserMoney += deltaP_;
                userBalance = UserMoney;
                state_ = UserTransactionResult.Success;
            }

            if(target_.Quantity == 0)
            {
                UserItems.Remove(target_);
            }

            return state_;
        }

        if (targetItem != null)
        {
            state = __UpdateUserData(targetItem, deltaP, quantity, userSelling);
        }
        else if(userSelling)
        {
            state = UserTransactionResult.ItemNotFound;
        }
        else
        {
            targetItem = model.GetCopy();
            UserItems.Add(targetItem);
            state = __UpdateUserData(targetItem, deltaP, quantity, userSelling);
        }

        return state;
    }

    public enum UserTransactionResult
    {
        UnnounFail,
        Success,
        NotEnufMoney,
        ItemNotFound,
        NotEnufQuantity
    }
}
