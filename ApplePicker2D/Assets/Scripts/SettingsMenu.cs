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
    private Settings settings = new Settings(new Settings.playerData());
    private static string settingsPath = "settings.json";
    private void Start()
    {
        // Create or load settings
        if (File.Exists(settingsPath))
        {
            settings.loadSettings();
        }
        else
        {
            settings.saveSettings();
        }
        document = GetComponent<UIDocument>();
        closeButton = document.rootVisualElement.Q("closeSettingsButton") as Button;
        volumeSlider = document.rootVisualElement.Q("volumeSlider") as SliderInt;
        volumeSlider.value = settings.volume;
        closeButton.RegisterCallback<ClickEvent>(OnCloseClick);
        volumeSlider.RegisterValueChangedCallback(evt => OnSliderMove(evt.newValue));
    }
    private void OnEnable()
    {
        // Create or load settings
        if (File.Exists(settingsPath))
        {
            settings.loadSettings();
        }
        else
        {
            settings.saveSettings();
        }
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
        settings.saveSettings();
    }
    private void OnCloseClick(ClickEvent evt)
    {
        // print("Settings button pressed");
        gameObject.SetActive(false);
        MainMenuUI.SetActive(true);
    }
    private void OnExitClick(ClickEvent evt)
    {
        // print("Exit button pressed");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
