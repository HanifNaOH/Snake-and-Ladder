using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject signUpPanel;
    public GameObject signInPanel;
    public GameObject mainMenuPanel;
    public GameObject loadingIndicator;
    
    [Header("Sign Up Form")]
    public TMP_InputField signUpEmailField;
    public TMP_InputField signUpPasswordField;
    public TMP_InputField signUpConfirmPasswordField;
    public Button signUpSubmitButton;
    public TextMeshProUGUI signUpErrorText;
    public Button switchToSignInButton;
    
    [Header("Sign In Form")]
    public TMP_InputField signInEmailField;
    public TMP_InputField signInPasswordField;
    public Button signInSubmitButton;
    public TextMeshProUGUI signInErrorText;
    public Button switchToSignUpButton;
    
    [Header("Configuration")]
    [Tooltip("Reference to the Authentication Config asset. If left empty, the system will look for it in Resources folder.")]
    public AuthenticationConfig config;
    
    private UserNetworkManager networkManager;
    private UserDataHandler userDataHandler;
    
    // Store user session data
    private string currentUserEmail;
    private bool isAuthenticated = false;
      void Awake()
    {
        // Load config if not set
        if (config == null)
        {
            config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
            if (config == null)
            {
                Debug.LogWarning("Authentication Config not found! Please create one via 'Snake and Ladder > Setup Authentication Config'");
                // Create a default config for now
                config = ScriptableObject.CreateInstance<AuthenticationConfig>();
            }
        }
        
        // Get or add required components
        networkManager = GetComponent<UserNetworkManager>();
        if (networkManager == null)
            networkManager = gameObject.AddComponent<UserNetworkManager>();
            
        userDataHandler = GetComponent<UserDataHandler>();
        if (userDataHandler == null)
            userDataHandler = gameObject.AddComponent<UserDataHandler>();
        
        // Set the API URLs and other settings from config
        userDataHandler.apiUrl = config.signUpApiUrl;
        userDataHandler.requestTimeout = config.requestTimeout;
        networkManager.useSecureConnection = config.useSecureConnection;
        networkManager.maxRetryAttempts = config.maxRetryAttempts;
        
        // Initialize UI state
        if (loadingIndicator) loadingIndicator.SetActive(false);
        ShowSignInPanel();
    }
    
    void Start()
    {
        // Set up button listeners
        if (signUpSubmitButton)
            signUpSubmitButton.onClick.AddListener(HandleSignUp);
            
        if (signInSubmitButton)
            signInSubmitButton.onClick.AddListener(HandleSignIn);
            
        if (switchToSignInButton)
            switchToSignInButton.onClick.AddListener(ShowSignInPanel);
            
        if (switchToSignUpButton)
            switchToSignUpButton.onClick.AddListener(ShowSignUpPanel);
    }
    
    void OnDestroy()
    {
        // Clean up listeners
        if (signUpSubmitButton)
            signUpSubmitButton.onClick.RemoveListener(HandleSignUp);
            
        if (signInSubmitButton)
            signInSubmitButton.onClick.RemoveListener(HandleSignIn);
            
        if (switchToSignInButton)
            switchToSignInButton.onClick.RemoveListener(ShowSignInPanel);
            
        if (switchToSignUpButton)
            switchToSignUpButton.onClick.RemoveListener(ShowSignUpPanel);
    }
    
    // UI State Management
    public void ShowSignUpPanel()
    {
        if (signUpPanel) signUpPanel.SetActive(true);
        if (signInPanel) signInPanel.SetActive(false);
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        
        // Clear any previous error messages
        if (signUpErrorText) signUpErrorText.gameObject.SetActive(false);
    }
    
    public void ShowSignInPanel()
    {
        if (signUpPanel) signUpPanel.SetActive(false);
        if (signInPanel) signInPanel.SetActive(true);
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        
        // Clear any previous error messages
        if (signInErrorText) signInErrorText.gameObject.SetActive(false);
    }
    
    public void ShowMainMenu()
    {
        if (signUpPanel) signUpPanel.SetActive(false);
        if (signInPanel) signInPanel.SetActive(false);
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
    }
    
    // Sign Up Logic
    private void HandleSignUp()
    {
        if (signUpErrorText) signUpErrorText.gameObject.SetActive(false);
        
        // Validate form
        string errorMessage = ValidateSignUpForm();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ShowSignUpError(errorMessage);
            return;
        }
        
        // Show loading
        if (loadingIndicator) loadingIndicator.SetActive(true);
        
        // Create user data
        UserData userData = new UserData
        {
            Email = signUpEmailField.text,
            Password = signUpPasswordField.text,
            CreateDate = DateTime.Now
        };
        
        // Store email for sign-in
        currentUserEmail = userData.Email;
        
        // Process sign-up
        userDataHandler.ProcessUserData(userData, OnSignUpComplete);
    }
    
    private string ValidateSignUpForm()
    {
        // Check for empty fields
        if (string.IsNullOrWhiteSpace(signUpEmailField.text))
            return "Email is required.";
            
        if (string.IsNullOrWhiteSpace(signUpPasswordField.text))
            return "Password is required.";
            
        if (string.IsNullOrWhiteSpace(signUpConfirmPasswordField.text))
            return "Please confirm your password.";
            
        // Check if passwords match
        if (signUpPasswordField.text != signUpConfirmPasswordField.text)
            return "Passwords do not match.";
              // Email validation
        if (!IsValidEmail(signUpEmailField.text))
            return "Please enter a valid email address.";
            
        // Password strength (using config)
        if (signUpPasswordField.text.Length < config.minimumPasswordLength)
            return $"Password must be at least {config.minimumPasswordLength} characters long.";
            
        return string.Empty;
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    private void ShowSignUpError(string message)
    {
        if (signUpErrorText)
        {
            signUpErrorText.text = message;
            signUpErrorText.gameObject.SetActive(true);
        }
    }
    
    private void OnSignUpComplete(bool success, string response)
    {
        // Hide loading
        if (loadingIndicator) loadingIndicator.SetActive(false);
          if (success)
        {
            Debug.Log("Sign-up successful");
            
            // Auto-fill sign-in form with the email that was just registered (if enabled in config)
            if (config.autoFillEmailAfterSignUp && signInEmailField) 
                signInEmailField.text = currentUserEmail;
            
            // Show sign-in panel
            ShowSignInPanel();
        }
        else
        {
            Debug.LogError("Sign-up failed: " + response);
            ShowSignUpError("Sign-up failed: " + response);
        }
    }
    
    // Sign In Logic
    private void HandleSignIn()
    {
        if (signInErrorText) signInErrorText.gameObject.SetActive(false);
        
        // Validate form
        string errorMessage = ValidateSignInForm();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ShowSignInError(errorMessage);
            return;
        }
        
        // Show loading
        if (loadingIndicator) loadingIndicator.SetActive(true);
        
        // Create sign-in data
        SignInData signInData = new SignInData
        {
            email = signInEmailField.text,
            password = signInPasswordField.text
        };
        
        // Send sign-in request
        StartCoroutine(SendSignInRequest(signInData));
    }
    
    private string ValidateSignInForm()
    {
        // Check for empty fields
        if (string.IsNullOrWhiteSpace(signInEmailField.text))
            return "Email is required.";
            
        if (string.IsNullOrWhiteSpace(signInPasswordField.text))
            return "Password is required.";
            
        return string.Empty;
    }
    
    private void ShowSignInError(string message)
    {
        if (signInErrorText)
        {
            signInErrorText.text = message;
            signInErrorText.gameObject.SetActive(true);
        }
    }
      private IEnumerator SendSignInRequest(SignInData signInData)
    {
        // Convert to JSON
        string jsonData = JsonUtility.ToJson(signInData);
        
        // Use the network manager to send the sign-in request
        bool success = false;
        string response = "";
        
        // Use the configured URL
        string signInUrl = config.signInApiUrl;
        
        using (UnityWebRequest webRequest = new UnityWebRequest(signInUrl, "POST"))
        {
            // Set up the request
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            
            // Send the request
            Debug.Log($"Sending sign-in request to {signInUrl}");
            yield return webRequest.SendWebRequest();
            
            // Check for network errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Network error: {webRequest.error}");
                response = $"Network error: {webRequest.error}";
                success = false;
            }
            // Check for HTTP errors
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"HTTP Error: {webRequest.error} - {webRequest.responseCode}");
                response = $"Server error: {webRequest.responseCode} - {webRequest.downloadHandler.text}";
                success = false;
            }
            // Success
            else
            {
                Debug.Log("Sign-in request successful");
                response = webRequest.downloadHandler.text;
                success = true;
            }
        }
        
        // Hide loading
        if (loadingIndicator) loadingIndicator.SetActive(false);
        
        // If this is a demo/prototype and no server is available yet
        // you can uncomment this to simulate a successful sign-in
        /*
        success = true;
        response = "{\"success\":true,\"userId\":\"12345\",\"token\":\"example-token\"}";
        */
        
        OnSignInComplete(success, response);
    }
    
    private void OnSignInComplete(bool success, string response)
    {
        if (success)
        {
            Debug.Log("Sign-in successful");
            
            // Set authenticated flag
            isAuthenticated = true;
            
            // Save user email for display purposes
            currentUserEmail = signInEmailField.text;
            
            // In a real app, you would parse the response to get user details
            // and store the authentication token for future API calls
            
            // Show main menu or game screen
            ShowMainMenu();
        }
        else
        {
            Debug.LogError("Sign-in failed: " + response);
            ShowSignInError("Invalid email or password. Please try again.");
        }
    }
      // Helper method to get current user info
    public string GetCurrentUserEmail()
    {
        return currentUserEmail;
    }
    
    public bool IsUserAuthenticated()
    {
        return isAuthenticated;
    }
    
    // Sign out the current user
    public void SignOut()
    {
        isAuthenticated = false;
        currentUserEmail = string.Empty;
        
        // Clear input fields for security
        if (signInEmailField) signInEmailField.text = string.Empty;
        if (signInPasswordField) signInPasswordField.text = string.Empty;
        if (signUpEmailField) signUpEmailField.text = string.Empty;
        if (signUpPasswordField) signUpPasswordField.text = string.Empty;
        if (signUpConfirmPasswordField) signUpConfirmPasswordField.text = string.Empty;
        
        // Show sign in panel
        ShowSignInPanel();
        
        Debug.Log("User signed out successfully");
    }
}

// Data structure for sign-in request
[System.Serializable]
public class SignInData
{
    public string email;
    public string password;
}
