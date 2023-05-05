using UnityEngine;

public class VFXTrigger : MonoBehaviour
{
    [SerializeField] GameObject vfxToInstantiate;

    public void Trigger() { Instantiate(vfxToInstantiate, transform.position, Quaternion.identity); }
}
