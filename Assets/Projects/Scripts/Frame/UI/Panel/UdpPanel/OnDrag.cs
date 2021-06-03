using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrag : MonoBehaviour
{
    public void Ondrag()
    {
        transform.position = Input.mousePosition;
    }
}
