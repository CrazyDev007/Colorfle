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

    public Image[] guessGridSlotsMix;

    private GuessGridPresenter _presenter;

    [SerializeField] private GameplayScreen gameplayScreen;

    public Image[] GetGuessGridSlot(int attempt)
    {
        return attempt switch
        {
            0 => guessGridSlots0,
            1 => guessGridSlots1,
            2 => guessGridSlots2,
            3 => guessGridSlots3,
            4 => guessGridSlots4,
            _ => guessGridSlots5
        };
    }

    public PaletteGridView PaletteGridView => mPaletteGridView;

    private void Start()
    {
        _presenter = new GuessGridPresenter(this, gameplayScreen);
    }

    public void SetSlotColor(Color color, int index)
    {
        GetGuessGridSlot(GameManager.instance.Attempts)[index].color = color;
    }

    public void UpdateGuessGridUI()
    {
        _presenter.UpdateGuessGridUI();
    }
}