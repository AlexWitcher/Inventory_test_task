using TestTask.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Text _count;
        
        public ItemType Type { private set; get; }
        public int Id { private set; get; }

        public void SetData(ItemContent content)
        {
            Id = content.Id;
            _icon.color = content.Color;
            _name.text = content.Name;
            _count.text = content.Count.ToString();
            Type = content.ItemType;
        }
    }
}


