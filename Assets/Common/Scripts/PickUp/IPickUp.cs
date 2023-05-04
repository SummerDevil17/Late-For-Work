using UnityEngine;

public interface IPickUp
{
    Transform PickUpTransform { get; }
    void PickUp(Transform pickUpper);
}
