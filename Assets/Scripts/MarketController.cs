using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketController : MonoBehaviour
{
    public static MarketController current;
    public List<MarketItem> items;
    public List<Item> equippedItems;
    public GameObject marketMenu;

    public void InitializeMarketController()
    {
        current = this;
        foreach(MarketItem item in items)
        {
            item.InitializeItem();
        }
    }

    public void ActivateMarketMenu(bool active) 
    {
        marketMenu.gameObject.SetActive(active);
    }
}
