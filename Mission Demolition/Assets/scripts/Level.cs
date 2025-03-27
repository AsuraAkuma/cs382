using UnityEngine;

class Level
{
    public string name;
    public int maxShots;
    public int maxPoints;
    private GameObject prefab;
    public Level() { }
    public Level(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
    }

    private void SetLevelValues()
    {
        GameObject levelObject = GameObject.Find(name + "(Clone)");
        Debug.Log("Search: " + levelObject.transform.childCount);
        maxShots = 0;
        maxPoints = 0;
        // Find the maximum number of shots for the level
        for (int i = 0; i < levelObject.transform.childCount; i++)
        {
            Transform child = levelObject.transform.GetChild(i);
            if (child.name.Contains("Pig"))
            {
                maxShots++;
                maxPoints += 100;
            }
        }
        GameController.maxShots = maxShots;
    }

    public void LoadLevel()
    {
        GameObject levelObject = GameObject.Find(name + "(Clone)");
        // If level exists delete it
        if (levelObject != null)
        {
            GameObject.Destroy(levelObject);
        }
        // Instantiate the level
        levelObject = GameObject.Instantiate(prefab);
        levelObject.SetActive(true);
        GameController.maxShots = maxShots;
        SetLevelValues();
    }

    public void UnloadLevel()
    {
        GameObject levelObject = GameObject.Find(name + "(Clone)");
        // If level exists delete it
        if (levelObject != null)
        {
            GameObject.Destroy(levelObject);
        }
    }
}