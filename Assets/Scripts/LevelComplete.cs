using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public string levelID;

    public void CompleteLevel()
    {
        PlayerPrefs.SetInt(levelID, 1);
        PlayerPrefs.Save();
    }
}