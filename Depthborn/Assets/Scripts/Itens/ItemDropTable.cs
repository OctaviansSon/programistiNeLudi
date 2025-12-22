using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Drop Table")]
public class ItemDropTable : ScriptableObject
{
    public ItemData[] possibleItems;

    public ItemData GetRandomItem()
    {
        if (possibleItems.Length == 0) return null;
        return possibleItems[Random.Range(0, possibleItems.Length)];
    }
}
    