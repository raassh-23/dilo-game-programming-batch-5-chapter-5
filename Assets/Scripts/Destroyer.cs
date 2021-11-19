using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        String tag = other.gameObject.tag;
        if (tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
        {
            Destroy(other.gameObject);
        }
    }
}
