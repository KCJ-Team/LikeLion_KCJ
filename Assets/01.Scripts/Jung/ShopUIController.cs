using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopUIController : MonoBehaviour
{
    public GameObject shopPanel;

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
}
