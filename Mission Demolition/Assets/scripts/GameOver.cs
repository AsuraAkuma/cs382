using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class GameOver : MonoBehaviour
{
    private UIDocument document;
    private Button closeButton;
    public GameObject MainMenuUI;
    private VisualElement star1;
    private VisualElement star2;
    private VisualElement star3;
    private void Start()
    {
        document = GetComponent<UIDocument>();
        closeButton = document.rootVisualElement.Q("MainMenuButton") as Button;
        closeButton.RegisterCallback<ClickEvent>(OnMainClick);
        // Load stars
        for (int i = 0; i < GameController.starCount; i++)
        {
            if (i == 0)
            {
                star1 = document.rootVisualElement.Q("Star1");
                star1.style.display = DisplayStyle.Flex;
            }
            else if (i == 1)
            {
                star2 = document.rootVisualElement.Q("Star2");
                star2.style.display = DisplayStyle.Flex;
            }
            else if (i == 2)
            {
                star3 = document.rootVisualElement.Q("Star3");
                star3.style.display = DisplayStyle.Flex;
            }
        }
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        closeButton = document.rootVisualElement.Q("MainMenuButton") as Button;
        closeButton.RegisterCallback<ClickEvent>(OnMainClick);
        // Load stars
        for (int i = 0; i < GameController.starCount; i++)
        {
            if (i == 0)
            {
                star1 = document.rootVisualElement.Q("Star1");
                star1.style.display = DisplayStyle.Flex;
            }
            else if (i == 1)
            {
                star2 = document.rootVisualElement.Q("Star2");
                star2.style.display = DisplayStyle.Flex;
            }
            else if (i == 2)
            {
                star3 = document.rootVisualElement.Q("Star3");
                star3.style.display = DisplayStyle.Flex;
            }
        }
        // Display score
        Label scoreLabel = document.rootVisualElement.Q<Label>("Score");
        scoreLabel.text = "Score: " + GameController.score;
    }

    private void OnDisable()
    {
        closeButton.UnregisterCallback<ClickEvent>(OnMainClick);
    }
    private void OnMainClick(ClickEvent evt)
    {
        GameController.gameState = State.NotStarted;
        GameController.score = 0;
        GameController.shotsTaken = 0;
        GameController.starCount = 0;
        GameController.currentLevel = GameController.levels[0];
        SceneManager.LoadScene("Menus");
    }
}
