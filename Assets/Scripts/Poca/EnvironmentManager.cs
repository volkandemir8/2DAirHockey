using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public PocaAgentScript blueAgent;
    public PocaAgentScript redAgent;

    public Rigidbody2D ballRb;
    public Rigidbody2D blueRb;
    public Rigidbody2D redRb;

    public void BlueWin()
    {
        blueAgent.AddReward(1f);
        ResetScene();
    }

    public void RedWin()
    {
        redAgent.AddReward(1f);
        ResetScene();
    }

    private void ResetScene()
    {
        blueAgent.canMove = false;
        blueAgent.gameObject.transform.position = blueAgent.startingPosition;
        blueRb.linearVelocity = Vector2.zero;

        redAgent.canMove = false;
        redAgent.gameObject.transform.position = redAgent.startingPosition;
        redRb.linearVelocity = Vector2.zero;

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

        blueAgent.canMove = true;
        redAgent.canMove = true;

        ballRb.GetComponent<BallController>().isGoal = false;
    }
}