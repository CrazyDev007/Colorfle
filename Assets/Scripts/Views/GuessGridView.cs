using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GuessGridView : MonoBehaviour
{
    [FormerlySerializedAs("mColorSelector")] [SerializeField]
    private PaletteGridView mPaletteGridView;

    public Image[] guessGridSlots0;
    public Image[] guessGridSlots1;
    public Image[] guessGridSlots2;
    public Image[] guessGridSlots3;
    public Image[] guessGridSlots4;
    public Image[] guessGridSlots5;

    private GuessGridPresenter _presenter;

    [SerializeField] private GameplayScreen gameplayScreen;

    public PaletteGridView PaletteGridView => mPaletteGridView;

    private void Start()
    {
        _presenter = new GuessGridPresenter(this, gameplayScreen);
    }

    public void SetSlotColor(Color color, int index)
    {
        guessGridSlots1[index].color = color;
    }

    public void UpdateGuessGridUI()
    {
        _presenter.UpdateGuessGridUI();
    }
}