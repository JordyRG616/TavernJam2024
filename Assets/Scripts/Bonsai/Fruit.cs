using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : MonoBehaviour
{
    [Header("Signals")]
    [Tooltip("Sinal disparado quando a fruta atinge o ch�o. N�o t�m parametros.")]
    public Signal OnGroundHit;
    [Tooltip("Sinal disparado quando a fruta � coletado ou estraga. O parametro se refere a pr�pria fruta.")]
    public Signal<Fruit> OnDespawn;
    public Signal OnSpawn;
    [Space]
    [SerializeField] private int fruitValue;
    [SerializeField] private float decayTime;

    protected Rigidbody body;
    protected WaitForSeconds waitToDecay;

    protected virtual void Start()
    {
        body = GetComponent<Rigidbody>();
        waitToDecay = new WaitForSeconds(decayTime);
    }

    private void OnEnable()
    {
        OnSpawn.Fire();
    }

    private IEnumerator Decay()
    {
        yield return waitToDecay;

        RemoveFruit(false);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Para a fruta assim que ela toca no ch�o.
        body.Sleep();
        OnGroundHit.Fire();
        StartCoroutine(Decay());
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

        StopAllCoroutines();
        OnDespawn.Fire(this);
        body.WakeUp();
    }
}
