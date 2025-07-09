using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the Colorfle game logic: generates a target color/percent combination,
/// processes guesses, and fires events for UI feedback.
/// </summary>
public class ColorfleGameManager : MonoBehaviour
{
    /// <summary>
    /// The maximum number of guesses allowed.
    /// </summary>
    [Tooltip("Number of guesses allowed.")]
    public int maxAttempts = 6;

    /// <summary>
    /// Invoked after each guess is evaluated, passing feedback for each slot.
    /// </summary>
    [Tooltip("Event fired after each guess is evaluated.")]
    public UnityEvent<FeedbackType[]> onGuessEvaluated;

    /// <summary>
    /// Invoked when the game ends. Passes true if player won, false otherwise.
    /// </summary>
    [Tooltip("Event fired when the game ends.")]
    public UnityEvent<bool> onGameOver;

    /// <summary>
    /// Feedback for each guessed slot.
    /// </summary>
    public enum FeedbackType { Correct, Misplaced, Absent }

    private Color[] targetColors = new Color[3];
    private int[] targetPercents = new int[3];
    private Color targetResultingColor; // The blended result color
    private int attempts;
    public ColorSelector colorSelector; // Reference to ColorSelector

    /// <summary>
    /// Initializes the game by selecting a random target.
    /// </summary>
    private void Start()
    {
        GenerateTarget();
        attempts = 0;
    }

    /// <summary>
    /// Submits a guess and evaluates it against the target.
    /// </summary>
    /// <param name="guessColors">Array of 3 guessed colors.</param>
    /// <param name="guessPercents">Array of 3 guessed percentages (sum = 100).</param>
    /// <returns>FeedbackType array for each slot.</returns>
    public FeedbackType[] SubmitGuess(Color[] guessColors, int[] guessPercents)
    {
        if (guessColors == null || guessPercents == null || guessColors.Length != 3 || guessPercents.Length != 3)
            throw new ArgumentException("Guess must have exactly 3 colors and 3 percents.");
        if (guessPercents.Sum() != 100)
            throw new ArgumentException("Guess percents must sum to 100.");

        var feedback = EvaluateGuess(guessColors, guessPercents);
        attempts++;
        onGuessEvaluated?.Invoke(feedback);

        bool won = feedback.All(f => f == FeedbackType.Correct);
        if (won || attempts >= maxAttempts)
        {
            onGameOver?.Invoke(won);
        }

        return feedback;
    }

    /// <summary>
    /// Evaluates the guess against the target and returns feedback for each slot.
    /// </summary>
    /// <param name="guessColors">Guessed colors.</param>
    /// <param name="guessPercents">Guessed percents.</param>
    /// <returns>FeedbackType array for each slot.</returns>
    private FeedbackType[] EvaluateGuess(Color[] guessColors, int[] guessPercents)
    {
        // Calculate the resulting blended colors
        Color guessResultingColor = CalculateResultingColor(guessColors, guessPercents);
        
        // Compare the resulting colors
        if (ColorsEqual(guessResultingColor, targetResultingColor))
        {
            // Perfect match - all correct
            return new FeedbackType[] { FeedbackType.Correct, FeedbackType.Correct, FeedbackType.Correct };
        }
        
        // For now, return all absent if not perfect match
        // You can implement more sophisticated feedback based on how close the colors are
        return new FeedbackType[] { FeedbackType.Absent, FeedbackType.Absent, FeedbackType.Absent };
    }

    /// <summary>
    /// Generates a random target of 3 unique colors and 3 percents summing to 100.
    /// </summary>
    private void GenerateTarget()
    {
        // Pick 3 unique colors from ColorSelector
        if (colorSelector == null || colorSelector.colorPercentages.Count < 3)
            throw new InvalidOperationException("ColorSelector must have at least 3 colors.");
        var indices = Enumerable.Range(0, colorSelector.colorPercentages.Count).OrderBy(_ => UnityEngine.Random.value).Take(3).ToArray();
        for (int i = 0; i < 3; i++)
            targetColors[i] = colorSelector.colorPercentages[indices[i]].color;

        // Generate 3 random integer percents that sum to 100
        int a = UnityEngine.Random.Range(1, 100 - 1);
        int b = UnityEngine.Random.Range(1, 100 - a);
        int c = 100 - a - b;
        int[] percents = new int[] { a, b, c };
        percents = percents.OrderBy(_ => UnityEngine.Random.value).ToArray();
        for (int i = 0; i < 3; i++)
            targetPercents[i] = percents[i];

        // Calculate the resulting blended color
        targetResultingColor = CalculateResultingColor(targetColors, targetPercents);
    }

    /// <summary>
    /// Compares two colors for equality (ignores alpha).
    /// </summary>
    private bool ColorsEqual(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }

    /// <summary>
    /// Blends colors using weighted average based on their percentages.
    /// </summary>
    /// <param name="colors">Array of colors to blend.</param>
    /// <param name="percentages">Array of percentages corresponding to each color.</param>
    /// <returns>The resulting blended color.</returns>
    public Color BlendColors(Color[] colors, int[] percentages)
    {
        if (colors.Length != percentages.Length || colors.Length == 0)
            return Color.white;
        
        float totalPercentage = percentages.Sum();
        if (totalPercentage == 0) return Color.white;
        
        Color result = Color.black;
        
        for (int i = 0; i < colors.Length; i++)
        {
            float weight = percentages[i] / totalPercentage;
            result.r += colors[i].r * weight;
            result.g += colors[i].g * weight;
            result.b += colors[i].b * weight;
        }
        
        result.a = 1f; // Full opacity
        return result;
    }

    /// <summary>
    /// Calculates the resulting color from selected colors and their percentages.
    /// </summary>
    public Color CalculateResultingColor(Color[] selectedColors, int[] percentages)
    {
        return BlendColors(selectedColors, percentages);
    }

    /// <summary>
    /// Gets the target resulting color that the player needs to guess.
    /// </summary>
    public Color GetTargetResultingColor()
    {
        return targetResultingColor;
    }
}