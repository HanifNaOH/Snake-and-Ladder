using UnityEngine;
using UnityEditor;
using System.IO;

public class SampleConfigCreator : Editor
{
    [MenuItem("Snake and Ladder/Create Sample Configs")]
    public static void CreateSampleConfigs()
    {
        // Create the Resources directory if it doesn't exist
        string resourcesPath = "Assets/Resources";
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
            AssetDatabase.Refresh();
        }
        
        // Development config
        CreateConfig(
            "AuthenticationConfig_Dev", 
            "http://localhost:3000/api/signup", 
            "http://localhost:3000/api/signin", 
            4, 
            false
        );
        
        // Testing config
        CreateConfig(
            "AuthenticationConfig_Test", 
            "https://test-api.snakeladder.com/signup", 
            "https://test-api.snakeladder.com/signin", 
            6, 
            true
        );
        
        // Production config
        CreateConfig(
            "AuthenticationConfig_Prod", 
            "https://api.snakeladder.com/signup", 
            "https://api.snakeladder.com/signin", 
            8, 
            true
        );
        
        Debug.Log("Sample configs created successfully!");
        EditorUtility.DisplayDialog("Sample Configs Created", 
            "Three sample configs have been created:\n\n" +
            "- Development (localhost)\n" +
            "- Testing (test-api.snakeladder.com)\n" +
            "- Production (api.snakeladder.com)\n\n" +
            "You can find them in the Project window under Resources folder.", 
            "OK");
    }
    
    private static void CreateConfig(string name, string signUpUrl, string signInUrl, int minPasswordLength, bool useSecureConnection)
    {
        string configPath = $"Assets/Resources/{name}.asset";
        
        // Check if config already exists
        AuthenticationConfig config = AssetDatabase.LoadAssetAtPath<AuthenticationConfig>(configPath);
        
        if (config == null)
        {
            // Create new config
            config = ScriptableObject.CreateInstance<AuthenticationConfig>();
            config.signUpApiUrl = signUpUrl;
            config.signInApiUrl = signInUrl;
            config.minimumPasswordLength = minPasswordLength;
            config.useSecureConnection = useSecureConnection;
            config.autoFillEmailAfterSignUp = true;
            config.maxRetryAttempts = 3;
            config.requestTimeout = 10f;
            
            AssetDatabase.CreateAsset(config, configPath);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"Created config: {name}");
        }
        else
        {
            Debug.Log($"Config {name} already exists, updating values");
            
            // Update existing config
            config.signUpApiUrl = signUpUrl;
            config.signInApiUrl = signInUrl;
            config.minimumPasswordLength = minPasswordLength;
            config.useSecureConnection = useSecureConnection;
            
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }
    }
}
