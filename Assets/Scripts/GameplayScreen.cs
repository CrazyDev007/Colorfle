using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    public static GameplayScreen instance;

    public ColorfleGameManager gameManager;
    public Button submitButton;
    public TextMeshProUGUI statusText;

    // all the view reference
    [FormerlySerializedAs("colorSelector")]
    public PaletteGridView paletteGridView;

    public GuessGridView guessGridView;
    public PieChartView pieChartView;

    public int selectionCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pieChartView != null && paletteGridView != null)
        {
            var targetColor = paletteGridView.GetTargetColor();
            pieChartView.SetTargetAndResetGuess(targetColor);
        }

        submitButton.onClick.AddListener(OnSubmitGuess);
        gameManager.onGameOver.AddListener(OnGameOver);
    }

    void OnSubmitGuess()
    {
        ColorfleGameManager.instance.OnSubmitGuess();
    }

    private void OnGameOver(bool won)
    {
        statusText.text = won ? "You Win!" : "Game Over!";
        submitButton.interactable = false;
    }
}