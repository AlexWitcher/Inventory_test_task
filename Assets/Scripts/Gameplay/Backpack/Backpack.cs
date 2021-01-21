using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TestTask.API;
using TestTask.Gameplay;
using UnityEngine;

namespace TestTask.Gameplay.Backpack
{

    public class Backpack : MonoBehaviour
    {
        [SerializeField] private ItemHolder[] _holders;
        private List<ItemObject> _pickedItems = new List<ItemObject>();

        private DragBehavior _dragBehavior;
        private GameAPI _api;
        private Action<bool> _backpackClicked;

        private bool _uiOpened;

        public void Initialize(DragBehavior dragBehavior, GameAPI api, IEnumerable<ItemObject> pickedItems,
            Action<bool> backpackClicked)
        {
            _api = api;
            _api.ItemRemoved.AddListener(OnItemRemoved);
            _backpackClicked = backpackClicked;

            _dragBehavior = dragBehavior;
            _dragBehavior.ItemPushedToBag = OnItemPushedToBag;

            if (pickedItems == null)
                return;

            _pickedItems.AddRange(pickedItems);

            for (var i = 0; i < _pickedItems.Count; i++)
            {
                PlaceItemToHolder(_pickedItems[i]);
            }
        }

        void OnDestroy()
        {
            if (_api != null)
                _api.ItemRemoved.RemoveListener(OnItemRemoved);

            if (_dragBehavior != null)
                _dragBehavior.ItemPushedToBag = null;
        }

        void OnMouseDown()
        {
            _backpackClicked?.Invoke(true);
            _uiOpened = true;
        }

        void OnMouseUp()
        {
            if (!_uiOpened)
                return;

            _uiOpened = false;
            _backpackClicked?.Invoke(false);
        }

        private void OnItemPushedToBag(ItemObject item)
        {
            _api.PickItem(item.Id);
            _pickedItems.Add(item);
            PlaceItemToHolder(item, false);
        }

        private void OnItemRemoved(int id)
        {
            var targetItem = _pickedItems.FirstOrDefault(x => x.Id == id);
            if (targetItem == null)
                return;

            targetItem.SetState(ItemState.Free);
            _pickedItems.Remove(targetItem);
        }

        /// <summary>
        /// Place picked item to the backpack holder position for this item type
        /// </summary>
        /// <param name="item">picked item</param>
        /// <param name="instantly">if false - then item will be pushed to backpack with animation</param>
        private void PlaceItemToHolder(ItemObject item, bool instantly = true)
        {
            for (var i = 0; i < _holders.Length; i++)
            {
                if (_holders[i].ItemType != item.ItemType)
                    continue;

                item.SetState(ItemState.InBackpack);
                item.transform.SetParent(_holders[i].Container);

                if (instantly)
                {
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;
                    return;
                }

                item.StartTween();
            }
        }
    }
}
