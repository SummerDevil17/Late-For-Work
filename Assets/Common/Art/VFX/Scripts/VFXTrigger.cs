using UnityEngine;

public class VFXTrigger : MonoBehaviour
{
    [SerializeField] GameObject vfxToInstantiate;

    public void Trigger()
    {
        GameObject newInstantiate = Instantiate(vfxToInstantiate, transform.position, Quaternion.identity);
        newInstantiate.transform.parent = this.transform;
    }
}
