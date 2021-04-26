using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Enemy : NetworkBehaviour
{


    //Partes do Inimigo 
    public GameObject[] cubes;

    //Cor do inimigo com uma variavel sincronizavel atrelada a um metodo de atualizacao visual
    [SyncVar(hook = nameof(UpdateCubeColor))]
    public Color myColor;

    public int life = 3;
    void Start()
    {
        if (isServer)
        {
            //Se for o servidor, gere uma cor aleatoria
            myColor = GenerateColor();
        }



    }


    //Vc nao pode chamar uma função RPC ou CMD se vc nao possui a autoridade disso "if(hasAuthority)", entao criamos o metodo hit para chamar a função RPC, ja que o inimigo
    //permanece rodando no servidor. assim atualizando a vida para os outros clientes.
    public void Hit(int x )
    {
        RpcTakeHit(x);

     
    }



    //RPC é uma mensagem enviada do servidor para os outros clientes, no caso informado q a vida foi diminuida e que é necessario organizar os cubos.
    [ClientRpc]
    public void RpcTakeHit(int x)
    {
        //Remove uma vida
        life -= x;


        //Atualiza o corpo baseando-se na life.
        UpdateCubes();
    }




    //Cria uma cor e retorna
   public Color GenerateColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color c = new Color(r, g, b, 1);
        return c;


    }



    //Metodo de gancho para a mudança da variavel cor
    void UpdateCubeColor(Color old, Color newCol)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<MeshRenderer>().material.color = newCol;
        }
    }

    //Atualiza o corpo de cubos do inimigo.
    void UpdateCubes()
    {

        for (int i = 0; i < cubes.Length; i++)
        {
            if (i < life)
            {
                cubes[i].gameObject.SetActive(true);
            }
            else
            {
                cubes[i].gameObject.SetActive(false);

            }
        }
    }
}
