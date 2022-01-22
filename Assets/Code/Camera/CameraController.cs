using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public float CameraOffsetY = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y + CameraOffsetY, transform.position.z);
    }
}
