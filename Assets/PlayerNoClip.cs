using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerNoClip : NetworkBehaviour
{
    public GameObject playerModel;
    public Vector2 keyboard;
    public Vector2 mouse;
    public Camera cam;
    public float x, y;

    [SyncVar(hook = nameof(UpdateCubeColor))]
    public Color myColor;

    void Start()
    {
      
        cam.enabled = isLocalPlayer;
        playerModel.gameObject.SetActive(!isLocalPlayer);


        if (isLocalPlayer)
        {
            
            CmdUpdateColor(GenerateColor());

        }
       


    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {


            keyboard.x = Input.GetAxis("Horizontal");
            keyboard.y = Input.GetAxis("Vertical");

            mouse.x = Input.GetAxis("Mouse X");
            mouse.y = Input.GetAxis("Mouse Y");


            if (Input.GetKeyDown(KeyCode.Space))
            {
                Color c = GenerateColor();
                CmdUpdateColor(c);
      
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        CmdHitEnemy(hit.transform.gameObject.GetComponentInParent<Enemy>());
                    }
                }

            }
           



        }
    }

    [Command]
    public void CmdHitEnemy(Enemy en) {
        en.Hit(1);
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            transform.Translate(new Vector3(keyboard.x, 0, keyboard.y) / 2);

            x += mouse.x;
            y += mouse.y;

            y = Mathf.Clamp(y,-80, 90);

            transform.eulerAngles = new Vector3( -y, x, 0);
          
    
        }
    }

     void UpdateCubeColor(Color old,Color newCol)
    {
        playerModel.GetComponent<MeshRenderer>().material.color = newCol;
    }


    [Command]
    public void CmdUpdateColor(Color c)
    {
        myColor = c;


    }

    Color GenerateColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color c = new Color(r, g, b, 1);
        return c;


    }
}
