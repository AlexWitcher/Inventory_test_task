using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.API;
using TestTask.Data;
using TestTask.UI;
using UnityEngine;

namespace TestTask.Gameplay
{
    public class SceneManager : MonoBehaviour, IGameManager
    {
        [SerializeField]
        private Backpack.Backpack _backpack;
        [SerializeField]
        private InventoryWindow _inventoryWindow;
        [SerializeField]
        private DragBehavior _dragBehavior;

        [SerializeField]
        private Transform[] _itemPoints;
        private Dictionary<int,ItemObject> _itemObjects = new Dictionary<int, ItemObject>(3);

        private Action _focusChanged;
        private ICollection<ItemData> _itemsData;
        private GameAPI _api;

        void Start()
        {
            ApplicationContext.Instance.RegisterManager(this);
        }

        public void Initialize(GameContent content)
        {
            if (content == null)
            {
                Debug.LogError("content can't be null");
                return;
            }

            _focusChanged = content.FocusChanged;
            _api = content.Api;
            _itemsData = content.ItemsData;

            CreateItems();

            var packedItems = _itemObjects.Values.Where(x => x.State == ItemState.InBackpack);
            _backpack.Initialize(_dragBehavior, _api, packedItems, OnBackpackClicked);
        }

        void OnApplicationQuit()
        {
            _focusChanged?.Invoke();
        }

        void OnApplicationPause(bool pause)
        {
            if (pause)
                _focusChanged?.Invoke();
        }

        //create items on the scene and check for every item, is it saved to the backpack or no
        private void CreateItems()
        {
            for (int i = 0; i < _itemsData.Count; i++)
            {
                var itemData = _itemsData.ElementAt(i);

                if (_itemObjects.ContainsKey(itemData.Id))
                    continue;

                var isInBackpack = _api.GetItemCount(itemData.Id) > 0;
                var itemObject = Instantiate(itemData.Prefab);

                itemObject.transform.position = _itemPoints[i].position;
                itemObject.SetData(itemData.Id,itemData.ItemType, itemData.Weight);
                itemObject.SetState(isInBackpack ? ItemState.InBackpack : ItemState.Free);

                _itemObjects[itemData.Id] = itemObject;
            }
        }

        private void OnBackpackClicked(bool status)
        {
            if (status)
            {
                ShowWindow();
                return;
            }
            
            CloseWindow();
        }

        private void ShowWindow()
        {
            var itemsContent = new List<ItemContent>();
            foreach (var itemData in _itemsData)
            {
                var count = _api.GetItemCount(itemData.Id);
                var contentItem = new ItemContent
                {
                    Count = count,
                    Color = itemData.UiColor,
                    ItemType = itemData.ItemType,
                    Id = itemData.Id,
                    Name = itemData.Name
                };

                itemsContent.Add(contentItem);
            }

            _dragBehavior.enabled = false;
            _inventoryWindow.Show(itemsContent);
        }

        private void CloseWindow()
        {
            _dragBehavior.enabled = true;

            var itemIdForDrop = _inventoryWindow.GetOverredItedId();
            _inventoryWindow.Hide();

            if (itemIdForDrop < 0)
                return;

            _api.DropItem(itemIdForDrop);
        }
    }

    public class GameContent
    {
        public GameAPI Api { get; set; }
        public ICollection<ItemData> ItemsData { get; set; }
        public Action FocusChanged { get; set; }
    }
}

