using UnityEngine;
using System;
using System.Collections;

// User data structure
[System.Serializable]
public class UserData
{
    public string Email;
    public string Password;
    public DateTime CreateDate;
}

public class UserDataHandler : MonoBehaviour
{
    [Header("API Settings")]
    public string apiUrl = "https://your-api-endpoint.com/signup";
    public float requestTimeout = 10f;
    
    // Reference to the network manager
    private UserNetworkManager networkManager;
    
    private void Awake()
    {
        // Initialize the network manager
        networkManager = GetComponent<UserNetworkManager>();
        if (networkManager == null)
        {
            networkManager = gameObject.AddComponent<UserNetworkManager>();
        }
    }
      // Delegate type for authentication response
    public delegate void AuthenticationCallback(bool success, string response);
    
    // Default callback
    private AuthenticationCallback defaultCallback;
    
    public void ProcessUserData(UserData userData)
    {
        ProcessUserData(userData, null);
    }
    
    public void ProcessUserData(UserData userData, AuthenticationCallback callback)
    {
        // Store callback if provided
        defaultCallback = callback;
        
        // Generate JSON data
        string jsonData = GenerateJsonData(userData);
        Debug.Log("Generated JSON data: " + jsonData);
        
        // Send data to server via network manager
        StartCoroutine(networkManager.SendSignUpData(apiUrl, jsonData, OnSignUpResponse));
    }
    
    private string GenerateJsonData(UserData userData)
    {
        // Create JSON object for sign-up
        SignUpJson signUpJson = new SignUpJson
        {
            email = userData.Email,
            password = userData.Password,
            createDate = userData.CreateDate.ToString("yyyy-MM-ddTHH:mm:ss")
        };
        
        // Convert to JSON string
        return JsonUtility.ToJson(signUpJson);
    }
    
    private void OnSignUpResponse(bool success, string response)
    {
        if (success)
        {
            Debug.Log("Sign-up successful: " + response);
            // Handle successful sign-up (e.g., redirect to login or game)
        }
        else
        {
            Debug.LogError("Sign-up failed: " + response);
            // Handle failed sign-up (e.g., show error message)
        }
        
        // Call the callback if provided
        if (defaultCallback != null)
        {
            defaultCallback(success, response);
        }
    }
}

// JSON structure to match the required format
[System.Serializable]
public class SignUpJson
{
    public string email;
    public string password;
    public string createDate;
}
