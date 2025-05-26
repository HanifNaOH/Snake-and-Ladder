using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AuthenticationFormSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Snake and Ladder/Authentication System")]
    public static void CreateAuthenticationSystem()
    {
        // Check if we have a Canvas in the scene
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            // Create a new Canvas if it doesn't exist
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Add an event system if it doesn't exist
            if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }
          // Create the Authentication Manager
        GameObject authManagerObj = new GameObject("AuthenticationManager");
        AuthenticationManager authManager = authManagerObj.AddComponent<AuthenticationManager>();
        UserDataHandler dataHandler = authManagerObj.AddComponent<UserDataHandler>();
        UserNetworkManager networkManager = authManagerObj.AddComponent<UserNetworkManager>();
        
        // Load the AuthenticationConfig
        AuthenticationConfig config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
        if (config == null)
        {
            // Create the config if it doesn't exist
            Debug.Log("Authentication Config not found. Creating a new one...");
            AuthenticationConfigSetup.SetupAuthenticationConfig();
            config = Resources.Load<AuthenticationConfig>("AuthenticationConfig");
        }
        
        // Assign the config to the AuthenticationManager
        authManager.config = config;
        
        // Create the Authentication UI Container
        GameObject authUIContainer = new GameObject("AuthenticationUI");
        authUIContainer.transform.SetParent(canvas.transform, false);
        RectTransform containerRect = authUIContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;
        
        // Create SignUp Panel
        GameObject signUpPanel = CreatePanel("SignUpPanel", authUIContainer.transform, "Sign Up");
        
        // Create SignIn Panel
        GameObject signInPanel = CreatePanel("SignInPanel", authUIContainer.transform, "Sign In");
        
        // Create Main Menu Panel (placeholder)
        GameObject mainMenuPanel = CreatePanel("MainMenuPanel", authUIContainer.transform, "Main Menu");
        mainMenuPanel.SetActive(false);
        
        // Create a welcome message in the main menu
        GameObject welcomeText = CreateTextObject("WelcomeText", mainMenuPanel.transform, "Welcome to Snake and Ladder!", 24);
        RectTransform welcomeTextRect = welcomeText.GetComponent<RectTransform>();
        welcomeTextRect.anchoredPosition = new Vector2(0, 100);
        
        // Create a play button in the main menu
        GameObject playButton = CreateButton("PlayButton", mainMenuPanel.transform, "Play Game");
        RectTransform playButtonRect = playButton.GetComponent<RectTransform>();
        playButtonRect.anchoredPosition = new Vector2(0, 0);
        
        // Add loading indicator
        GameObject loadingIndicator = CreateLoadingIndicator(authUIContainer.transform);
        loadingIndicator.SetActive(false);
        
        // Connect everything to the AuthenticationManager
        authManager.signUpPanel = signUpPanel;
        authManager.signInPanel = signInPanel;
        authManager.mainMenuPanel = mainMenuPanel;
        authManager.loadingIndicator = loadingIndicator;
        
        // Setup Sign Up form elements
        TMP_InputField signUpEmailField = GetComponentInChildren<TMP_InputField>(signUpPanel, "EmailField");
        TMP_InputField signUpPasswordField = GetComponentInChildren<TMP_InputField>(signUpPanel, "PasswordField");
        TMP_InputField signUpConfirmPasswordField = GetComponentInChildren<TMP_InputField>(signUpPanel, "ConfirmPasswordField");
        Button signUpSubmitButton = GetComponentInChildren<Button>(signUpPanel, "SubmitButton");
        TextMeshProUGUI signUpErrorText = GetComponentInChildren<TextMeshProUGUI>(signUpPanel, "ErrorMessage");
        Button switchToSignInButton = GetComponentInChildren<Button>(signUpPanel, "SwitchFormButton");
        
        // Setup Sign In form elements
        TMP_InputField signInEmailField = GetComponentInChildren<TMP_InputField>(signInPanel, "EmailField");
        TMP_InputField signInPasswordField = GetComponentInChildren<TMP_InputField>(signInPanel, "PasswordField");
        Button signInSubmitButton = GetComponentInChildren<Button>(signInPanel, "SubmitButton");
        TextMeshProUGUI signInErrorText = GetComponentInChildren<TextMeshProUGUI>(signInPanel, "ErrorMessage");
        Button switchToSignUpButton = GetComponentInChildren<Button>(signInPanel, "SwitchFormButton");
        
        // Set references in the AuthenticationManager
        authManager.signUpEmailField = signUpEmailField;
        authManager.signUpPasswordField = signUpPasswordField;
        authManager.signUpConfirmPasswordField = signUpConfirmPasswordField;
        authManager.signUpSubmitButton = signUpSubmitButton;
        authManager.signUpErrorText = signUpErrorText;
        authManager.switchToSignInButton = switchToSignInButton;
        
        authManager.signInEmailField = signInEmailField;
        authManager.signInPasswordField = signInPasswordField;
        authManager.signInSubmitButton = signInSubmitButton;
        authManager.signInErrorText = signInErrorText;
        authManager.switchToSignUpButton = switchToSignUpButton;
        
        // Select the created Authentication Manager in the hierarchy
        Selection.activeGameObject = authManagerObj;
        
        Debug.Log("Authentication system created successfully!");
    }
    
    private static GameObject CreatePanel(string name, Transform parent, string title)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        Image panelImage = panel.AddComponent<Image>();
        
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = new Vector2(100, 100);
        panelRect.offsetMax = new Vector2(-100, -100);
        panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        
        // Add title
        GameObject titleObj = CreateTextObject("Title", panel.transform, title, 24);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 160);
        
        if (name == "SignUpPanel")
        {
            SetupSignUpForm(panel.transform);
        }
        else if (name == "SignInPanel")
        {
            SetupSignInForm(panel.transform);
        }
        
        return panel;
    }
    
    private static void SetupSignUpForm(Transform parent)
    {
        // Email field
        GameObject emailField = CreateInputField("EmailField", parent, "Email");
        RectTransform emailRect = emailField.GetComponent<RectTransform>();
        emailRect.anchoredPosition = new Vector2(0, 100);
        
        // Password field
        GameObject passwordField = CreateInputField("PasswordField", parent, "Password");
        RectTransform passwordRect = passwordField.GetComponent<RectTransform>();
        passwordRect.anchoredPosition = new Vector2(0, 40);
        
        // Set password field to hide input
        TMP_InputField passwordInput = passwordField.GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        
        // Confirm password field
        GameObject confirmPasswordField = CreateInputField("ConfirmPasswordField", parent, "Confirm Password");
        RectTransform confirmPasswordRect = confirmPasswordField.GetComponent<RectTransform>();
        confirmPasswordRect.anchoredPosition = new Vector2(0, -20);
        
        // Set confirm password field to hide input
        TMP_InputField confirmPasswordInput = confirmPasswordField.GetComponent<TMP_InputField>();
        confirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
        
        // Submit button
        GameObject submitButton = CreateButton("SubmitButton", parent, "Sign Up");
        RectTransform submitButtonRect = submitButton.GetComponent<RectTransform>();
        submitButtonRect.anchoredPosition = new Vector2(0, -80);
        
        // Switch form button
        GameObject switchFormButton = CreateButton("SwitchFormButton", parent, "Already have an account? Sign In");
        RectTransform switchFormButtonRect = switchFormButton.GetComponent<RectTransform>();
        switchFormButtonRect.anchoredPosition = new Vector2(0, -130);
        
        // Error message
        GameObject errorMessage = CreateTextObject("ErrorMessage", parent, "", 14);
        RectTransform errorRect = errorMessage.GetComponent<RectTransform>();
        errorRect.anchoredPosition = new Vector2(0, -180);
        TextMeshProUGUI errorText = errorMessage.GetComponent<TextMeshProUGUI>();
        errorText.color = Color.red;
        errorText.gameObject.SetActive(false);
    }
    
    private static void SetupSignInForm(Transform parent)
    {
        // Email field
        GameObject emailField = CreateInputField("EmailField", parent, "Email");
        RectTransform emailRect = emailField.GetComponent<RectTransform>();
        emailRect.anchoredPosition = new Vector2(0, 80);
        
        // Password field
        GameObject passwordField = CreateInputField("PasswordField", parent, "Password");
        RectTransform passwordRect = passwordField.GetComponent<RectTransform>();
        passwordRect.anchoredPosition = new Vector2(0, 20);
        
        // Set password field to hide input
        TMP_InputField passwordInput = passwordField.GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        
        // Submit button
        GameObject submitButton = CreateButton("SubmitButton", parent, "Sign In");
        RectTransform submitButtonRect = submitButton.GetComponent<RectTransform>();
        submitButtonRect.anchoredPosition = new Vector2(0, -40);
        
        // Switch form button
        GameObject switchFormButton = CreateButton("SwitchFormButton", parent, "Don't have an account? Sign Up");
        RectTransform switchFormButtonRect = switchFormButton.GetComponent<RectTransform>();
        switchFormButtonRect.anchoredPosition = new Vector2(0, -90);
        
        // Error message
        GameObject errorMessage = CreateTextObject("ErrorMessage", parent, "", 14);
        RectTransform errorRect = errorMessage.GetComponent<RectTransform>();
        errorRect.anchoredPosition = new Vector2(0, -140);
        TextMeshProUGUI errorText = errorMessage.GetComponent<TextMeshProUGUI>();
        errorText.color = Color.red;
        errorText.gameObject.SetActive(false);
    }
    
    private static GameObject CreateLoadingIndicator(Transform parent)
    {
        GameObject loadingObj = new GameObject("LoadingIndicator");
        loadingObj.transform.SetParent(parent, false);
        
        RectTransform loadingRect = loadingObj.AddComponent<RectTransform>();
        Image loadingBgImage = loadingObj.AddComponent<Image>();
        
        loadingRect.anchorMin = Vector2.zero;
        loadingRect.anchorMax = Vector2.one;
        loadingRect.offsetMin = Vector2.zero;
        loadingRect.offsetMax = Vector2.zero;
        loadingBgImage.color = new Color(0, 0, 0, 0.7f);
        
        // Create spinner
        GameObject spinnerObj = new GameObject("Spinner");
        spinnerObj.transform.SetParent(loadingObj.transform, false);
        
        RectTransform spinnerRect = spinnerObj.AddComponent<RectTransform>();
        Image spinnerImage = spinnerObj.AddComponent<Image>();
        
        spinnerRect.anchoredPosition = Vector2.zero;
        spinnerRect.sizeDelta = new Vector2(100, 100);
        
        // Use a circular image for the spinner
        spinnerImage.color = Color.white;
        spinnerImage.sprite = Sprite.Create(EditorGUIUtility.whiteTexture, new Rect(0, 0, EditorGUIUtility.whiteTexture.width, EditorGUIUtility.whiteTexture.height), new Vector2(0.5f, 0.5f)); // Just a placeholder
        
        // Add a text saying "Loading..."
        GameObject loadingTextObj = CreateTextObject("LoadingText", loadingObj.transform, "Loading...", 18);
        RectTransform loadingTextRect = loadingTextObj.GetComponent<RectTransform>();
        loadingTextRect.anchoredPosition = new Vector2(0, -80);
        
        return loadingObj;
    }
    
    private static GameObject CreateInputField(string name, Transform parent, string placeholder)
    {
        // Create main GameObject
        GameObject inputFieldObj = new GameObject(name);
        inputFieldObj.transform.SetParent(parent, false);
        
        // Add components
        RectTransform rectTransform = inputFieldObj.AddComponent<RectTransform>();
        Image image = inputFieldObj.AddComponent<Image>();
        TMP_InputField inputField = inputFieldObj.AddComponent<TMP_InputField>();
        
        // Set appearance
        rectTransform.sizeDelta = new Vector2(300, 50);
        image.color = Color.white;
        
        // Create text area
        GameObject textArea = new GameObject("Text Area");
        textArea.transform.SetParent(inputFieldObj.transform, false);
        RectTransform textAreaRect = textArea.AddComponent<RectTransform>();
        textAreaRect.anchorMin = Vector2.zero;
        textAreaRect.anchorMax = Vector2.one;
        textAreaRect.offsetMin = new Vector2(10, 6);
        textAreaRect.offsetMax = new Vector2(-10, -6);
        
        // Create text component for input
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(textArea.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.color = Color.black;
        text.fontSize = 18;
        text.alignment = TextAlignmentOptions.Left;
        
        // Create placeholder text
        GameObject placeholderObj = new GameObject("Placeholder");
        placeholderObj.transform.SetParent(textArea.transform, false);
        RectTransform placeholderRect = placeholderObj.AddComponent<RectTransform>();
        placeholderRect.anchorMin = Vector2.zero;
        placeholderRect.anchorMax = Vector2.one;
        placeholderRect.offsetMin = Vector2.zero;
        placeholderRect.offsetMax = Vector2.zero;
        TextMeshProUGUI placeholderText = placeholderObj.AddComponent<TextMeshProUGUI>();
        placeholderText.text = placeholder;
        placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        placeholderText.fontSize = 18;
        placeholderText.alignment = TextAlignmentOptions.Left;
        
        // Configure input field
        inputField.targetGraphic = image;
        inputField.textViewport = textAreaRect;
        inputField.textComponent = text;
        inputField.placeholder = placeholderText;
        inputField.fontAsset = text.font;
        
        return inputFieldObj;
    }
    
    private static GameObject CreateButton(string name, Transform parent, string label)
    {
        // Create main GameObject
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        // Add components
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        Image image = buttonObj.AddComponent<Image>();
        Button button = buttonObj.AddComponent<Button>();
        
        // Set appearance
        rectTransform.sizeDelta = new Vector2(300, 50);
        
        // Use a nice blue color for the button
        if (name.Contains("Submit"))
        {
            image.color = new Color(0.2f, 0.6f, 1f);
        }
        else if (name.Contains("Switch"))
        {
            // Make switch buttons less prominent
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            image.color = new Color(0.3f, 0.7f, 0.3f);
        }
        
        button.targetGraphic = image;
        
        // Create text
        GameObject textObj = CreateTextObject("Text", buttonObj.transform, label, 16);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return buttonObj;
    }
    
    private static GameObject CreateTextObject(string name, Transform parent, string content, int fontSize)
    {
        // Create main GameObject
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        // Add components
        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        
        // Set appearance
        rectTransform.sizeDelta = new Vector2(300, 50);
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        
        return textObj;
    }
    
    private static T GetComponentInChildren<T>(GameObject parent, string childName) where T : Component
    {
        Transform child = parent.transform.Find(childName);
        if (child != null)
        {
            return child.GetComponent<T>();
        }
        return null;
    }
#endif
}
