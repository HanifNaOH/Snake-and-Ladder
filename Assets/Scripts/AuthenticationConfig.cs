using UnityEngine;

[CreateAssetMenu(fileName = "AuthenticationConfig", menuName = "Snake and Ladder/Authentication Config", order = 1)]
public class AuthenticationConfig : ScriptableObject
{
    [Header("API Endpoints")]
    [Tooltip("The URL for the sign-up API endpoint")]
    public string signUpApiUrl = "https://your-api-endpoint.com/signup";
    
    [Tooltip("The URL for the sign-in API endpoint")]
    public string signInApiUrl = "https://your-api-endpoint.com/signin";
    
    [Header("Network Settings")]
    [Tooltip("Use secure connection (HTTPS)")]
    public bool useSecureConnection = true;
    
    [Tooltip("Maximum number of retry attempts for API calls")]
    public int maxRetryAttempts = 3;
    
    [Tooltip("Timeout duration for API requests in seconds")]
    public float requestTimeout = 10f;
    
    [Header("Authentication Settings")]
    [Tooltip("Minimum password length for sign-up")]
    public int minimumPasswordLength = 6;
    
    [Tooltip("Automatically fill the sign-in form with the email after successful sign-up")]
    public bool autoFillEmailAfterSignUp = true;
    
    [Tooltip("Show/hide password toggle button")]
    public bool enablePasswordToggle = true;
}
