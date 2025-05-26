using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class AuthenticationConfigUpdater : EditorWindow
{
    private Vector2 scrollPosition;
    private List<AuthenticationManager> authManagers = new List<AuthenticationManager>();
    
    [MenuItem("Snake and Ladder/Update Authentication Config")]
    public static void ShowWindow()
    {
        GetWindow<AuthenticationConfigUpdater>("Auth Config Updater");
    }
    
    private void OnEnable()
    {
        // Make sure we have a configuration
        AuthenticationConfig config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
        if (config == null)
        {
            Debug.LogWarning("Authentication Config not found! Creating one...");
            AuthenticationConfigSetup.SetupAuthenticationConfig();
        }
        
        // Find all AuthenticationManager instances in the current scene
        FindAuthenticationManagers();
    }
    
    private void FindAuthenticationManagers()
    {
        authManagers.Clear();
        
        // Find all Authentication Managers in the scene
        AuthenticationManager[] managers = Object.FindObjectsByType<AuthenticationManager>(FindObjectsSortMode.None);
        authManagers.AddRange(managers);
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Authentication Config Updater", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Get the current config
        AuthenticationConfig config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
        if (config == null)
        {
            EditorGUILayout.HelpBox("Authentication Config not found in Resources folder!", MessageType.Error);
            if (GUILayout.Button("Create Configuration"))
            {
                AuthenticationConfigSetup.SetupAuthenticationConfig();
                config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
            }
            
            if (config == null) return;
        }
        
        // Display current config values
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.LabelField("Current Configuration", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Sign Up API URL", config.signUpApiUrl);
        EditorGUILayout.TextField("Sign In API URL", config.signInApiUrl);
        EditorGUILayout.Toggle("Use Secure Connection", config.useSecureConnection);
        EditorGUILayout.IntField("Max Retry Attempts", config.maxRetryAttempts);
        EditorGUILayout.IntField("Min Password Length", config.minimumPasswordLength);
        EditorGUILayout.Toggle("Auto-Fill Email After SignUp", config.autoFillEmailAfterSignUp);
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        
        // Edit Configuration button
        if (GUILayout.Button("Edit Configuration"))
        {
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }
        
        EditorGUILayout.Space();
        
        // AuthenticationManager instances in the scene
        if (GUILayout.Button("Refresh AuthenticationManagers"))
        {
            FindAuthenticationManagers();
        }
        
        EditorGUILayout.LabelField("Authentication Managers in Scene", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        if (authManagers.Count == 0)
        {
            EditorGUILayout.HelpBox("No AuthenticationManager found in the current scene.", MessageType.Info);
        }
        else
        {
            foreach (AuthenticationManager manager in authManagers)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField("Auth Manager", manager, typeof(AuthenticationManager), true);
                
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    Selection.activeObject = manager.gameObject;
                    EditorGUIUtility.PingObject(manager.gameObject);
                }
                
                EditorGUILayout.EndHorizontal();
                
                if (manager.config == null)
                {
                    EditorGUILayout.HelpBox("This manager has no config assigned!", MessageType.Warning);
                    
                    if (GUILayout.Button("Assign Config"))
                    {
                        manager.config = config;
                        EditorUtility.SetDirty(manager);
                    }
                }
                else if (manager.config != config)
                {
                    EditorGUILayout.HelpBox("This manager is using a different config!", MessageType.Warning);
                    
                    if (GUILayout.Button("Update Config Reference"))
                    {
                        manager.config = config;
                        EditorUtility.SetDirty(manager);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Config is correctly assigned", EditorStyles.miniLabel);
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            if (authManagers.Count > 0)
            {
                if (GUILayout.Button("Update All Authentication Managers"))
                {
                    foreach (AuthenticationManager manager in authManagers)
                    {
                        manager.config = config;
                        EditorUtility.SetDirty(manager);
                    }
                    Debug.Log("All Authentication Managers updated with the current config");
                }
            }
        }
        
        EditorGUILayout.EndScrollView();
    }
}
