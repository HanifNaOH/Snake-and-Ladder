using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Names")]
    public string loginSceneName = "Login";
    public string gameplaySceneName = "Gameplay";
    
    // Reference to authentication manager
    private AuthenticationManager authManager;
    
    void Awake()
    {
        authManager = GetComponent<AuthenticationManager>();
        if (authManager == null)
        {
            Debug.LogWarning("AuthenticationManager not found on the same GameObject as SceneLoader.");
        }
    }
    
    public void LoadGameplayScene()
    {
        // Check if user is authenticated
        if (authManager != null && !authManager.IsUserAuthenticated())
        {
            Debug.LogWarning("User not authenticated. Please sign in first.");
            return;
        }
        
        // Load the gameplay scene
        Debug.Log("Loading gameplay scene: " + gameplaySceneName);
        SceneManager.LoadScene(gameplaySceneName);
    }
    
    public void LoadLoginScene()
    {
        Debug.Log("Loading login scene: " + loginSceneName);
        SceneManager.LoadScene(loginSceneName);
    }
    
    // Call this when user signs out
    public void SignOut()
    {
        if (authManager != null)
        {
            // Reset authentication state (you'll need to add this method to AuthenticationManager)
            // authManager.SignOut();
        }
        
        // Return to login scene
        LoadLoginScene();
    }
}
