using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SignUpFormSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Custom/Sign-Up Form")]
    public static void CreateSignUpForm()
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
        
        // Create the sign-up form panel
        GameObject signUpPanel = new GameObject("SignUpPanel");
        signUpPanel.transform.SetParent(canvas.transform, false);
        
        // Add panel components
        RectTransform panelRect = signUpPanel.AddComponent<RectTransform>();
        Image panelImage = signUpPanel.AddComponent<Image>();
        panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        
        // Set panel size and position
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(400, 500);
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add title
        GameObject titleObj = CreateTextObject("Title", signUpPanel.transform, "Sign Up", 24);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 200);
        
        // Add email input field
        GameObject emailField = CreateInputField("EmailField", signUpPanel.transform, "Email");
        RectTransform emailRect = emailField.GetComponent<RectTransform>();
        emailRect.anchoredPosition = new Vector2(0, 120);
        
        // Add password input field
        GameObject passwordField = CreateInputField("PasswordField", signUpPanel.transform, "Password");
        RectTransform passwordRect = passwordField.GetComponent<RectTransform>();
        passwordRect.anchoredPosition = new Vector2(0, 40);
        
        // Set password field to hide input
        TMP_InputField passwordInput = passwordField.GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        
        // Add confirm password input field
        GameObject confirmPasswordField = CreateInputField("ConfirmPasswordField", signUpPanel.transform, "Confirm Password");
        RectTransform confirmPasswordRect = confirmPasswordField.GetComponent<RectTransform>();
        confirmPasswordRect.anchoredPosition = new Vector2(0, -40);
        
        // Set confirm password field to hide input
        TMP_InputField confirmPasswordInput = confirmPasswordField.GetComponent<TMP_InputField>();
        confirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
        
        // Add submit button
        GameObject submitButton = CreateButton("SubmitButton", signUpPanel.transform, "Submit");
        RectTransform submitButtonRect = submitButton.GetComponent<RectTransform>();
        submitButtonRect.anchoredPosition = new Vector2(0, -120);
        
        // Add error message text
        GameObject errorMessage = CreateTextObject("ErrorMessage", signUpPanel.transform, "", 14);
        RectTransform errorRect = errorMessage.GetComponent<RectTransform>();
        errorRect.anchoredPosition = new Vector2(0, -180);
        TextMeshProUGUI errorText = errorMessage.GetComponent<TextMeshProUGUI>();
        errorText.color = Color.red;
        errorText.gameObject.SetActive(false);
        
        // Add the required scripts
        SignUpForm signUpForm = signUpPanel.AddComponent<SignUpForm>();
        UserDataHandler dataHandler = signUpPanel.AddComponent<UserDataHandler>();
        UserNetworkManager networkManager = signUpPanel.AddComponent<UserNetworkManager>();
        
        // Connect the references
        signUpForm.emailInputField = emailField.GetComponent<TMP_InputField>();
        signUpForm.passwordInputField = passwordField.GetComponent<TMP_InputField>();
        signUpForm.confirmPasswordInputField = confirmPasswordField.GetComponent<TMP_InputField>();
        signUpForm.submitButton = submitButton.GetComponent<Button>();
        signUpForm.errorMessageText = errorText;
        
        // Select the created panel in the hierarchy
        Selection.activeGameObject = signUpPanel;
        
        Debug.Log("Sign-up form created successfully!");
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
        rectTransform.sizeDelta = new Vector2(200, 60);
        image.color = new Color(0.2f, 0.6f, 1f);
        button.targetGraphic = image;
        
        // Create text
        GameObject textObj = CreateTextObject("Text", buttonObj.transform, label, 18);
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
        
        return textObj;
    }
#endif
}
