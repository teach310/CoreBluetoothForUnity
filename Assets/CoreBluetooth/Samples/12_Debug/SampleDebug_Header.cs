using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleDebug_Header : MonoBehaviour
{
    public void OnClickHome()
    {
        SceneManager.LoadScene("SampleDebug_HomeScene");
    }
}
