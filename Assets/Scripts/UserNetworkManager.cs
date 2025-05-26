using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class UserNetworkManager : MonoBehaviour
{
    [Header("Network Settings")]
    public bool useSecureConnection = true;
    public int maxRetryAttempts = 3;
    
    // Callback type for sign-up response
    public delegate void SignUpCallback(bool success, string response);
    
    // Method to send sign-up data to server
    public IEnumerator SendSignUpData(string apiUrl, string jsonData, SignUpCallback callback)
    {
        int retryCount = 0;
        bool requestComplete = false;
        bool requestSuccess = false;
        string responseMessage = "";
        
        while (!requestComplete && retryCount < maxRetryAttempts)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
            {
                // Set up the request
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                
                // Optional: Add authentication headers if needed
                // webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
                
                // Send the request
                Debug.Log($"Sending sign-up request to {apiUrl}");
                yield return webRequest.SendWebRequest();
                
                // Check for network errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError($"Network error: {webRequest.error}");
                    responseMessage = $"Network error: {webRequest.error}";
                    retryCount++;
                    
                    if (retryCount < maxRetryAttempts)
                    {
                        Debug.Log($"Retrying... Attempt {retryCount + 1} of {maxRetryAttempts}");
                        yield return new WaitForSeconds(1f); // Wait before retry
                    }
                    else
                    {
                        requestComplete = true;
                        requestSuccess = false;
                    }
                }
                // Check for HTTP errors
                else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"HTTP Error: {webRequest.error} - {webRequest.responseCode}");
                    responseMessage = $"Server error: {webRequest.responseCode} - {webRequest.downloadHandler.text}";
                    requestComplete = true;
                    requestSuccess = false;
                }
                // Success
                else
                {
                    Debug.Log("Sign-up request successful");
                    responseMessage = webRequest.downloadHandler.text;
                    requestComplete = true;
                    requestSuccess = true;
                }
            }
        }
        
        // Invoke callback with results
        callback?.Invoke(requestSuccess, responseMessage);
    }
    
    // Method to validate server certificate (if using HTTPS)
    public bool ValidateServerCertificate(string serverUrl)
    {
        // This is a simple implementation. For production,
        // you should implement proper certificate validation.
        return true;
    }
}
