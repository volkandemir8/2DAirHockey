using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentScript : Agent
{
    public bool canMove = false;

    [HideInInspector] public Rigidbody2D rb;
    public Rigidbody2D ballRb;
    public Rigidbody2D enemyRb;

    public float moveSpeed = 6f;

    public Vector2 startingPosition;

    [Header("Kaleler")]
    public Transform ownGoal;
    public Transform enemyGoal;

    [Header("Agent sinir noktalari")]
    public Transform upBoundary;
    public Transform downBoundary;
    public Transform leftBoundary;
    public Transform rightBoundary;

    public float currentReward = 0f;

    public override void Initialize()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        currentReward = 0f;
        ResetScene();
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

        currentReward = GetCumulativeReward();
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

    private void ResetScene()
    {
        canMove = false;
        transform.position = startingPosition;
        rb.linearVelocity = Vector2.zero;

        if (ballRb.GetComponent<BallController>().levelType == "PlayerVsAgent")
        {
            enemyRb.GetComponent<PlayerController>().canMove = false;
            enemyRb.gameObject.transform.position = enemyRb.GetComponent<PlayerController>().startingPosition;
            enemyRb.linearVelocity = Vector2.zero;
        }
        else if (ballRb.GetComponent<BallController>().levelType == "AgentVsClassic")
        {
            enemyRb.GetComponent<AIScript>().canMove = false;
            enemyRb.gameObject.transform.position = enemyRb.GetComponent<AIScript>().startingPosition;
            enemyRb.linearVelocity = Vector2.zero;
        }
        else if (ballRb.GetComponent<BallController>().levelType == "AgentVsAgent")
        {
            enemyRb.GetComponent<AgentScript>().canMove = false;
            enemyRb.gameObject.transform.position = enemyRb.GetComponent<AgentScript>().startingPosition;
            enemyRb.linearVelocity = Vector2.zero;
        }

        ballRb.gameObject.transform.position = ballRb.GetComponent<BallController>().startingPosition;
        ballRb.linearVelocity = Vector2.zero;
        ballRb.constraints = RigidbodyConstraints2D.FreezeAll;

        StartCoroutine(StartAfterDelay());
    }

    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        ballRb.constraints = RigidbodyConstraints2D.None;
        ballRb.constraints = RigidbodyConstraints2D.FreezeRotation;

        canMove = true;

        if (ballRb.GetComponent<BallController>().levelType == "PlayerVsAgent")
        {
            enemyRb.GetComponent<PlayerController>().canMove = true;
        }
        else if (ballRb.GetComponent<BallController>().levelType == "AgentVsClassic")
        {
            enemyRb.GetComponent<AIScript>().canMove = true;
        }
        else if (ballRb.GetComponent<BallController>().levelType == "AgentVsAgent")
        {
            enemyRb.GetComponent<AgentScript>().canMove = true;
        }

        ballRb.GetComponent<BallController>().isGoal = false;
    }
}