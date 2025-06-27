using UnityEngine;

public class TabButton : MonoBehaviour
{
    public ClothingType type;
    public ShopManager manager;

    public void OnTabClicked()
    {
        manager.PopulateShop(type);
    }
}
