using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Tooltip("Number of guesses allowed.")]
    public int maxAttempts = 6;

    [Tooltip("Event fired when the game ends.")]
    public UnityEvent<bool> onGameOver;

    public UnityEvent onRestartGame;
    public UnityEvent onWrongGuess;

    public int CurrentIndex { get; set; }
    public int Attempts { get; set; }

    public int[] SelectedColorIndices { get; set; } = new int[3] { -1, -1, -1 };
    public int[] TargetColorIndices { get; set; } = new int[3] { -1, -1, -1 };

    [FormerlySerializedAs("colorSelector")]
    public PaletteGridView paletteGridView;


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

    public void ResetGame()
    {
        for (var i = 0; i < SelectedColorIndices.Length; i++)
        {
            SelectedColorIndices[i] = -1;
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < SelectedColorIndices.Length; i++)
        {
            SelectedColorIndices[i] = -1;
        }

        Attempts = 0;
        onRestartGame?.Invoke();
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

    private bool ColorsEqual(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }
}