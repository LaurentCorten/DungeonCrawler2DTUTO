using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float timeOffset = 0.2f;
    public Vector3 positionOffset;
    private Vector3 velocity;

    private void Awake()
    {
        transform.position = player.position + positionOffset;
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + positionOffset, ref velocity, timeOffset);
    }
}
