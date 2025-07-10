using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ColorfleUIManager : MonoBehaviour
{
    public static ColorfleUIManager instance;

    public ColorfleGameManager gameManager;
    public Button submitButton;
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI statusText;
    [FormerlySerializedAs("colorSelector")] public PaletteGridView paletteGridView; // Reference to ColorSelector
    public GuessGridView guessGridView;
    public PieChartView pieChartView; // Reference to PieChart

    public int[] selectedColorIndices = new int[3] { -1, -1, -1 }; // Stores indices of selected colors
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
        //gameManager.onGuessEvaluated.AddListener(ShowFeedback);
        gameManager.onGameOver.AddListener(OnGameOver);
    }

    void OnSubmitGuess()
    {
        // Collect selected colors
        Color[] guessColors = new Color[3];
        int selected = 0;
        for (int i = 0; i < 3; i++)
        {
            if (selectedColorIndices[i] >= 0 && paletteGridView != null)
            {
                guessColors[i] = paletteGridView.paletteColors[selectedColorIndices[i]];
                selected++;
            }
            else
            {
                guessColors[i] = Color.white;
            }
        }

        if (selected < 3)
        {
            statusText.text = "Please select 3 colors before submitting your guess.";
            return;
        }

        // Calculate total percentage and compare with target
        var targetColor = paletteGridView.targetColor;
        var targetMixColor = paletteGridView.GetMixColor(guessColors);
        if (targetColor != targetMixColor)
        {
            statusText.text =
                $"guess color ({targetMixColor}%) does not match target color ({targetColor}%).";
            return;
        }

        //var feedback = gameManager.SubmitGuess(guessColors, guessPercents);
        // Optionally, show the resulting blended color
        //var result = colorSelector.GetTargetColor(); //gameManager.CalculateResultingColor(guessColors, guessPercents);
        statusText.text =
            $"Guess submitted. Resulting color: {targetMixColor}";
    }

    void OnGameOver(bool won)
    {
        statusText.text = won ? "You Win!" : "Game Over!";
        submitButton.interactable = false;
    }
}