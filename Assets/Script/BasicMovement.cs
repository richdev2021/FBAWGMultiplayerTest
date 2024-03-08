using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class BasicMovement : NetworkBehaviour
{
    [SyncVar]
    public Color charColor;
    private Renderer capsuleRenderer;

    public Rigidbody2D rb;
    public float jumpcounter, jumpForce =  10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleRenderer = GetComponent<Renderer>();
        charColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        capsuleRenderer.material.SetColor("_Color", charColor);
        CambiarColorServidorRPC(charColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (base.IsOwner == false) return;
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-10, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(10, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, jumpForce, 0) * Time.deltaTime;
            jumpcounter += 1*Time.deltaTime;
            if(jumpForce > 0) jumpForce -= 10 * Time.deltaTime;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 1));
            jumpcounter = 0;
            jumpForce = 10;
        }
    }
    [ServerRpc]//la funcion se ejecuta en el lado del servidor
    void CambiarColorServidorRPC(Color _color)
    {
        CambiarColorRPC(_color);
    }
    [ObserversRpc(RunLocally = true)]
    void CambiarColorRPC(Color _color)
    {
        capsuleRenderer.material.color = _color;
    }
}
