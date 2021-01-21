using System;
using UnityEngine;

namespace TestTask.Gameplay
{
    public class DragBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform _floorLevel;

        private ItemObject _selectedObject;
        private Vector3 _currentScreenProjection;
        private Vector3 _diff;

        private bool _isInDrag;

        private const int LAYER_MASK = 1 << 8;
        private const int BACKPACK_LAYER_MASK = 1 << 9;
        private const int RAYCAST_DISTANCE = 1000;

        public Action<ItemObject> ItemPushedToBag { get; set; }

        private Vector3 CurrentScreenPosition
        {
            get => new Vector3(Input.mousePosition.x, Input.mousePosition.y, _currentScreenProjection.z);
        }

        private Ray ScreenRay
        {
            get=> Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0) && _isInDrag)
            {
                if (Physics.Raycast(ScreenRay.origin, ScreenRay.direction, RAYCAST_DISTANCE, BACKPACK_LAYER_MASK))
                {
                    //item now over the backpack in screen space projection and we must push it to the backpack
                    ItemPushedToBag?.Invoke(_selectedObject);
                }
                else
                {
                    //item just dropped off
                    _selectedObject.SetState(ItemState.Free);
                }
                
                _selectedObject = null;
                _isInDrag = false;
                return;
            }

            //check is mouse over the "free" item, if yes, then we have to start drag process for it
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ScreenRay.origin, ScreenRay.direction, out var hit, RAYCAST_DISTANCE, LAYER_MASK))
                {
                    _selectedObject = hit.collider.gameObject.GetComponent<ItemObject>();
                    if (_selectedObject==null)
                        return;

                    if (_selectedObject.State == ItemState.InBackpack)
                    {
                        _selectedObject = null;
                        return;
                    }

                    _isInDrag = true;
                    _selectedObject.SetState(ItemState.InDrag);

                    _currentScreenProjection = Camera.main.WorldToScreenPoint(_selectedObject.transform.position);
                    _diff = _selectedObject.transform.position - Camera.main.ScreenToWorldPoint(CurrentScreenPosition);
                }
            }

            //calculating next position for the dragging item
            if (_isInDrag)
            {
                var newWorldPosition = Camera.main.ScreenToWorldPoint(CurrentScreenPosition) + _diff;
                if (newWorldPosition.y<=_floorLevel.position.y)
                    newWorldPosition = new Vector3(newWorldPosition.x, _floorLevel.position.y,newWorldPosition.z);

                _selectedObject.transform.position = newWorldPosition;
            }
        }
    }
}
