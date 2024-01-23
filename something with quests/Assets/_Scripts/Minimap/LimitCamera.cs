using System;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void LateUpdate()
    {
        var playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, 40, playerPos.z);
    }
}
