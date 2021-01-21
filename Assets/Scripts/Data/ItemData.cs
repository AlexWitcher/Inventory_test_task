using System;
using TestTask.Gameplay;
using UnityEngine;

namespace TestTask.Data
{
    [Serializable]
    public class ItemData
    {
        public int Weight;
        public string Name;
        public int Id;
        public ItemType ItemType;
        public ItemObject Prefab;
        public Color UiColor;
    }

    public enum ItemType
    {
        First = 1,
        Second = 2,
        Third = 3
    }
}

