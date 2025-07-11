using MB;
using TMPro;
using UnityEngine;

public class WinScreen : UIScreen
{
    [SerializeField] private TextMeshProUGUI textStatus;

    public void OnClickBtnBack()
    {
        GameManager.instance.OnQuitGame();
    }

    public void OnClickBtnNext()
    {
        Hide();
        GameManager.instance.RestartGame();
    }

    public void OnGameOver()
    {
        textStatus.text = "Game Over, You Win!";
    }

    public void OnGameLose()
    {
        textStatus.text = "Game Over, You Lose!";
    }
}