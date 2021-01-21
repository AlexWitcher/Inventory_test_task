using System;
using TestTask.Data;
using UnityEngine;

namespace TestTask.Gameplay.Backpack
{
    [Serializable]
    public class ItemHolder
    {
        [SerializeField]
        private Transform _container;
        [SerializeField]
        private ItemType _itemType;

        public ItemType ItemType
        {
            get => _itemType;
        }

        //container for the special picked item
        public Transform Container
        {
            get => _container;
        }
    }
}

