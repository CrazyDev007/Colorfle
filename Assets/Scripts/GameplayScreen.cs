using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour, IGameplayMediator
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

    public void OnDeleteLastGuessColor()
    {
        var gameManager = ColorfleGameManager.instance;
        if (gameManager.CurrentIndex <= 0)
            return;
        gameManager.CurrentIndex -= 1;
        ColorfleGameManager.instance.selectedColorIndices[gameManager.CurrentIndex] = -1;
        // Reset the corresponding guess grid slot
        guessGridView.guessGridSlots1[gameManager.CurrentIndex].color = Color.white;
        // Reset the pie chart guess index and re-apply remaining colors
        pieChartView.SetGuessColors(Color.gray, gameManager.CurrentIndex);
    }

    public void Notify(object sender, GameplayEvent eventCode, object selectedColor)
    {
        switch (eventCode)
        {
            case GameplayEvent.PaletteColorClicked:
                pieChartView.SetGuessColors((Color)selectedColor, ColorfleGameManager.instance.CurrentIndex);
                guessGridView.UpdateGuessGridUI();
                ColorfleGameManager.instance.CurrentIndex += 1;
                break;
        }
    }
}