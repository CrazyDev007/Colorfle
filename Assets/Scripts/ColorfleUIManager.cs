using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorfleUIManager : MonoBehaviour
{
    public static ColorfleUIManager instance;

    public ColorfleGameManager gameManager;
    public Button submitButton;
    public TextMeshProUGUI[] feedbackTexts; // 3 texts for feedback
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI statusText;
    public ColorSelector colorSelector; // Reference to ColorSelector
    public GuessGridView guessGridView;
    public PieChartView pieChartView; // Reference to PieChart
    public Image[] paletteButtons; // Assign palette button images in Inspector

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
        SetPaletteButtonColors();
        if (pieChartView != null && colorSelector != null)
        {
            var targetColor = colorSelector.GetTargetColor();
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
            if (selectedColorIndices[i] >= 0 && colorSelector != null)
            {
                guessColors[i] = colorSelector.colorPercentages[selectedColorIndices[i]].color;
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
        var targetColor = colorSelector.targetColor;
        var targetMixColor = colorSelector.GetMixColor(guessColors);
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

    void ShowFeedback(ColorfleGameManager.FeedbackType[] feedback)
    {
        for (int i = 0; i < 3; i++)
            feedbackTexts[i].text = feedback[i].ToString();
        // Optionally update attemptsText here
    }

    void OnGameOver(bool won)
    {
        statusText.text = won ? "You Win!" : "Game Over!";
        submitButton.interactable = false;
    }

    // Call this from the palette color button's OnClick event, passing the color index
    public void OnPaletteColorClicked(int colorIndex)
    {
        if (colorSelector == null || colorIndex < 0 || colorIndex >= colorSelector.colorPercentages.Count)
        {
            statusText.text = $"Invalid color selection.";
            return;
        }

        // Prevent duplicate selection
        for (int i = 0; i < selectionCount; i++)
        {
            if (selectedColorIndices[i] == colorIndex)
            {
                statusText.text = $"Color already selected.";
                return;
            }
        }

        if (selectionCount >= 3)
        {
            statusText.text = $"You can only select 3 colors.";
            return;
        }

        selectedColorIndices[selectionCount] = colorIndex;
        selectionCount++;
        // Call PieChart.SetGuessColors with the selected color
        if (pieChartView != null)
        {
            var color = colorSelector.colorPercentages[colorIndex].color;
            pieChartView.SetGuessColor(color);
        }

        guessGridView.UpdateGuessGridUI();
        var colorInfo = colorSelector.colorPercentages[colorIndex];
        var c = colorInfo.color;
        statusText.text =
            $"Selected Color {colorIndex + 1}: RGB({(int)(c.r * 255)}, {(int)(c.g * 255)}, {(int)(c.b * 255)})";
    }

    public void SetPaletteButtonColors()
    {
        if (paletteButtons == null || colorSelector == null) return;
        for (int i = 0; i < paletteButtons.Length && i < colorSelector.colorPercentages.Count; i++)
        {
            paletteButtons[i].color = colorSelector.colorPercentages[i].color;
        }
    }
}