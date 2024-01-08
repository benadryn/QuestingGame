using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendRay : MonoBehaviour
{
    public void SendHitRaycast(Ray ray, float maxDistance, LayerMask layerToHit)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerToHit))
        {
            Debug.Log("this ray hit an " + hit);
        }
    }
}
