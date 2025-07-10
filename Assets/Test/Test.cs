using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Color targetColor;

    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;
    //[SerializeField] private Color colorC;

    [SerializeField] private float weightA = 0.7f;
    [SerializeField] private float weightB = 0.3f;
    //[SerializeField] private float weightC = 0.3f;

    private void Start()
    {
        Color result = new Color(
            (colorA.r * weightA + colorB.r * weightB), // + colorC.r * weightC),
            (colorA.g * weightA + colorB.g * weightB), // + colorC.g * weightC),
            (colorA.b * weightA + colorB.b * weightB), //+ colorC.b * weightC)
            (colorA.a * weightA + colorB.a * weightB) //+ colorC.a * weightC)
        );
        
        Debug.Log(result);
        Debug.Log(result==targetColor);
    }
}