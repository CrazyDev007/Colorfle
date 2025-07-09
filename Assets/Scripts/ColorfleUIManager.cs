using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorfleUIManager : MonoBehaviour
{
    public ColorfleGameManager gameManager;
    public TMP_InputField[] percentInputs; // 3 input fields for percents
    public Button submitButton;
    public TextMeshProUGUI[] feedbackTexts; // 3 texts for feedback
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI statusText;
    public ColorSelector colorSelector; // Reference to ColorSelector

    private int[] selectedColorIndices = new int[3] { -1, -1, -1 }; // Stores indices of selected colors
    private int selectionCount = 0;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitGuess);
        gameManager.onGuessEvaluated.AddListener(ShowFeedback);
        gameManager.onGameOver.AddListener(OnGameOver);
    }

    void OnSubmitGuess()
    {
        Color[] guessColors = new Color[3];
        int[] guessPercents = new int[3];
        for (int i = 0; i < 3; i++)
        {
            // TODO: Replace with actual UI selection logic
            // Example: guessColors[i] = colorSelector.colorPercentages[selectedIndices[i]].color;
            guessColors[i] = Color.white; // Placeholder
            int.TryParse(percentInputs[i].text, out guessPercents[i]);
        }
        gameManager.SubmitGuess(guessColors, guessPercents);
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
        UpdateGuessGridUI();
        var colorInfo = colorSelector.colorPercentages[colorIndex];
        var c = colorInfo.color;
        statusText.text = $"Selected Color {colorIndex + 1}: RGB({(int)(c.r * 255)}, {(int)(c.g * 255)}, {(int)(c.b * 255)})";
    }

    // Placeholder: update the guess grid UI to show selected colors
    private void UpdateGuessGridUI()
    {
        // TODO: Implement actual guess grid UI update logic
        // For now, just log selected indices
        string msg = "Guess Grid: ";
        for (int i = 0; i < 3; i++)
        {
            if (selectedColorIndices[i] >= 0)
                msg += $"[{selectedColorIndices[i] + 1}] ";
            else
                msg += "[ ] ";
        }
        Debug.Log(msg);
    }
}