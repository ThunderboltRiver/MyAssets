using System;
using UnityEngine;
using UnityEngine.TestTools;
using ItemSearchSystem;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using System.Collections;

namespace PlayModeTests
{
    public class SearcherPlayModeTest
    {
        private Scene scene;

        [SetUp]
        public void Setup()
        {
        }


        [UnityTest]
        public IEnumerator SearcherがISearchableなGameObjectを発見できるか()
        {
            scene = SceneManager.CreateScene("New Scene");
            SceneManager.SetActiveScene(scene);
            Searcher searcher = new();
            GameObject target = new();
            _ = target.AddComponent<SearchableTestItem>();
            SphereCollider collider = target.AddComponent<SphereCollider>();
            collider.radius = 0.5f;
            target.transform.position = 2.0f * Vector3.forward;
            searcher.Search(out GameObject gameObject);
            yield return null;
            Assert.That(gameObject.TryGetComponent(out ISearchable _), Is.True);
        }



    }
}