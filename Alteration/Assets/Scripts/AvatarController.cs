using Genies.Sdk;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField] private string avatarName = "MyAvatar";
    [SerializeField] private Transform parentTransform = null;
    [SerializeField] private RuntimeAnimatorController animatorController = null;

    private void Start()
    {
        if (AvatarSdk.IsLoggedIn)
        {
            SpawnUserAvatar();
        }
        else
        {
            AvatarSdk.Events.UserLoggedIn += SpawnUserAvatar;
        }
    }

    private void OnDestroy()
    {
        AvatarSdk.Events.UserLoggedIn -= SpawnUserAvatar;
    }

    async void SpawnUserAvatar()
    {
        ManagedAvatar userAvatar = await AvatarSdk.LoadUserAvatarAsync(avatarName, parentTransform, animatorController);
        Debug.Log("User Avatar Loaded: " + userAvatar.Root.name);
    }
}