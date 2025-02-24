using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json.Linq;

// Used to import functions from the js library
using System.Runtime.InteropServices;
using System;

public class JointsStateSubscriber_SMPL : MonoBehaviour
{
    [Tooltip("The global angles (in degrees) to position the character to look in the z direction and have the x direction to his right (y direction up).")]
    public float xAngle = 0, yAngle = 0, zAngle = 0;

    private float abd_;
    private float ier_;
    private float fe_;
    public int abd_off, ier_off, fe_off, sh_adb_off, sh_ier_off, sh_fe_off;

    private string msg_name_;
    private string joint_name_;
    private string joint_angle_;
    private string side_;
    private int separator_position_;
    private int knee_rotation_;

    private Animator animator_;

    private Dictionary<string, HumanBodyBones> body_mecanim_bones_ = new Dictionary<string, HumanBodyBones>();
    private Dictionary<string, Vector3> body_mecanim_angles_ = new Dictionary<string, Vector3>();
    private Dictionary<HumanBodyBones, Quaternion> body_mecanim_n_pose_global2local_ = new Dictionary<HumanBodyBones, Quaternion>();
    private Dictionary<HumanBodyBones, Quaternion> body_mecanim_n_pose_local_ = new Dictionary<HumanBodyBones, Quaternion>();

    private WebSocket ws;

    #if !UNITY_EDITOR
        // Import functions from the js library
        [DllImport("__Internal")]
        private static extern string getWsUrl();
    #endif

    // Start is called before the first frame update
    async void Start()
    {
        animator_ = GetComponent<Animator>();

        Quaternion initial_rotation = transform.rotation;
        transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);
        mapMecanimBones();
        mapMecanimAngles();
        mapMecanimNpose();
        transform.rotation = initial_rotation;

        // Initialize the WebSocket connection to the rosbridge server
        #if UNITY_EDITOR
            ws = new WebSocket("ws://localhost:9090");
        #else
            ws = new WebSocket(getWsUrl());
        #endif

        ws.OnOpen += () =>
        {
            Debug.Log("WebSocket connection open!");
            SubscribeToJointStates();
        };

        ws.OnError += (e) =>
        {
            Debug.LogError("WebSocket error: " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("WebSocket connection closed!");
        };

        ws.OnMessage += (bytes) =>
        {
            try
            {
                // Reading a plain text message
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                OnMessageReceived(message);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error processing WebSocket message: " + ex.Message);
            }
        };

        await ws.Connect();
    }

    private void SubscribeToJointStates()
    {
        // Subscribe to the /joint_states topic
        JObject subscribeMessage = new JObject();
        subscribeMessage["op"] = "subscribe";
        subscribeMessage["topic"] = "/suit0/joints_state";
        ws.SendText(subscribeMessage.ToString());
    }

