using UnityEngine;
using UnityEngine.UIElements;
public class PauseMenu : MonoBehaviour
{
    private UIDocument document;
    private Button settingsButton;
    private Button exitButton;
    public GameObject SettingsUI;
    private void Start()
    {
        document = GetComponent<UIDocument>();
        settingsButton = document.rootVisualElement.Q("settingsButton") as Button;
        exitButton = document.rootVisualElement.Q("exitButton") as Button;
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        settingsButton = document.rootVisualElement.Q("settingsButton") as Button;
        exitButton = document.rootVisualElement.Q("exitButton") as Button;
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }

    private void OnDisable()
    {
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.UnregisterCallback<ClickEvent>(OnExitClick);
    }
    private void OnSettingsClick(ClickEvent evt)
    {
        // print("Settings button pressed");
        gameObject.SetActive(false);
        SettingsUI.SetActive(true);
    }
    private void OnExitClick(ClickEvent evt)
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
