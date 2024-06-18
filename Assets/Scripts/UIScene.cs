using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScene : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private CanvasGroup buttonPanelCanvasGroup;
    [SerializeField] private CanvasGroup sliderCanvasGroup;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Slider loadingSlider;

    private bool _sceneLoading = false;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        exitButton.onClick.AddListener(OnExitClicked);

        var bestScore = PlayerPrefs.GetInt("BestScore", 0);

        scoreText.text = $"BEST SCORE: {bestScore}";
    }

    private void OnPlayClicked()
    {
        if (_sceneLoading)
            return;

        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        _sceneLoading = true;

        yield return buttonPanelCanvasGroup.DOFade(0f, .5f);

        buttonPanelCanvasGroup.gameObject.SetActive(false);

        sliderCanvasGroup.gameObject.SetActive(true);

        yield return sliderCanvasGroup.DOFade(1f, .5f);

        var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;

                yield return new WaitForSeconds(.5f);

                asyncOperation.allowSceneActivation = true;

            }
            else
            {
                yield return null;
            }
        }

    }

    private void OnExitClicked()
    {
        if (_sceneLoading)
            return;

        Application.Quit();
    }
}
