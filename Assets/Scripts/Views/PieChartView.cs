using UnityEngine;
using UnityEngine.UI;

public class PieChartView : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Image[] guessImage;

    private PieChartPresenter _pieChartPresenter;

    public int GuessImagesLength => guessImage.Length;

    private void Awake()
    {
        _pieChartPresenter = new PieChartPresenter(this);
    }

    public void SetGuessColors(Color color, int currentIndex)
    {
        if (currentIndex <= 0)
        {
            ResetGuessColors(Color.gray);
        }

        color.a = 1;
        guessImage[currentIndex].color = color;
    }

    public void ResetPieChart(Color targetColor)
    {
        targetImage.color = targetColor;

        foreach (var img in guessImage)
        {
            img.color = Color.gray;
        }
    }

    public void ResetGuessColors(Color mixColor)
    {
        foreach (var img in guessImage)
        {
            img.color = mixColor;
        }
    }

    public void SetTargetAndResetGuess(Color targetColor)
    {
        _pieChartPresenter.SetTargetAndResetGuess(targetColor);
    }

    public void SetGuessColor(Color guessColor)
    {
        _pieChartPresenter.SetGuessColors(guessColor);
    }
}