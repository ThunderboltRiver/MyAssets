using NUnit.Framework;
using UnityEngine;
using ItemSearchSystem;
using UnityEditor.SceneManagement;


namespace EditModeTests
{
    public class SearchSystemManagerUnitTest
    {
        GameObject target;
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            target = new();
            target.transform.position = 2.0f * Vector3.forward;
            SphereCollider collider = target.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = 0.5f;
        }
    }
}