using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Check if the tutorial has been completed
        if (Globals.tutorialStatus == StatusType.Type.Completed)
        {
            // Load the game state
            LoadGameState();
        }
        else
        {
            // Start the tutorial
            ProgressTutorial();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to start the tutorial
    private void ProgressTutorial()
    {
        // Get the current tutorial step
        int currentStep = Globals.tutorialStep;
        switch (currentStep)
        {
            case 0:
                // Start the tutorial step 1
                TutorialStep1();
                break;

            default:
                Debug.Log("No more tutorial steps available.");
                break;
        }
    }
    // Tutorial step logic
    private void TutorialStep1()
    {
        // Add logic for tutorial step 1 here
        Debug.Log("Tutorial Step 1: Welcome to the Warehouse Tycoon!");

        // Move to the next step
        Globals.tutorialStep++;
    }

    // Method to load the game state
    private void LoadGameState()
    {
        // Add logic to load the game state here
        Debug.Log("Game state loaded.");
    }

}
