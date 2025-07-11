using System;
using MB;
using UnityEngine;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

public class GameplayScreen : UIScreen, IGameplayMediator
{
    public static GameplayScreen instance;

    public GameManager gameManager;
    public Button submitButton;

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
        GameManager.instance.Attempts++;
        if (GameManager.instance.Attempts >= 6)
        {
            UIManager.Instance.ShowScreen(UIScreenType.Win);
            GameManager.instance.onGameLose?.Invoke();
        }
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
            guessGridView.OnWrongGuess();
            targetMixColor.a = 1;
            guessGridView.guessGridSlotsMix[gameManager.Attempts].SetSlotColor(targetMixColor);
            gameManager.onWrongGuess?.Invoke();
            //
            ResetGame(targetMixColor);
            Debug.Log($"guess color ({targetMixColor}%) does not match target color ({targetColor}%).");
            return;
        }

        targetMixColor.a = 1;
        guessGridView.guessGridSlotsMix[gameManager.Attempts].SetSlotColor(targetMixColor);
        gameManager.onGameOver?.Invoke(true);
        ResetGame(targetMixColor);
        UIManager.Instance.ShowScreen(UIScreenType.Win);
        Debug.Log($"Guess submitted. Resulting color: {targetMixColor}");
    }

    private void OnGameOver(bool won)
    {
        submitButton.interactable = false;
    }

    public void OnDeleteLastGuessColor()
    {
        if (gameManager.CurrentIndex <= 0)
            return;
        gameManager.CurrentIndex -= 1;
        GameManager.instance.SelectedColorIndices[gameManager.CurrentIndex] = -1;
        // Reset the corresponding guess grid slot
        guessGridView.GetGuessGridSlot(GameManager.instance.Attempts)[gameManager.CurrentIndex]
            .SetSlotColor(Color.white);
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

    public void OnRestartGame()
    {
        //reset palette grid
        paletteGridView.OnRestartGame();
        //reset pie chart
        targetColor = paletteGridView.GetTargetColor();
        pieChartView.OnRestartGame(targetColor);
        //reset guess grid
        guessGridView.OnRestartGame();
        //
        submitButton.interactable = true;
    }
}