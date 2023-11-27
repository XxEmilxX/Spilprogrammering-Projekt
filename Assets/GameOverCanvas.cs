using UnityEngine;

public class GameOverCanvas : Singleton<GameOverCanvas>
{
    [SerializeField] private GameObject wonCanvas, lostCanvas;

    public void GameWon()
    {
        lostCanvas.SetActive(false);
        wonCanvas.SetActive(true);
    }

    public void GameOver()
    {
        wonCanvas.SetActive(false);
        lostCanvas.SetActive(true);
    }
}
