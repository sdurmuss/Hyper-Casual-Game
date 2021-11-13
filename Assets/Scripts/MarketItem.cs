using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public int itemId, wearId;
    public int price;

    public Button buyButton, equipButton, unequipButton;
    public Text priceText;

    public GameObject itemPrefab;

    public bool HasItem() //item varmý kontrolü
    {
        // 0: Daha satýn alýnmamýþ
        // 1: Satýn alýnmýþ fakat giyilmemiþ
        // 2: Satýn alýnmýþ ve giyilmiþ olarak hafýzada tutulacak.

        bool hasItem = PlayerPrefs.GetInt("item" + itemId.ToString()) != 0;
        return hasItem;
    }
    
    public bool IsEquipped()
    {
        bool equippedItem = PlayerPrefs.GetInt("item" + itemId.ToString()) == 2;
        return equippedItem;
    }
    
    public void InitializeItem()
    {
        priceText.text = price.ToString();
        if (HasItem()) //alýnmýþsa
        {
            buyButton.gameObject.SetActive(false);
            if (IsEquipped())
            {
                EquipItem();
            }
            else
            {
                equipButton.gameObject.SetActive(true);
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
        }
    }
    
    public void BuyItem()
    {
        if (!HasItem())
        {
            int money = PlayerPrefs.GetInt("money");
            if (money >= price)
            {
                PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.buyAudioClip, 0.3f);
                LevelController.current.GiveMoneyToPlayer(-price);
                PlayerPrefs.SetInt("item" + itemId.ToString(), 1);
                buyButton.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(true);
            }
        }
    }

    public void EquipItem()
    {
        UnequipItem();
        MarketController.current.equippedItems[wearId] = Instantiate(itemPrefab, PlayerController.current.wearSpots[wearId].transform).GetComponent<Item>();
        MarketController.current.equippedItems[wearId].itemId = itemId;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("item" + itemId.ToString(), 2);
    }

    public void UnequipItem()
    {
        Item equippedItem = MarketController.current.equippedItems[wearId];
        if (equippedItem != null)// karaktede eþya giyilmiþse
        {
            MarketItem marketItem = MarketController.current.items[equippedItem.itemId];
            PlayerPrefs.SetInt("item" + marketItem.itemId.ToString(), 1);
            marketItem.equipButton.gameObject.SetActive(true);
            marketItem.unequipButton.gameObject.SetActive(false);
            Destroy(equippedItem.gameObject);
        }
    }

    public void UnequipItemButton()
    {
        PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.unequipAudioClip, 0.3f);        
        UnequipItem();
    }

    public void EquipItemButton()
    {
        PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.equipAudioClip, 0.3f);
        EquipItem();
    }
}
