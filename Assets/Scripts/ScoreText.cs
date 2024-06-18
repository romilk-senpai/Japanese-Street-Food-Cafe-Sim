using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AIManager aiManager;

    void Start()
    {
        aiManager.OnScoreUpdated += UpdateText;
    }

    private void UpdateText(int score)
    {
        text.text = $"Score: {score}";
    }
}
