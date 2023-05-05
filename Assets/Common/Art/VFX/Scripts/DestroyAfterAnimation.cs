using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyAfterAnimation : MonoBehaviour
{
    public void Destroy() { Destroy(this.gameObject); }
}