    private void OnMessageReceived(string messageData)
    {
        try
        {
            // Parse the received message
            JObject message = JObject.Parse(messageData);
            if ((string)message["topic"] == "/suit0/joints_state")
            {
                JArray positions = (JArray)message["msg"]["position"];
                JArray names = (JArray)message["msg"]["name"];

                for (int idx = 0; idx < names.Count; ++idx)
                {
                    msg_name_ = (string)names[idx];
                    separator_position_ = msg_name_.LastIndexOf('_');
                    if (separator_position_ != -1)
                    {
                        joint_name_ = msg_name_.Substring(0, separator_position_);
                        joint_angle_ = msg_name_.Substring(separator_position_ + 1, msg_name_.Length - (separator_position_ + 1));
                        if (body_mecanim_angles_.ContainsKey(joint_name_))
                        {
                            switch (joint_angle_)
                            {
                                case "abd":
                                    body_mecanim_angles_[joint_name_] = new Vector3((float)positions[idx], body_mecanim_angles_[joint_name_].y, body_mecanim_angles_[joint_name_].z);
                                    break;
                                case "ier":
                                    body_mecanim_angles_[joint_name_] = new Vector3(body_mecanim_angles_[joint_name_].x, (float)positions[idx], body_mecanim_angles_[joint_name_].z);
                                    break;
                                case "fe":
                                    body_mecanim_angles_[joint_name_] = new Vector3(body_mecanim_angles_[joint_name_].x, body_mecanim_angles_[joint_name_].y, (float)positions[idx]);
                                    break;
                                default:
                                    // code block
                                    break;
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing WebSocket message: " + ex.Message);
        }
    }

    void Update()
    {
        if (ws != null)
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                ws.DispatchMessageQueue();
            #endif
        }

        foreach (var key in body_mecanim_bones_.Keys)
        {
            if (body_mecanim_angles_.ContainsKey(key))
            {
                Transform bone_transform = animator_.GetBoneTransform(body_mecanim_bones_[key]);
                abd_ = body_mecanim_angles_[key].x;
                ier_ = body_mecanim_angles_[key].y;
                fe_ = body_mecanim_angles_[key].z;


                Quaternion currentGlobalToLocal = bone_transform.rotation * Quaternion.Inverse(bone_transform.localRotation);
                Quaternion rotationFromNpose = currentGlobalToLocal * Quaternion.Inverse(body_mecanim_n_pose_global2local_[body_mecanim_bones_[key]]);

                bone_transform.localRotation = body_mecanim_n_pose_local_[body_mecanim_bones_[key]];

                separator_position_ = key.IndexOf('_');
                if (separator_position_ != -1)
                {
                    side_ = key.Substring(0, separator_position_);
                    joint_name_ = key.Substring(separator_position_ + 1, key.Length - (separator_position_ + 1));
                    knee_rotation_ = joint_name_ == "knee" ? -1 : 1;
                    if (joint_name_ == "elbow") { //conversion from T-pose to N-pose for the elbow
                        fe_ += fe_off;
                        abd_ += abd_off;
                        ier_ -= 90;
                        ier_ += ier_off;
                    } else if (joint_name_ == "shoulder") { //conversion from T-pose to N-pose for the shoulder
                        fe_ += sh_fe_off;
                        abd_ += sh_adb_off;
                        ier_ += sh_ier_off;
                    }

                    switch (side_)
                    {
                        case "left":
                            bone_transform.Rotate(rotationFromNpose * Vector3.up, ier_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.forward, -abd_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.right, -fe_ * knee_rotation_, Space.World);
                            break;
                        case "right":
                            bone_transform.Rotate(rotationFromNpose * Vector3.up, -ier_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.forward, abd_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.right, -fe_ * knee_rotation_, Space.World);
                            break;

                        case "vertical":
                            bone_transform.Rotate(rotationFromNpose * Vector3.up, ier_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.forward, -abd_, Space.World);
                            bone_transform.Rotate(rotationFromNpose * Vector3.right, fe_, Space.World);
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    bone_transform.Rotate(rotationFromNpose * Vector3.up, ier_, Space.World);
                    bone_transform.Rotate(rotationFromNpose * Vector3.forward, -abd_, Space.World);
                    bone_transform.Rotate(rotationFromNpose * Vector3.right, fe_, Space.World);
                }
            }
        }
    }

    private async void OnDestroy()
    {
        if (ws != null)
        {
            await ws.Close();
        }
    }

    void mapMecanimBones()
    {
        body_mecanim_bones_.Add("vertical_pelvis", HumanBodyBones.Hips);

        body_mecanim_bones_.Add("neck", HumanBodyBones.Neck);
        body_mecanim_bones_.Add("trunk", HumanBodyBones.Spine);
        body_mecanim_bones_.Add("right_hip", HumanBodyBones.RightUpperLeg);
        body_mecanim_bones_.Add("left_hip", HumanBodyBones.LeftUpperLeg);
        body_mecanim_bones_.Add("right_knee", HumanBodyBones.RightLowerLeg);
        body_mecanim_bones_.Add("left_knee", HumanBodyBones.LeftLowerLeg);
        body_mecanim_bones_.Add("right_shoulder", HumanBodyBones.RightUpperArm);
        body_mecanim_bones_.Add("left_shoulder", HumanBodyBones.LeftUpperArm);
        body_mecanim_bones_.Add("right_elbow", HumanBodyBones.RightLowerArm);
        body_mecanim_bones_.Add("left_elbow", HumanBodyBones.LeftLowerArm);
        body_mecanim_bones_.Add("right_wrist", HumanBodyBones.RightHand);
        body_mecanim_bones_.Add("left_wrist", HumanBodyBones.LeftHand);
        body_mecanim_bones_.Add("right_ankle", HumanBodyBones.RightFoot);
        body_mecanim_bones_.Add("left_ankle", HumanBodyBones.LeftFoot);
    }

    void mapMecanimAngles()
    {
        foreach (var key in body_mecanim_bones_.Keys)
        {
            body_mecanim_angles_.Add(key, new Vector3(0, 0, 0));
        }
    }

    void mapMecanimNpose()
    {
        foreach (var key in body_mecanim_bones_.Values)
        {
            Transform bone_transform = animator_.GetBoneTransform(key);
            body_mecanim_n_pose_global2local_.Add(key, bone_transform.rotation * Quaternion.Inverse(bone_transform.localRotation));
            body_mecanim_n_pose_local_.Add(key, bone_transform.localRotation);
        }
    }
}