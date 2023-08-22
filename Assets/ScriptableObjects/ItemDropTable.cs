using UnityEngine;

[CreateAssetMenu(menuName = "Item Drop Table")]
public class ItemDropTable : ScriptableObject
{
    [System.Serializable]
    public class DropChance
    {
        public InventoryItemData item;
        [Range(0, 1)]
        public float chance;
    }

    public DropChance[] dropChances;

    public InventoryItemData DropItem()
    {
        float totalChance = 0;
        foreach (DropChance dropChance in dropChances)
        {
            totalChance += dropChance.chance;
        }

        float randomChance = Random.Range(0, totalChance);

        for (int i = 0; i < dropChances.Length; i++)
        {
            if (randomChance < dropChances[i].chance)
            {
                return dropChances[i].item;
            }
            randomChance -= dropChances[i].chance;
        }

        return null;
    }
}