#define TEST_THIS_LIBRARY
using System;
using UnityEngine;

namespace InputableActor
{
#if TEST_THIS_LIBRARY

    [Serializable]
    public class TestHandler : InputHandler<float>, ISettingLoadable
    {
        [SerializeField] private String message = "";

        public void LoadSetting(String setting)
        {
            message = setting;
        }

        protected override void OnUpdate(float value)
        {
            Debug.Log($"{message}:{value}");
        }

        ~TestHandler()
        {
            Debug.Log("TestHandler is Delete");
        }
    }

    public class TestActor : Actor<int, float>
    {
        [SerializeField] TestHandler handler = new();
        public void Start()
        {
            AddInputHandler(0, handler);
            Debug.Log("This is Test");
        }
        protected override void OnUpdate(int key, float value)
        {
            //AcceptInput(0, Time.deltaTime);
        }
        public void onDestroy()
        {
            Destroy();
            Debug.Log("Called At TestActor");
        }
    }
#endif
}

