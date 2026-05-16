using UnityEngine;

public class MapButtonHandler : MonoBehaviour
{
    public GameManager gameManager;
    public LockMessageUI lockMessage;

    public void PressMap2()
    {
        gameManager.ShowMap2Levels();
    }

    public void PressMap3()
    {
        Debug.Log("MAP3 CLICKED");

        int m2l3 = PlayerPrefs.GetInt("M2L3", 0);

        if (m2l3 == 0)
        {
            lockMessage.ShowMapLocked();
        }
        else
        {
            gameManager.ShowMap3Levels();
        }
    }
}