using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : MonoBehaviour
{
    [Header("Signals")]
    [Tooltip("Sinal disparado quando a fruta atinge o chão. Não têm parametros.")]
    public Signal OnGroundHit;
    [Tooltip("Sinal disparado quando a fruta é coletado ou estraga. O parametro se refere a própria fruta.")]
    public Signal<Fruit> OnDespawn;
    [Space]
    [SerializeField] private int fruitValue;
    
    protected Rigidbody body;


    protected virtual void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Para a fruta assim que ela toca no chão.
        body.Sleep();
        OnGroundHit.Fire();
    }

    /// <summary>
    /// Desativa e retorna a fruta para o bonsai. Caso tenha sido coletada, o jogador recebe o valor da
    /// fruta.
    /// </summary>
    /// <param name="gathered">Se a fruta foi coletada.</param>
    public virtual void RemoveFruit(bool gathered)
    {
        if (gathered)
        {
            GameMaster.GetManager<InventoryManager>().GainFruits(fruitValue);
        }

        OnDespawn.Fire(this);
        body.WakeUp();
    }
}
