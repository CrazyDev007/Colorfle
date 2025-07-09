using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorfleUIManager : MonoBehaviour
{
    public ColorfleGameManager gameManager;
    public TMP_Dropdown[] colorDropdowns; // 3 dropdowns for color selection
    public TMP_InputField[] percentInputs; // 3 input fields for percents
    public Button submitButton;
    public TextMeshProUGUI[] feedbackTexts; // 3 texts for feedback
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI statusText;

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
            guessColors[i] = gameManager.palette[colorDropdowns[i].value];
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
}