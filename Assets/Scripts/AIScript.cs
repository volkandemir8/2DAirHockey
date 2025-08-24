using System.Collections;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public bool canMove = false;

    [HideInInspector] public Rigidbody2D rb;
    public Rigidbody2D ballRb;

    public float maxMoveSpeed = 3f;

    public Vector2 startingPosition;
    private Vector2 targetPosition;

    private bool isFirstTime = true;
    private float offsetXFromTarget;

    [Header("AI sinir noktalari")]
    public Transform upBoundary;
    public Transform downBoundary;
    public Transform leftBoundary;
    public Transform rightBoundary;

    [Header("Top sinir noktalari")]
    public Transform ballUpBoundary;
    public Transform ballDownBoundary;
    public Transform ballLeftBoundary;
    public Transform ballRightBoundary;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (ballRb.GetComponent<BallController>().levelType == "PlayerVsClassic")
        {
            StartCoroutine(StartDelay());
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float moveSpeed;

        if (ballRb.position.y < ballDownBoundary.position.y)
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                offsetXFromTarget = Random.Range(-1f, 1f);
            }

            moveSpeed = maxMoveSpeed * Random.Range(0.1f, 0.3f);
            targetPosition = new Vector2
                (Mathf.Clamp(ballRb.position.x + offsetXFromTarget, leftBoundary.position.x, rightBoundary.position.x), startingPosition.y);
        }
        else
        {
            isFirstTime = true;

            moveSpeed = Random.Range(maxMoveSpeed * 0.4f, maxMoveSpeed);
            targetPosition = new Vector2
                (Mathf.Clamp(ballRb.position.x, leftBoundary.position.x, rightBoundary.position.x), 
                Mathf.Clamp(ballRb.position.y, downBoundary.position.y, upBoundary.position.y));
        }

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }


    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);

        canMove = true;

        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        playerController.canMove = true;
    }


    /*
     1.	Hareket Kontrol�:
        �	E�er canMove false ise, AI hareket etmez ve h�z� s�f�rlan�r.

     2.	Topun Pozisyonuna G�re AI'n�n Hedefi ve H�z�:
        �	E�er top, ballDownBoundary'nin alt�nda ise:
        �	AI, ilk defa bu b�lgeye giriyorsa rastgele bir X offseti belirler.
        �	AI daha yava� hareket eder (maksimum h�z�n %10-30'u).
        �	Hedef pozisyon, topun X pozisyonuna offset eklenerek ve yatay s�n�rlar aras�nda k�s�tlanarak belirlenir. Y pozisyonu sabit kal�r.
        �	E�er top, yukar�daysa:
        �	AI'n�n ilk defa bu b�lgeye girdi�i bilgisi s�f�rlan�r.
        �	AI daha h�zl� hareket eder (maksimum h�z�n %40-100'� aras�).
        �	Hedef pozisyon, topun X ve Y pozisyonuna g�re ve s�n�rlar dahilinde belirlenir.

      3. Hareketin Uygulanmas�:
        �	AI, hedef pozisyona do�ru belirlenen h�zda hareket ettirilir.
     */
}