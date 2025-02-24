using UnityEngine;

public class GetAvatarBoneTree : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Export all the bones of the avatar with unique log
        Transform[] bones = GetComponentsInChildren<Transform>();
        string boneNames = "";
        foreach (Transform bone in bones)
        {
            boneNames += bone.name + "; ";
        }
        Debug.Log("Bones: " + boneNames);
    }
}
