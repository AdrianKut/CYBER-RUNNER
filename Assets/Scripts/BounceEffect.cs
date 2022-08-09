using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{

    private void Start() => Bounce();
    private void OnEnable() => Bounce();

    private void Bounce() => LeanTween.moveY(this.gameObject, -3, 2).setEaseInOutCubic().setLoopPingPong();
}
