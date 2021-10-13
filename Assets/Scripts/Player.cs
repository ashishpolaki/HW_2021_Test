using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float speed = 8;

    // Update is called once per frame
    public Rigidbody rigid;

    float fallingTimer = 60;
    float tempTimer = 0;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * speed);


        if (GameManager.instance.IsGameOver == false)
        {
            //Check playerCollisions with Pulpits
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
            {
            }
            else
            {
                tempTimer++;
                if (tempTimer == fallingTimer)
                {
                    tempTimer = 0;
                    GameManager.instance.IsGameOver = true;
                }
            }
        }
    }


}
