using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genies.Sdk;
using TMPro;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button instantLoginButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private Button submitEmailButton;
    [SerializeField] private TMP_InputField otpInputField;
    [SerializeField] private Button submitOTPButton;
    [SerializeField] private Button resendOTPButton;
    [SerializeField] private Button signupButton;

    [Header("Player Reference")]
    [SerializeField] private GameObject playerRoot;

    // Triggered when the user clicks the "Submit Email" button
    private async void OnSubmitEmail()
    {
        string email = emailInputField.text;
        statusText.text = $"Email submitted: {email}";
        (bool success, string errorMessage) = await AvatarSdk.StartLoginEmailOtpAsync(email);
        if (success)
        {
            statusText.text = "OTP sent successfully. Please check your email.";
        }
        else
        {
            statusText.text = "Error sending OTP: " + errorMessage;
        }
    }

    // Triggered when the user clicks the "Resend OTP" button
    private async void OnResendOTP()
    {
        statusText.text = "Resending OTP...";
        (bool success, string errorMessage) = await AvatarSdk.ResendEmailCodeAsync();
        if (success)
        {
            statusText.text = "OTP resent successfully. Please check your email.";
        }
        else
        {
            statusText.text = "Error resending OTP: " + errorMessage;
        }
    }

    // Triggered when the user clicks the "Submit OTP" button
    private async void OnSubmitOTP()
    {
        string otpCode = otpInputField.text;
        statusText.text = $"OTP submitted: {otpCode}";
        (bool success, string errorMessage) = await AvatarSdk.SubmitEmailOtpCodeAsync(otpCode);
        if (success)
        {
            statusText.text = "OTP verified successfully. User logged in.";
        }
        else
        {
            statusText.text = "Error verifying OTP: " + errorMessage;
        }
    }

    // Triggered when the user clicks the "Instant Login" button
    private async void OnInstantLogin()
    {
        statusText.text = "Attempting instant login...";
        var result = await AvatarSdk.TryInstantLoginAsync();
        if (result.isLoggedIn)
        {
            statusText.text = "Instant login successful.";
        }
        else
        {
            statusText.text = "Instant login failed.";
        }
    }

    // Triggered when the user successfully logs in
    private void OnUserLogin()
    {
        statusText.text = "User logged in.";
        playerRoot.SetActive(true);
        gameObject.SetActive(false);
    }

    // Triggered when the user clicks the "Logout" button
    private async void OnLogout()
    {
        statusText.text = "Logging out...";
        await AvatarSdk.LogOutAsync();
        statusText.text = "Logged out.";
    }

    // Triggered when the user clicks the "Sign Up" button
    public void OnSignUp()
    {
        Application.OpenURL(AvatarSdk.UrlGeniesHubSignUp);
    }

    private void Start()
    {
        statusText.text = "Ready";

        // Button listeners
        submitEmailButton.onClick.AddListener(OnSubmitEmail);
        submitOTPButton.onClick.AddListener(OnSubmitOTP);
        logoutButton.onClick.AddListener(OnLogout);
        instantLoginButton.onClick.AddListener(OnInstantLogin);
        resendOTPButton.onClick.AddListener(OnResendOTP);
        signupButton.onClick.AddListener(OnSignUp);

        // Event listeners
        AvatarSdk.Events.UserLoggedIn += OnUserLogin;
    }

    private void OnDestroy()
    {
        // Remove button listeners
        submitEmailButton.onClick.RemoveListener(OnSubmitEmail);
        submitOTPButton.onClick.RemoveListener(OnSubmitOTP);
        logoutButton.onClick.RemoveListener(OnLogout);
        instantLoginButton.onClick.RemoveListener(OnInstantLogin);
        resendOTPButton.onClick.RemoveListener(OnResendOTP);
        signupButton.onClick.RemoveListener(OnSignUp);

        // Clean up event listeners
        AvatarSdk.Events.UserLoggedIn -= OnUserLogin;
    }
}
