using UnityEngine;
using UnityEditor;
using System.IO;

public class AuthenticationConfigSetup : Editor
{
    private const string CONFIG_PATH = "Assets/Resources";
    private const string CONFIG_NAME = "AuthenticationConfig";
    
    [MenuItem("Snake and Ladder/Setup Authentication Config")]
    public static void SetupAuthenticationConfig()
    {
        // Create Resources folder if it doesn't exist
        if (!Directory.Exists(CONFIG_PATH))
        {
            Directory.CreateDirectory(CONFIG_PATH);
            AssetDatabase.Refresh();
        }
        
        // Check if config already exists
        string assetPath = $"{CONFIG_PATH}/{CONFIG_NAME}.asset";
        AuthenticationConfig config = AssetDatabase.LoadAssetAtPath<AuthenticationConfig>(assetPath);
        
        if (config == null)
        {
            // Create new config
            config = ScriptableObject.CreateInstance<AuthenticationConfig>();
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"Authentication Config created at: {assetPath}");
        }
        
        // Ping the config in the Project window
        EditorGUIUtility.PingObject(config);
        Selection.activeObject = config;
        
        // Show a message to the user
        EditorUtility.DisplayDialog("Authentication Config", 
            "Authentication Config has been created in the Resources folder.\n\n" +
            "You can now configure your API endpoints and other settings in the Inspector.", 
            "OK");
    }
}
