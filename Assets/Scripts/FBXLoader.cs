using UnityEngine;
using System.IO;
using GLTFast;
using UnityEngine.Animations;
using System;

public class FBXLoader : MonoBehaviour
{
    public string filePath = "C:/Users/alelo/Desktop/wem_webapp/test1.glb"; // Percorso locale
    //public string filePath = "https://http://127.0.0.1:5500/test1.glb"; // Se vuoi caricare da web

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

    async void Start()
    {
        LoadGltfBinaryFromMemory();
    }

    async void LoadGltfBinaryFromMemory() {
        var gltf = new GLTFast.GltfImport();

        var success = await gltf.Load("http://127.0.0.1:5500/test1.glb");	
        if (!success) {
            Debug.LogError("Failed to load gltf");
            return;
        } else {
            Debug.Log("Loaded gltf");
            GameObject gltfObject = new GameObject("GLTF");
            await gltf.InstantiateSceneAsync(gltfObject.transform);
            gltfObject.transform.SetPositionAndRotation(new Vector3(0, 1.35f, 1.0f), Quaternion.Euler(0, 180, 0));
            Animator animator_ = gltfObject.AddComponent<Animator>();
            animator_.avatar = AvatarBuilder.BuildHumanAvatar(gltfObject, humanDescription);
            gltfObject.AddComponent<JointsStateSubscriber_SMPL>();
            Debug.Log("Instantiated gltf");
        }
    }

    /**
    * @Brief load the GLTF/GLB model from file
    * @Param path: the path of the file
    *//*
   private void LoadGLTF(string path)
    {
        GameObject result = Importer.LoadFromFile(path);
        result.transform.SetPositionAndRotation(new Vector3(0, 1.35f, 1.0f), Quaternion.Euler(0, 180, 0));
        Animator animator_ = result.AddComponent<Animator>();
        animator_.avatar = AvatarBuilder.BuildHumanAvatar(result, humanDescription);
        result.AddComponent<JointsStateSubscriber_SMPL>();
    }*/
}