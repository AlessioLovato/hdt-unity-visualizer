using UnityEngine;
// Used to import functions from the js library
using System.Runtime.InteropServices;
using UnityEditor;


public class CharacterSelector : MonoBehaviour
{
    // Array of GameObjects that will be used to store the characters
    public GameObject[] characters;

    
    #if !UNITY_EDITOR
        // Import functions from the js library
        [DllImport("__Internal")]
        private static extern int getCharacterIndex();
        [DllImport("__Internal")]
        private static extern string getCharacterFilePath();
        [DllImport("__Internal")]
        private static extern string getPageUrl();
    #endif

    // value to store the selected character
    #if UNITY_EDITOR
        public int index = 1;
        public string filePath = "";
        public string webpage = "file://C:/Users/alelo/Desktop/wem_webapp/";
    #else
        private int index = getCharacterIndex();
        private string filePath = getCharacterFilePath();
        private string webpage = getPageUrl(); 
    #endif

    // Update is called once per frame
    void Start()
    {
        // Run application in background
        Application.runInBackground = true;
        // Loop through all the characters and disable them
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        // Enable the selected character
        if (index <= (characters.Length - 1)) { // selected one of the default characters (-1 because index starts from 1)
            if (index < 0)
                index = 1;
            characters[index].SetActive(true);
        } else if (filePath != "") { // selected a custom character
            string path = webpage + "Avatar/" + filePath;
            LoadGLB(path);
        } else {
            characters[1].SetActive(true); // Default character
        }
        
    }

    async void LoadGLB(string path) {
        var gltf = new GLTFast.GltfImport();
        var success = await gltf.Load(path);	
        if (!success) {
            Debug.LogError("Failed to load gltf. Given path: " + path);
            return;
        } else {
            GameObject gltfObject = new GameObject("GLTF");
            await gltf.InstantiateSceneAsync(gltfObject.transform);
            gltfObject.transform.SetPositionAndRotation(new Vector3(0, 1, -1.5f), Quaternion.Euler(0, 180, 0));
            Animator animator_ = gltfObject.AddComponent<Animator>();
            animator_.avatar = AvatarBuilder.BuildHumanAvatar(gltfObject, humanDescription);
            gltfObject.AddComponent<JointsStateSubscriber_SMPL>();
            gltfObject.AddComponent<PlayerController>();
        }
    }

    HumanDescription humanDescription = new HumanDescription
    {
        human = new HumanBone[]
            {
            new HumanBone { boneName = "pelvis", humanName = "Hips" },
            new HumanBone { boneName = "spine_01", humanName = "Spine" },
            new HumanBone { boneName = "spine_02", humanName = "Chest" },
            new HumanBone { boneName = "spine_03", humanName = "UpperChest" },
            new HumanBone { boneName = "neck_01", humanName = "Neck" },
            new HumanBone { boneName = "head", humanName = "Head" },
            new HumanBone { boneName = "clavicle_l", humanName = "LeftShoulder" },
            new HumanBone { boneName = "upperarm_l", humanName = "LeftUpperArm" },
            new HumanBone { boneName = "lowerarm_l", humanName = "LeftLowerArm" },
            new HumanBone { boneName = "hand_l", humanName = "LeftHand" },
            new HumanBone { boneName = "clavicle_r", humanName = "RightShoulder" },
            new HumanBone { boneName = "upperarm_r", humanName = "RightUpperArm" },
            new HumanBone { boneName = "lowerarm_r", humanName = "RightLowerArm" },
            new HumanBone { boneName = "hand_r", humanName = "RightHand" },
            new HumanBone { boneName = "thigh_l", humanName = "LeftUpperLeg" },
            new HumanBone { boneName = "calf_l", humanName = "LeftLowerLeg" },
            new HumanBone { boneName = "foot_l", humanName = "LeftFoot" },
            new HumanBone { boneName = "ball_l", humanName = "LeftToes" },
            new HumanBone { boneName = "thigh_r", humanName = "RightUpperLeg" },
            new HumanBone { boneName = "calf_r", humanName = "RightLowerLeg" },
            new HumanBone { boneName = "foot_r", humanName = "RightFoot" },
            new HumanBone { boneName = "ball_r", humanName = "RightToes" }
            },
        skeleton = new SkeletonBone[]
            {
            new SkeletonBone { name = "Meshcapade Avatar" },
            new SkeletonBone { name = "SceneRoot" },
            new SkeletonBone { name = "Armature_0" },
            new SkeletonBone { name = "root" },
            new SkeletonBone { name = "pelvis" },
            new SkeletonBone { name = "spine_01" },
            new SkeletonBone { name = "spine_02" },
            new SkeletonBone { name = "spine_03" },
            new SkeletonBone { name = "neck_01" },
            new SkeletonBone { name = "head" },
            new SkeletonBone { name = "clavicle_l" },
            new SkeletonBone { name = "upperarm_l" },
            new SkeletonBone { name = "lowerarm_l" },
            new SkeletonBone { name = "hand_l" },
            new SkeletonBone { name = "clavicle_r" },
            new SkeletonBone { name = "upperarm_r" },
            new SkeletonBone { name = "lowerarm_r" },
            new SkeletonBone { name = "hand_r" },
            new SkeletonBone { name = "thigh_l" },
            new SkeletonBone { name = "calf_l" },
            new SkeletonBone { name = "foot_l" },
            new SkeletonBone { name = "ball_l" },
            new SkeletonBone { name = "thigh_r" },
            new SkeletonBone { name = "calf_r" },
            new SkeletonBone { name = "foot_r" },
            new SkeletonBone { name = "ball_r" }
            }
    };
}
