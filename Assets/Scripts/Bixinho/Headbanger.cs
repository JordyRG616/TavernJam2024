using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbanger : BixinhoBase
{
    [SerializeField] private List<int> fruitsPerLevel;
    [SerializeField] private float retreatDistance;
    [Space]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float retreatSpeed;

    private Bonsai bonsai;
    private int fruitAmount;


    protected override void Start()
    {
        base.Start();

        bonsai = GameMaster.GetManager<Bonsai>();
        fruitAmount = fruitsPerLevel[0];
    }

    protected override void Activate()
    {
        // Muda a velocidade máxima do bixinho durante a investida.
        NavigationAgent.speed = chargeSpeed;
        NavigationAgent.SetDestination(bonsai.transform.position);
    }

    public override void LevelUp()
    {
        base.LevelUp();

        fruitAmount = fruitsPerLevel[Level];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.tag == "Bonsai" && activated)
        {
            // Derruba a quantidade correspondente de frutas e define o bixinho como inativo (necessário
            // para que ele se mantenha no loop de fazer a ação).
            bonsai.SpawnFruits(fruitAmount);
            activated = false;

            // Muda a velocidade máxima do bixinho de volta para a velocidade de caminhada e define a 
            // direção para onde ele vai recuar.
            NavigationAgent.speed = retreatSpeed;

            var direction = transform.position.normalized * retreatDistance;

            Vector3 rdm = Random.insideUnitCircle.normalized;
            rdm = new Vector3(rdm.x, 0, rdm.y);
            direction += rdm;

            NavigationAgent.SetDestination(direction);
        }
    }
}
