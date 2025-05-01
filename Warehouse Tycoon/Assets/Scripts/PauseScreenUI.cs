using UnityEngine;
using UnityEngine.UIElements;

public class PauseScreenUI : MonoBehaviour
{
    public UIDocument gameUI;

    void OnEnable()
    {
        // Make sure UI is loaded
        if (gameUI == null || gameUI.rootVisualElement == null) return;

        // Get or create the pause screen container
        VisualElement root = gameUI.rootVisualElement;
        VisualElement pauseScreen = root.Q<VisualElement>("pauseScreen");

        if (pauseScreen == null)
        {
            // Create pause screen if it doesn't exist
            pauseScreen = new VisualElement();
            pauseScreen.name = "pauseScreen";
            pauseScreen.style.position = Position.Absolute;
            pauseScreen.style.width = new Length(100, LengthUnit.Percent);
            pauseScreen.style.height = new Length(100, LengthUnit.Percent);
            pauseScreen.style.backgroundColor = new Color(0, 0, 0, 0.8f);
            pauseScreen.style.justifyContent = Justify.Center;
            pauseScreen.style.alignItems = Align.Center;

            // Create pause menu panel
            VisualElement pausePanel = new VisualElement();
            pausePanel.name = "pausePanel";
            pausePanel.style.width = new Length(400, LengthUnit.Pixel);
            pausePanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            pausePanel.style.paddingTop = 20;
            pausePanel.style.paddingBottom = 20;

            // Create pause title
            Label pauseTitle = new Label("Game Paused");
            pauseTitle.style.fontSize = 24;
            pauseTitle.style.unityTextAlign = TextAnchor.MiddleCenter;
            pauseTitle.style.marginBottom = 20;
            pausePanel.Add(pauseTitle);

            // Create resume button
            Button resumeButton = new Button();
            resumeButton.name = "pauseResumeButton";
            resumeButton.text = "Resume Game";
            resumeButton.style.height = 40;
            resumeButton.style.fontSize = 16;
            resumeButton.style.marginTop = 10;
            resumeButton.style.marginBottom = 10;
            resumeButton.style.marginLeft = 50;
            resumeButton.style.marginRight = 50;
            pausePanel.Add(resumeButton);

            // Create save button
            Button saveButton = new Button();
            saveButton.name = "pauseSaveButton";
            saveButton.text = "Save Game";
            saveButton.style.height = 40;
            saveButton.style.fontSize = 16;
            saveButton.style.marginTop = 10;
            saveButton.style.marginBottom = 10;
            saveButton.style.marginLeft = 50;
            saveButton.style.marginRight = 50;
            pausePanel.Add(saveButton);

            // Create exit button
            Button exitButton = new Button();
            exitButton.name = "pauseExitButton";
            exitButton.text = "Exit Game";
            exitButton.style.height = 40;
            exitButton.style.fontSize = 16;
            exitButton.style.marginTop = 10;
            exitButton.style.marginBottom = 10;
            exitButton.style.marginLeft = 50;
            exitButton.style.marginRight = 50;
            pausePanel.Add(exitButton);

            // Add panel to pause screen
            pauseScreen.Add(pausePanel);

            // Add pause screen to root
            root.Add(pauseScreen);
        }
    }
}
