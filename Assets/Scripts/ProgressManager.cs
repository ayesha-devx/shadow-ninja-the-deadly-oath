using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Button map2Button;
    public Button map3Button;

    public GameObject map2Lock;
    public GameObject map3Lock;

    public Button M1L1;
    public Button M1L2;
    public Button M1L3;

    public Button M2L1;
    public Button M2L2;
    public Button M2L3;

    public Button M3L1;
    public Button M3L2;
    public Button M3L3;

    void Start()
    {
        UpdateLocks();
    }

    public void UpdateLocks()
    {
        int m1l1 = PlayerPrefs.GetInt("M1L1", 0);
        int m1l2 = PlayerPrefs.GetInt("M1L2", 0);
        int m1l3 = PlayerPrefs.GetInt("M1L3", 0);

        int m2l1 = PlayerPrefs.GetInt("M2L1", 0);
        int m2l2 = PlayerPrefs.GetInt("M2L2", 0);
        int m2l3 = PlayerPrefs.GetInt("M2L3", 0);

        int m3l1 = PlayerPrefs.GetInt("M3L1", 0);
        int m3l2 = PlayerPrefs.GetInt("M3L2", 0);

        // MAP 1 LEVELS
        M1L1.interactable = true;
        M1L2.interactable = m1l1 == 1;
        M1L3.interactable = m1l2 == 1;

        // MAP 2 UNLOCK
        map2Button.interactable = m1l3 == 1;
        map2Lock.SetActive(m1l3 == 0);

        // MAP 2 LEVELS
        M2L1.interactable = m1l3 == 1;
        M2L2.interactable = m2l1 == 1;
        M2L3.interactable = m2l2 == 1;

        // MAP 3 UNLOCK
        map3Button.interactable = m2l3 == 1;
        map3Lock.SetActive(m2l3 == 0);

        // MAP 3 LEVELS
        M3L1.interactable = m2l3 == 1;
        M3L2.interactable = m3l1 == 1;
        M3L3.interactable = m3l2 == 1;
    }

    public void CompleteM1L1() 
    {
        PlayerPrefs.SetInt("M1L1", 1); UpdateLocks(); 
    }
    public void CompleteM1L2() 
    { 
        PlayerPrefs.SetInt("M1L2", 1); UpdateLocks(); 
    }
    public void CompleteM1L3() 
    { 
        PlayerPrefs.SetInt("M1L3", 1); UpdateLocks(); 
    }

    public void CompleteM2L1() 
    { 
        PlayerPrefs.SetInt("M2L1", 1); UpdateLocks(); 
    }
    public void CompleteM2L2() 
    { 
        PlayerPrefs.SetInt("M2L2", 1); UpdateLocks(); 
    }
    public void CompleteM2L3() 
    { 
        PlayerPrefs.SetInt("M2L3", 1); UpdateLocks(); 
    }

    public void CompleteM3L1() 
    { 
        PlayerPrefs.SetInt("M3L1", 1); UpdateLocks(); 
    }
    public void CompleteM3L2() 
    { 
        PlayerPrefs.SetInt("M3L2", 1); UpdateLocks(); 
    }
    public void CompleteM3L3() 
    { 
        PlayerPrefs.SetInt("M3L3", 1); UpdateLocks(); 
    }
}