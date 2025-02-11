using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject PlayerSelectUI;
    public GameObject SettingsUI;
    public GameObject GameOverUI;
    public GameObject Music;
    public Texture2D cursorTexture;
    private Settings settings = new Settings();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        if (TempInfo.gameState == TempInfo.GameState.MainMenu)
        {
            // Load main menu UI
            MainMenuUI.SetActive(true);
            PlayerSelectUI.SetActive(false);
            SettingsUI.SetActive(false);
            GameOverUI.SetActive(false);
        }
        else if (TempInfo.gameState == TempInfo.GameState.PlayerSelect)
        {
            // Load main menu UI
            MainMenuUI.SetActive(false);
            PlayerSelectUI.SetActive(true);
            SettingsUI.SetActive(false);
            GameOverUI.SetActive(false);
        }
        else if (TempInfo.gameState == TempInfo.GameState.Settings)
        {
            // Load main menu UI
            MainMenuUI.SetActive(false);
            PlayerSelectUI.SetActive(false);
            SettingsUI.SetActive(true);
            GameOverUI.SetActive(false);
        }
        else if (TempInfo.gameState == TempInfo.GameState.GameOver)
        {
            // Load main menu UI
            MainMenuUI.SetActive(false);
            PlayerSelectUI.SetActive(false);
            SettingsUI.SetActive(false);
            GameOverUI.SetActive(true);
        }
        Music.GetComponent<AudioSource>().volume = 0.2f * ((float)settings.volume / 100f);
    }
}
