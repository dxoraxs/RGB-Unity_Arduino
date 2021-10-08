using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Singleton<ItemSpawner>
{
    [SerializeField] private ItemSavedColor prefab;
    private List<ItemSavedColor> items = new List<ItemSavedColor>();

    public static void SpawnListItems(List<ItemColor> list) => Instance.SpawnList(list);

    public void SpawnList(List<ItemColor> list)
    {
        var itemsRework = new List<ItemSavedColor>(items);
        var cloneList = new List<ItemColor>(list);

        for (int i = 0; i < itemsRework.Count; i++)
        {
            if (itemsRework[i].item != null && cloneList.Contains(itemsRework[i].item))
            {
                cloneList.Remove(itemsRework[i].item);
                itemsRework.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < cloneList.Count && i < itemsRework.Count; i++)
        {
            itemsRework[i].Enable();
            itemsRework[i].InitItem(cloneList[i]);
        }

        if (cloneList.Count < itemsRework.Count)
        {
            for (int i = cloneList.Count; i < itemsRework.Count; i++)
            {
                itemsRework[i].Disable();
            }
        }
        else
        {
            for (int i = itemsRework.Count; i < cloneList.Count; i++)
            {
                var newItem = Instantiate(prefab, transform);
                newItem.InitItem(cloneList[i]);
                items.Add(newItem);
            }
        }
    }
}