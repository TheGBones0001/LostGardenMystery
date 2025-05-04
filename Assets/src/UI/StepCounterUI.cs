using TMPro;
using UnityEngine;

public class StepCounterUI : MonoBehaviour
{
    public static StepCounterUI Instance { get; private set; }

    public TextMeshProUGUI counterText;

    private int steps = 0;
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        UpdateText();
    }

    public void IncrementSteps()
    {
        steps++;
        UpdateText();
    }
    private void UpdateText()
    {
        counterText.text = $"Passos \n {steps}";
    }
}
