using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zidan : MonoBehaviour
{
    public GameObject game;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "子弹";
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 30);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject xiaoguo = Instantiate(game);
        xiaoguo.transform.position = transform.position;


    }
}
