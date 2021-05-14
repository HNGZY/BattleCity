using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jiazai : MonoBehaviour
{
    public Slider slider;
    AsyncOperation operation;
    float index;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Jia());
    }
    IEnumerator Jia()
    {
        operation = SceneManager.LoadSceneAsync(2);
        operation.allowSceneActivation = false;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        if(operation==null)
        {
            return;
        }
        else if(operation.progress<0.9f)
        {
            index = operation.progress;
        }
        else
        {
            index = 1;
        }
        slider.value = Mathf.MoveTowards(slider.value, index, Time.deltaTime * 0.5f);
        if(slider.value==1)
        {
            operation.allowSceneActivation = true;
        }
    }
}
