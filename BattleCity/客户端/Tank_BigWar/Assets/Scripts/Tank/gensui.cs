using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gensui : MonoBehaviour
{
    public GameObject game;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = transform.position - game.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, game.transform.position + dir, Time.deltaTime * 5);
    }
}
