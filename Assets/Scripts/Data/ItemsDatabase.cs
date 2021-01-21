using UnityEngine;

namespace TestTask.Data
{

    [CreateAssetMenu(fileName = "ItemsData", menuName = "ScriptableObjects/ItemsData")]
    public class ItemsDatabase : ScriptableObject
    {
        [SerializeField] private ItemData[] _items;

        public ItemData[] Items
        {
            get => _items;
        }
    }
}

