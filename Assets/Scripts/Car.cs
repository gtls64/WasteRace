using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    private bool hasCrossedFinishLine = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLineCollider") && !hasCrossedFinishLine)
        {
            hasCrossedFinishLine = true;
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Scene5"); // Change "NextSceneName" to the actual name of the scene you want to load
    }
}
