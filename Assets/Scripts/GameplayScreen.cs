using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour, IGameplayMediator
{
    public static GameplayScreen instance;

    public GameManager gameManager;
    public Button submitButton;
    public TextMeshProUGUI statusText;

    private Color targetColor;

    // all the view reference
    [FormerlySerializedAs("colorSelector")]
    public PaletteGridView paletteGridView;

    public GuessGridView guessGridView;
    public PieChartView pieChartView;

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

    private void Start()
    {
        targetColor = paletteGridView.GetTargetColor();
        pieChartView.SetTargetAndResetGuess(targetColor);
        //
        submitButton.onClick.AddListener(OnSubmitGuess);
        gameManager.onGameOver.AddListener(OnGameOver);
    }

    public void ResetGame(Color mixColor)
    {
        pieChartView.ResetGuessColors(mixColor);
        gameManager.CurrentIndex = 0;
    }

    private void OnSubmitGuess()
    {
        // Collect selected colors
        var guessColors = new Color[3];
        var selected = 0;
        for (var i = 0; i < 3; i++)
        {
            if (gameManager.SelectedColorIndices[i] >= 0)
            {
                guessColors[i] = paletteGridView.paletteColors[gameManager.SelectedColorIndices[i]];
                selected++;
            }
            else
            {
                guessColors[i] = Color.white;
            }
        }

        if (selected < 3)
        {
            Debug.Log("Please select 3 colors before submitting your guess.");
            return;
        }

        // Calculate total percentage and compare with target
        var targetMixColor = paletteGridView.GetMixColor(guessColors);
        if (targetColor != targetMixColor)
        {
            targetMixColor.a = 1;
            guessGridView.guessGridSlotsMix[gameManager.Attempts++].color = targetMixColor;
            gameManager.onWrongGuess?.Invoke();
            ResetGame(targetMixColor);
            Debug.Log($"guess color ({targetMixColor}%) does not match target color ({targetColor}%).");
            return;
        }

        targetMixColor.a = 1;
        guessGridView.guessGridSlotsMix[gameManager.Attempts++].color = targetMixColor;
        gameManager.onGameOver?.Invoke(true);
        ResetGame(targetMixColor);
        Debug.Log($"Guess submitted. Resulting color: {targetMixColor}");
    }

    private void OnGameOver(bool won)
    {
        statusText.text = won ? "You Win!" : "Game Over!";
        submitButton.interactable = false;
    }

    public void OnDeleteLastGuessColor()
    {
        if (gameManager.CurrentIndex <= 0)
            return;
        gameManager.CurrentIndex -= 1;
        GameManager.instance.SelectedColorIndices[gameManager.CurrentIndex] = -1;
        // Reset the corresponding guess grid slot
        guessGridView.GetGuessGridSlot(GameManager.instance.Attempts)[gameManager.CurrentIndex].color = Color.white;
        // Reset the pie chart guess index and re-apply remaining colors
        pieChartView.SetGuessColors(Color.gray, gameManager.CurrentIndex);
    }

    public void Notify(object sender, GameplayEvent eventCode, object selectedColor)
    {
        switch (eventCode)
        {
            case GameplayEvent.PaletteColorClicked:
                pieChartView.SetGuessColors((Color)selectedColor, GameManager.instance.CurrentIndex);
                guessGridView.UpdateGuessGridUI();
                GameManager.instance.CurrentIndex += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventCode), eventCode, null);
        }
    }
}