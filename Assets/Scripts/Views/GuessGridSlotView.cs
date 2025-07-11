using UnityEngine;
using UnityEngine.UI;

public class GuessGridSlotView : MonoBehaviour
{
    [SerializeField] private Image borderImage;
    [SerializeField] private Image colorImage;

    public Image ColorImage => colorImage;

    public void SetSlotColor(Color color)
    {
        colorImage.color = color;
    }

    public void SetBorderImageActive(bool active)
    {
        borderImage.gameObject.SetActive(active);
    }

    public void SetBorderColor(Color color)
    {
        borderImage.color = color;
    }
}