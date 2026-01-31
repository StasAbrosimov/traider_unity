using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopListButton : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _shopName;
    [SerializeField]
    private Button _btn;

    [SerializeField]
    public UnityEvent<ShopModel> OnClickEvent = new ();

    public ShopModel Model { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _btn.onClick.RemoveListener(OnbtnClick);
        _btn.onClick.AddListener(OnbtnClick);
    }

    public void InitializeButton(ShopModel model)
    {
        Model = model;
        _shopName.text = model.Name;
    }

    private void OnbtnClick()
    {
        OnClickEvent?.Invoke(Model);
    }

    private void OnDestroy()
    {
        OnClickEvent.RemoveAllListeners();
    }
}
