using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : AnimalMove
{
    public float speed = 3f;

    public void Move(Animals animal)
    {
        //animal.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
