using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class LoginSceneSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Snake and Ladder/Setup Login Scene")]
    public static void SetupLoginScene()
    {
        // Check if Login scene exists
        string loginScenePath = "Assets/Scenes/Login.unity";
        
        // Check if AuthenticationConfig exists, create it if not
        AuthenticationConfig config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
        if (config == null)
        {
            // Create the config
            Debug.Log("Authentication Config not found. Creating a new one...");
            AuthenticationConfigSetup.SetupAuthenticationConfig();
        }
        
        // Open the Login scene
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(loginScenePath, OpenSceneMode.Single);
        
        // Check if AuthenticationManager already exists in the scene
        AuthenticationManager existingAuthManager = Object.FindFirstObjectByType<AuthenticationManager>();
        if (existingAuthManager != null)
        {
            Debug.Log("Authentication system already exists in the Login scene.");
            Selection.activeGameObject = existingAuthManager.gameObject;
            return;
        }
        
        // Create the Authentication system using our existing tool
        AuthenticationFormSetup.CreateAuthenticationSystem();
        
        // Save the scene
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        
        Debug.Log("Login scene setup completed successfully!");
    }
      [MenuItem("Snake and Ladder/Connect Login to Gameplay")]
    public static void ConnectLoginToGameplay()
    {
        // Make sure both scenes are included in build settings
        AddSceneToBuildSettings("Assets/Scenes/Login.unity");
        AddSceneToBuildSettings("Assets/Scenes/Gameplay.unity");
          // Find the AuthenticationManager in the current scene
        AuthenticationManager authManager = Object.FindFirstObjectByType<AuthenticationManager>();
        if (authManager == null)
        {
            Debug.LogError("AuthenticationManager not found in the scene! Please run 'Setup Login Scene' first.");
            return;
        }
        
        // Add SceneLoader component if not already added
        SceneLoader sceneLoader = authManager.gameObject.GetComponent<SceneLoader>();
        if (sceneLoader == null)
        {
            sceneLoader = authManager.gameObject.AddComponent<SceneLoader>();
        }
        
        // Configure the SceneLoader
        sceneLoader.gameplaySceneName = "Gameplay";
        
        // Find the play button in the main menu
        Transform mainMenuPanel = authManager.transform.Find("AuthenticationUI/MainMenuPanel");
        if (mainMenuPanel != null)
        {
            Transform playButton = mainMenuPanel.Find("PlayButton");
            if (playButton != null)
            {
                UnityEngine.UI.Button button = playButton.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    // Remove any existing listeners
                    button.onClick.RemoveAllListeners();
                    
                    // Add listener to load gameplay scene
                    button.onClick.AddListener(sceneLoader.LoadGameplayScene);
                    
                    Debug.Log("Play button connected to load the Gameplay scene");
                }
            }
        }
        
        // Save the scene
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        
        Debug.Log("Login scene connected to Gameplay scene successfully!");
    }
    
    private static void AddSceneToBuildSettings(string scenePath)
    {
        // Get current scenes in build settings
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        
        // Check if scene is already in build settings
        bool sceneExists = false;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
            {
                sceneExists = true;
                break;
            }
        }
        
        // If scene doesn't exist in build settings, add it
        if (!sceneExists)
        {
            EditorBuildSettingsScene[] newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            System.Array.Copy(scenes, newScenes, scenes.Length);
            
            newScenes[scenes.Length] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = newScenes;
            
            Debug.Log($"Added {scenePath} to build settings");
        }
    }
#endif
}
