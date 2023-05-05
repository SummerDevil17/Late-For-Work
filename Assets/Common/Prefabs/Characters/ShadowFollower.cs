using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (!player.IsGrounded)
        {
            Vector2 shadowPosition = transform.position;

            shadowPosition.y = player.StartingJumpY;
            transform.position = shadowPosition;
        }
    }
}
