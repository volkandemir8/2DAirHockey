using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PocaAgentScript : Agent
{
    public bool canMove = true;

    [HideInInspector] public Rigidbody2D rb;
    public Rigidbody2D ballRb;
    public Rigidbody2D enemyRb;

    public float moveSpeed = 5f;

    [HideInInspector] public Vector2 startingPosition;

    public Transform ownGoal;
    public Transform enemyGoal;

    public Transform upBoundary;
    public Transform downBoundary;
    public Transform leftBoundary;
    public Transform rightBoundary;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(rb.linearVelocity);

        sensor.AddObservation(ballRb.gameObject.transform.localPosition);
        sensor.AddObservation(ballRb.linearVelocity);

        sensor.AddObservation(enemyRb.gameObject.transform.localPosition);
        sensor.AddObservation(enemyRb.linearVelocity);

        sensor.AddObservation(ownGoal.localPosition);
        sensor.AddObservation(enemyGoal.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float moveY = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        Vector2 move = new Vector2(moveX, moveY);

        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = move * moveSpeed;

        ClampPosition();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
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