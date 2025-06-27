using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<ClothingItem> availableItems;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public Transform character;

    private Dictionary<ClothingType, GameObject> equippedClothes = new();

    void Start()
    {
        PopulateShop(Register.Instance.type); // default category
    }

    public void PopulateShop(ClothingType category)
    {
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        foreach (var item in availableItems)
        {
            if (item.type == category)
            {
                var slotGO = Instantiate(slotPrefab, slotsParent);
                slotGO.GetComponent<ClothingSlotUI>().Initialize(item, this);
            }
        }
    }

    public void EquipClothing(ClothingItem item)
    {
        if (equippedClothes.TryGetValue(item.type, out var existing))
            Destroy(existing);

        GameObject newCloth = Instantiate(item.modelPrefab, character);
        equippedClothes[item.type] = newCloth;
        Register.Instance.playerGender = item.modelPrefab;
    }

    public bool TryUnlockClothing(ClothingItem item)
    {
        if (!item.isUnlocked && scoreManager.Instance.TrySpendCoins(item.price))
        {
            item.isUnlocked = true;
            return true;
        }
        return false;
    }

    public void ResetClothes()
    {
        foreach (var cloth in equippedClothes.Values)
            Destroy(cloth);
        equippedClothes.Clear();
    }
}
