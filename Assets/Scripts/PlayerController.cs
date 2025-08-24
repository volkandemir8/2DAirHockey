using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = false;

    [HideInInspector] public Rigidbody2D rb;
    public float moveSpeed = 7f;
    private Vector2 movementDirection;

    public Vector2 startingPosition;

    [Header("Oyuncu sinir noktalari")]
    public Transform upBoundary;
    public Transform downBoundary;
    public Transform leftBoundary;
    public Transform rightBoundary;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movementDirection = new Vector2(moveHorizontal, moveVertical);

        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movementDirection * moveSpeed;

        ClampPosition();
    }

    private void ClampPosition()
    {
        // Pozisyonu sinirla
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, downBoundary.position.y, upBoundary.position.y);
        rb.position = clampedPosition;
    }
}