using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable] // Allows serialization
public class SettingsMenu : MonoBehaviour
{
    private UIDocument document;
    private Button closeButton;
    private SliderInt volumeSlider;
    public GameObject MainMenuUI;
    public GameObject PauseMenuUI;
    public GameObject Music;
    private Settings settings = new Settings();
    private void Start()
    {
        document = GetComponent<UIDocument>();
        closeButton = document.rootVisualElement.Q("closeSettingsButton") as Button;
        volumeSlider = document.rootVisualElement.Q("volumeSlider") as SliderInt;
        volumeSlider.value = settings.volume;
        closeButton.RegisterCallback<ClickEvent>(OnCloseClick);
        volumeSlider.RegisterValueChangedCallback(evt => OnSliderMove(evt.newValue));
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        closeButton = document.rootVisualElement.Q("closeSettingsButton") as Button;
        volumeSlider = document.rootVisualElement.Q("volumeSlider") as SliderInt;
        volumeSlider.value = settings.volume;
        closeButton.RegisterCallback<ClickEvent>(OnCloseClick);
        volumeSlider.RegisterValueChangedCallback(evt => OnSliderMove(evt.newValue));
    }

    private void OnDisable()
    {
        closeButton.UnregisterCallback<ClickEvent>(OnCloseClick);
        volumeSlider.UnregisterValueChangedCallback(evt => OnSliderMove(evt.newValue));
    }
    private void OnSliderMove(float evt)
    {
        // print("Slider: " + evt);
        settings.volume = (int)evt;
        Music.GetComponent<AudioSource>().volume = 0.2f * ((float)settings.volume / 100f);
        settings.saveSettings();
    }
    private void OnCloseClick(ClickEvent evt)
    {
        gameObject.SetActive(false);
        if (TempInfo.gameState == TempInfo.GameState.InGame)
        {
            PauseMenuUI.SetActive(true);
        }
        else if (TempInfo.gameState == TempInfo.GameState.MainMenu)
        {
            MainMenuUI.SetActive(true);
        }
    }
}
