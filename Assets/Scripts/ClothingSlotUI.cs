using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI price;
    public GameObject lockIcon;

    private ClothingItem item;
    private ShopManager shopManager;

    public void Initialize(ClothingItem newItem, ShopManager manager)
    {
        item = newItem;
        icon.sprite = item.icon;
        shopManager = manager;
        price.text = item.price.ToString();
        lockIcon.SetActive(!item.isUnlocked);
    }

    public void OnClick()
    {
        if (!item.isUnlocked)
        {
            if (shopManager.TryUnlockClothing(item))
            {
                lockIcon.SetActive(false);
            }
        }
        else
        {
            shopManager.EquipClothing(item);
        }
    }
}
