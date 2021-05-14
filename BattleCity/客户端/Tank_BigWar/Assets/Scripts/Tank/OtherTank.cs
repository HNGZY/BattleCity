using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTank : MonoBehaviour
{
    public GameObject Paotai;
    public void Move_Pos(Vector3 dir)
    {
        transform.position = dir;
    }
    public void Move_Rot(Vector3 dir)
    {
        transform.rotation = Quaternion.Euler(dir);
    }
    public void Move_PaoT(Vector3 dir)
    {
        Paotai.transform.rotation = Quaternion.Euler(dir);
    }
}
