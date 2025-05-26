using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SignUpForm : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public Button submitButton;
    public TextMeshProUGUI errorMessageText;
    
    [Header("Form Settings")]
    public bool validateEmailFormat = true;
    public bool requireStrongPassword = true;
    public int minimumPasswordLength = 8;
    
    // Reference to the data handler
    private UserDataHandler dataHandler;
    
    private void Start()
    {
        // Initialize the data handler
        dataHandler = GetComponent<UserDataHandler>();
        if (dataHandler == null)
        {
            dataHandler = gameObject.AddComponent<UserDataHandler>();
        }
        
        // Add listeners to UI elements
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitButtonClicked);
        }
        
        // Hide error message at start
        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        // Clean up listeners
        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(OnSubmitButtonClicked);
        }
    }
    
    private void OnSubmitButtonClicked()
    {
        // Hide any previous error message
        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
        
        // Validate input fields
        string errorMessage = ValidateForm();
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            // Show error message
            if (errorMessageText != null)
            {
                errorMessageText.text = errorMessage;
                errorMessageText.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError(errorMessage);
            }
            return;
        }
        
        // If validation passed, submit the form
        SubmitForm();
    }
    
    private string ValidateForm()
    {
        // Check if any field is empty
        if (string.IsNullOrWhiteSpace(emailInputField.text))
        {
            return "Email is required.";
        }
        
        if (string.IsNullOrWhiteSpace(passwordInputField.text))
        {
            return "Password is required.";
        }
        
        if (string.IsNullOrWhiteSpace(confirmPasswordInputField.text))
        {
            return "Please confirm your password.";
        }
        
        // Check if passwords match
        if (passwordInputField.text != confirmPasswordInputField.text)
        {
            return "Passwords do not match.";
        }
        
        // Validate email format if enabled
        if (validateEmailFormat && !IsValidEmail(emailInputField.text))
        {
            return "Please enter a valid email address.";
        }
        
        // Check password strength if enabled
        if (requireStrongPassword)
        {
            if (passwordInputField.text.Length < minimumPasswordLength)
            {
                return $"Password must be at least {minimumPasswordLength} characters long.";
            }
            
            // Additional password strength checks can be added here
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;
            
            foreach (char c in passwordInputField.text)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                else if (char.IsLower(c)) hasLowerCase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else hasSpecialChar = true;
            }
            
            if (!(hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar))
            {
                return "Password must include uppercase, lowercase, digit, and special character.";
            }
        }
        
        return string.Empty; // No errors
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
    
    private void SubmitForm()
    {
        // Create user data
        UserData userData = new UserData
        {
            Email = emailInputField.text,
            Password = passwordInputField.text,
            CreateDate = DateTime.Now
        };
        
        // Pass data to handler for processing
        dataHandler.ProcessUserData(userData);
    }
}
