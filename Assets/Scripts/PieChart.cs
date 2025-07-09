using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Image[] guessImage;
    private int currentIndex = 0;

    public void SetGuessColors(Color color)
    {
        if (guessImage != null && currentIndex < guessImage.Length)
        {
            guessImage[currentIndex].color = color;
            currentIndex++;
        }
    }

    public void ResetGuessIndex()
    {
        currentIndex = 0;
    }

    public void SetTargetAndResetGuess(Color targetColor)
    {
        if (targetImage != null)
        {
            Color opaqueTarget = targetColor;
            opaqueTarget.a = 1f;
            targetImage.color = opaqueTarget;
        }
        if (guessImage != null)
        {
            foreach (var img in guessImage)
            {
                if (img != null)
                    img.color = Color.gray;
            }
        }
        ResetGuessIndex();
    }
}