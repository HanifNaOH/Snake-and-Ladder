# Snake and Ladder Authentication System

This README provides instructions on how to set up the authentication system in your Snake and Ladder game.

## Setting Up the Authentication System in the Login Scene

There are two ways to set up the authentication system:

### Method 1: Using the Menu Option (Recommended)

1. Open your Unity project
2. In the Unity Editor menu, go to: **Snake and Ladder > Setup Login Scene**
3. This will automatically:
   - Open the Login scene
   - Add the authentication system to the scene
   - Save the scene

### Method 2: Manual Setup

1. Open the Login scene from `Assets/Scenes/Login.unity`
2. In the Unity Editor menu, go to: **GameObject > UI > Snake and Ladder > Authentication System**
3. This will create the authentication system in the current scene
4. Save the scene

## Connecting Login to Gameplay

After setting up the authentication system, you can connect it to your Gameplay scene:

1. In the Unity Editor menu, go to: **Snake and Ladder > Connect Login to Gameplay**
2. This will:
   - Add both scenes to build settings if they're not already included
   - Add a SceneLoader component to the AuthenticationManager
   - Connect the "Play Game" button to load the Gameplay scene
   - Save the scene

## How to Customize

### API Endpoints

1. Select the AuthenticationManager in the Hierarchy
2. In the Inspector, find:
   - Sign Up Api Url - Set to your sign-up endpoint
   - Sign In Api Url - Set to your sign-in endpoint

### UI Appearance

You can customize the appearance of the UI elements by:

1. Selecting the Canvas > AuthenticationUI in the Hierarchy
2. Modifying the colors, fonts, and layouts of the panels and elements

## Testing the Authentication Flow

1. Enter the Play mode
2. The sign-in form should appear first
3. Click "Don't have an account? Sign Up" to switch to the sign-up form
4. Enter an email and password, then click "Sign Up"
5. After signing up, you'll be redirected to the sign-in form
6. Enter your credentials and click "Sign In"
7. The main menu will appear with a "Play Game" button
8. Click "Play Game" to load the Gameplay scene

## Important Notes

- The current implementation simulates network communication for sign-in. In a real application, you'll need to modify the `SendSignInRequest` method in `AuthenticationManager.cs` to communicate with your actual server.
- For security, passwords are masked in the input fields.
- Form validation includes checking for valid email format and matching passwords.
