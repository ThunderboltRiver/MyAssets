using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicked(){
      Debug.Log("clicked");
      SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
