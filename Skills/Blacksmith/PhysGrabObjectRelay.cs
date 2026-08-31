using Photon.Pun;
using UnityEngine;

internal class PhysGrabObjectRelay : MonoBehaviour
{
    [PunRPC]
    public void OverrideIndestructibleRPC(float duration)
    {
        PhysGrabObject physGrabObject = GetComponent<PhysGrabObject>();

        if (physGrabObject != null)
            physGrabObject.OverrideIndestructible(duration);
    }
}
