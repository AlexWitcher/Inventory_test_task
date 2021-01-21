using DG.Tweening;
using TestTask.Data;
using UnityEngine;

namespace TestTask.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemObject : MonoBehaviour
    {
        public int Id { get; private set; }
        public ItemType ItemType { get; private set; }
        public ItemState State { get; private set; }

        private Tween _positionTween;
        private Tween _rotationTween;

        public void SetData(int id, ItemType itemType, int weight)
        {
            Id = id;
            ItemType = itemType;
            GetComponent<Rigidbody>().mass = weight;
        }

        public void SetState(ItemState state)
        {
            State = state;

            var rigidbody = GetComponent<Rigidbody>();
            var collider = GetComponent<Collider>();

            switch (state)
            {
                case ItemState.Free:
                    if (_positionTween!=null)
                        _positionTween.Kill();
                    if (_rotationTween!=null)
                        _rotationTween.Kill();

                    rigidbody.freezeRotation = false;
                    rigidbody.useGravity = true;
                    transform.SetParent(null);
                    collider.isTrigger = false;
                    break;
                case ItemState.InDrag:
                case ItemState.InBackpack:
                    rigidbody.freezeRotation = true;
                    rigidbody.useGravity = false;
                    rigidbody.velocity = Vector3.zero;
                    collider.isTrigger = true;
                    break;
            }
        }

        public void StartTween()
        {
            _positionTween = transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutQuad);
            _rotationTween = transform.DOLocalRotate(Vector3.zero, 0.5f);
        }
    }

    public enum ItemState
    {
        Free,InDrag,InBackpack
    }
}

