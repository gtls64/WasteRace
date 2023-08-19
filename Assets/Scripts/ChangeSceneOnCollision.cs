using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollision : MonoBehaviour
{
    // Allow the scene to be set through the Unity inspector
    [SerializeField] private string sceneToLoad = "Scene5";

    // This method is called when a collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane")) // Check if the colliding object has a "Player" tag
        {
            Debug.Log("Collision detected with player. Loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
    }
}
