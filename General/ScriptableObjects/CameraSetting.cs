using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CameraSetting", menuName = "CreateCameraSetting")]
public class CameraSetting : ScriptableObject
{

    [SerializeField] private float _cameraSensitivity;
    public float CameraSensitivity => _cameraSensitivity;

}