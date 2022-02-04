using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Utilites : MonoBehaviour
{
    public static Vector2 GetMousePosition() => Mouse.current.position.ReadValue();
}
