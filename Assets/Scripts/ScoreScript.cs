using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI blueScoreText;
    public TMPro.TextMeshProUGUI redScoreText;

    private int blueScore = 0;
    private int redScore = 0;

    private void Start()
    {
        blueScore = 0;
        redScore = 0;
        blueScoreText.text = blueScore.ToString();
        redScoreText.text = redScore.ToString();
    }

    public void IncreaseBlueScore()
    {
        blueScore++;
        blueScoreText.text = blueScore.ToString();
    }

    public void IncreaseRedScore()
    {
        redScore++;
        redScoreText.text = redScore.ToString();
    }
}