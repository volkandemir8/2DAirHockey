using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public string levelType;

    public float bounceForce = 10f;
    public float maxSpeed = 8f; // Maksimum hýz limiti
    public float minSpeed = 0.5f; // Minimum hýz esigi
    public float unstuckForce = 2f; // Takilinca uygulanacak ekstra kuvvet

    [HideInInspector] public bool isGoal;

    [HideInInspector] public Rigidbody2D rb;

    public Vector2 startingPosition;

    public ScoreScript scoreScript;
    public AgentScript agentScript;

    private void Awake()
    {
        isGoal = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 velocity = rb.linearVelocity;

        Vector2 reflectedVelocity = Vector2.Reflect(velocity, normal);
        rb.linearVelocity = reflectedVelocity.normalized * velocity.magnitude;
        rb.AddForce(normal * bounceForce, ForceMode2D.Impulse);

        LimitSpeed();
    }

    private void Update()
    {
        LimitSpeed();
        UnstuckIfNeeded();
    }

    private void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void UnstuckIfNeeded()
    {
        if (rb.linearVelocity.magnitude < minSpeed)
        {
            // Rastgele bir yon sec ve ekstra kuvvet uygula
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDir * unstuckForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGoal)
        {
            if (collision.CompareTag("RedGoal"))
            {
                isGoal = true;

                if (levelType == "PlayerVsClassic")
                {
                    if (scoreScript != null)
                        scoreScript.IncreaseBlueScore();

                    StartCoroutine(ResetBall());
                }
                else
                {
                    if (scoreScript != null)
                        scoreScript.IncreaseBlueScore();

                    if (agentScript != null)
                    {
                        agentScript.AddReward(1f);
                        agentScript.EndEpisode();
                    }
                }
            }
            else if (collision.CompareTag("BlueGoal"))
            {
                isGoal = true;

                if (levelType == "PlayerVsClassic")
                {
                    if (scoreScript != null)
                        scoreScript.IncreaseRedScore();

                    StartCoroutine(ResetBall());
                }
                else
                {
                    if (scoreScript != null)
                        scoreScript.IncreaseRedScore();

                    if (agentScript != null)
                        agentScript.EndEpisode();
                }
            }
        }
    }

    private IEnumerator ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        AIScript aiScript = FindFirstObjectByType<AIScript>();
        if (aiScript != null)
        {
            aiScript.canMove = false;
            aiScript.rb.linearVelocity = Vector2.zero;
            aiScript.transform.position = aiScript.startingPosition;
        }

        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.rb.linearVelocity = Vector2.zero;
            playerController.transform.position = playerController.startingPosition;
        }

        yield return new WaitForSeconds(1f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (aiScript != null)
        {
            aiScript.canMove = true;
        }
        if (playerController != null)
        {
            playerController.canMove = true;
        }

        isGoal = false;
    }
}