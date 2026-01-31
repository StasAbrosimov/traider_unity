using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopsListView : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopButtonPref;
    [SerializeField]
    private RectTransform _listContainer;

    [SerializeField]
    public UnityEvent<ShopModel> OnClickEvent = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeList(IList<ShopModel> _shops)
    {
        for (int i = _listContainer.childCount - 1; i >= 0; i--)
        {
            GameObject child = _listContainer.GetChild(i).gameObject;
            Destroy(child);
        }

        for (int i = 0; i < _shops.Count; i++)
        {
            var buttonGO = Instantiate(_shopButtonPref, _listContainer);
            var buttonView = buttonGO.GetComponent<ShopListButton>();
            buttonView.InitializeButton(_shops[i]);

            buttonView.OnClickEvent.RemoveListener(OnButtonPreset);
            buttonView.OnClickEvent.AddListener(OnButtonPreset);
        }
    }

    private void OnButtonPreset(ShopModel shop)
    {
        OnClickEvent?.Invoke(shop);
    }
}
