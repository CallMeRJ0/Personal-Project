using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            transform.position = new Vector3(playerPosition.x + offset.x, transform.position.y, playerPosition.z + offset.z);
        }
    }
}
