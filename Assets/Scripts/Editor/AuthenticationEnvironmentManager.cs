using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AuthenticationEnvironmentManager : EditorWindow
{
    private List<AuthenticationConfig> configs = new List<AuthenticationConfig>();
    private Vector2 scrollPosition;
    private const string CONFIG_PATH = "Assets/Resources";
    private const string CONFIG_NAME = "AuthenticationConfig";
    
    [MenuItem("Snake and Ladder/Authentication Environment Manager")]
    public static void ShowWindow()
    {
        GetWindow<AuthenticationEnvironmentManager>("Auth Environment");
    }
    
    private void OnEnable()
    {
        // Find all authentication configs in the project
        FindAllConfigurations();
    }
    
    private void FindAllConfigurations()
    {
        configs.Clear();
        string[] guids = AssetDatabase.FindAssets("t:AuthenticationConfig");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AuthenticationConfig config = AssetDatabase.LoadAssetAtPath<AuthenticationConfig>(path);
            if (config != null)
            {
                configs.Add(config);
            }
        }
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Authentication Environment Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Refresh Configurations"))
        {
            FindAllConfigurations();
        }
        
        EditorGUILayout.Space();
        
        if (configs.Count == 0)
        {
            EditorGUILayout.HelpBox("No AuthenticationConfig assets found. Create one using Snake and Ladder > Setup Authentication Config", MessageType.Info);
            
            if (GUILayout.Button("Create Configuration"))
            {
                AuthenticationConfigSetup.SetupAuthenticationConfig();
                FindAllConfigurations();
            }
            
            return;
        }
        
        EditorGUILayout.LabelField("Available Configurations", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        foreach (AuthenticationConfig config in configs)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(config.name, EditorStyles.boldLabel);
            
            GUI.enabled = !IsActiveConfig(config);
            if (GUILayout.Button("Set Active", GUILayout.Width(100)))
            {
                SetActiveConfig(config);
            }
            GUI.enabled = true;
            
            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                Selection.activeObject = config;
                EditorGUIUtility.PingObject(config);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Sign Up URL:", config.signUpApiUrl);
            EditorGUILayout.LabelField("Sign In URL:", config.signInApiUrl);
            EditorGUILayout.LabelField("Password Length:", config.minimumPasswordLength.ToString());
            EditorGUILayout.LabelField("Secure Connection:", config.useSecureConnection.ToString());
            EditorGUI.indentLevel--;
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Create New Configuration"))
        {
            string uniqueName = AssetDatabase.GenerateUniqueAssetPath($"{CONFIG_PATH}/AuthenticationConfig_New.asset");
            AuthenticationConfig newConfig = ScriptableObject.CreateInstance<AuthenticationConfig>();
            AssetDatabase.CreateAsset(newConfig, uniqueName);
            AssetDatabase.SaveAssets();
            FindAllConfigurations();
            Selection.activeObject = newConfig;
            EditorGUIUtility.PingObject(newConfig);
        }
    }
    
    private bool IsActiveConfig(AuthenticationConfig config)
    {
        AuthenticationConfig activeConfig = Resources.Load<AuthenticationConfig>(CONFIG_NAME);
        return activeConfig == config;
    }
    
    private void SetActiveConfig(AuthenticationConfig config)
    {
        string sourcePath = AssetDatabase.GetAssetPath(config);
        string targetPath = $"{CONFIG_PATH}/{CONFIG_NAME}.asset";
        
        // Check if source path is already in Resources folder and named correctly
        if (sourcePath == targetPath)
        {
            Debug.Log($"{config.name} is already the active configuration");
            return;
        }
        
        // Delete existing config in Resources if it exists
        if (File.Exists(targetPath))
        {
            AssetDatabase.DeleteAsset(targetPath);
        }
        
        // Make sure the Resources directory exists
        if (!Directory.Exists(CONFIG_PATH))
        {
            Directory.CreateDirectory(CONFIG_PATH);
        }
        
        // Create a duplicate of the config in the Resources folder
        AssetDatabase.CopyAsset(sourcePath, targetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"Set {config.name} as the active configuration");
        EditorUtility.DisplayDialog("Configuration Updated", 
            $"{config.name} is now the active configuration.\n\n" +
            "The changes will take effect when you restart the game or enter Play mode.", 
            "OK");
    }
}
