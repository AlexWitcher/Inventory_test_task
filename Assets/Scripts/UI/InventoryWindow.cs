using System.Collections.Generic;
using System.Linq;
using TestTask.Data;
using TestTask.Gameplay.Backpack;
using UnityEngine;

namespace TestTask.UI
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField]
        private GameObject _windowContainer;
        [SerializeField]
        private InventoryItem _itemPrefab;

        private List<InventoryItem> _itemsList = new List<InventoryItem>(3);

        [SerializeField]
        private List<ItemHolder> _holders;

        public void Show(ICollection<ItemContent> itemsData)
        {
            for (int i = 0; i < itemsData.Count; i++)
            {
                var data = itemsData.ElementAt(i);
                if (_itemsList.Count <= i)
                {
                    CreateItem(data);
                    continue;
                }

                var targetItem = _itemsList.FirstOrDefault(x => x.Type == data.ItemType);
                if (targetItem!=null)
                    targetItem.SetData(data);
            }

            _windowContainer.SetActive(true);
        }

        public void Hide()
        {
            _windowContainer.SetActive(false);
        }

        //here, we will check, is the mouse over ui item representation
        public int GetOverredItedId()
        {
            foreach (var inventoryItem in _itemsList)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(inventoryItem.GetComponent<RectTransform>(),
                    Input.mousePosition))
                {
                    return inventoryItem.Id;
                }
            }

            return -1;
        }

        //here, we fill UI window with items representation
        private void CreateItem(ItemContent content)
        {
            var item = Instantiate(_itemPrefab);
            item.gameObject.SetActive(true);

            var holder = _holders.FirstOrDefault(x => x.ItemType == content.ItemType);
            if (holder == null)
                return;

            item.transform.SetParent(holder.Container);
            item.transform.localPosition = Vector3.zero;
            _itemsList.Add(item);
            item.SetData(content);
        }
    }

    public struct ItemContent
    {
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Color Color { get; set; }
        public int Id { get; set; }
    }
}
