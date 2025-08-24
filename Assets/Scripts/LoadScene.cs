using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void ExitToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void PlayerVsClassic()
    {
        SceneManager.LoadSceneAsync("PlayerVsClassic", LoadSceneMode.Single);
    }

    public void PlayerVsPPO()
    {
        SceneManager.LoadSceneAsync("PlayerVsPPO", LoadSceneMode.Single);
    }

    public void PlayerVsSAC()
    {
        SceneManager.LoadSceneAsync("PlayerVsSAC", LoadSceneMode.Single);
    }

    public void PPOVsClassic()
    {
        SceneManager.LoadSceneAsync("PPOVsClassic", LoadSceneMode.Single);
    }

    public void SACVsClassic()
    {
        SceneManager.LoadSceneAsync("SACVsClassic", LoadSceneMode.Single);
    }

    public void PPOvsSAC()
    {
        SceneManager.LoadSceneAsync("PPOvsSAC", LoadSceneMode.Single);
    }
}
