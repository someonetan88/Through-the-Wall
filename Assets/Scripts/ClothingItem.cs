using UnityEngine;

[CreateAssetMenu(fileName = "NewClothingItem", menuName = "Shop/Clothing Item")]
public class ClothingItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public GameObject modelPrefab;
    public ClothingType type;
    public bool isUnlocked = false;
}

public enum ClothingType
{
    Male,
    Female
}
