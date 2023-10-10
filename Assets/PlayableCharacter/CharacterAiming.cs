using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on code from TheKiwiCoder: https://www.youtube.com/watch?v=_I8HsTfKep8&t=1s
//Standard assets for animation
//Mixamo for Pistol idle animation
//POLYGON starter pack https://assetstore.unity.com/packages/3d/props/polygon-starter-pack-low-poly-3d-art-by-synty-156819?aid=1011ljjCh&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker
public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get camera's y-axis rotational value and gradually rotate player's transform towards the same y-axis value
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
