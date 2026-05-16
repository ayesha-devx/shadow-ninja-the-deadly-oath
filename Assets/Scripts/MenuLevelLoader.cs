using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelLoader : MonoBehaviour
{
    // MAP 1
    public void LoadM1L1() { SceneManager.LoadScene("Level1"); }
    public void LoadM1L2() { SceneManager.LoadScene("Level2"); }
    public void LoadM1L3() { SceneManager.LoadScene("Level3"); }

    // MAP 2
    public void LoadM2L1() { SceneManager.LoadScene("M2L1"); }
    public void LoadM2L2() { SceneManager.LoadScene("M2L2"); }
    public void LoadM2L3() { SceneManager.LoadScene("M2L3"); }

    // MAP 3
    public void LoadM3L1() { SceneManager.LoadScene("M3L1"); }
    public void LoadM3L2() { SceneManager.LoadScene("M3L2"); }
    public void LoadM3L3() { SceneManager.LoadScene("M3L3"); }
}