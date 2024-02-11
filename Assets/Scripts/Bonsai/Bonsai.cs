using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonsai : ManagerBehaviour
{
    [Header("Signals")]
    [Tooltip("Sinal disparado quando o bonsai recebe fertilizante. O parametro se refere à porcentagem de fertilizante em relação ao requerido para o próximo level")]
    public Signal<float> OnFertilization;
    [Tooltip("Sinal disparado quando o bonsai upa de level. O parametro se refere ao level atual do bonsai (depois de upar)")]
    public Signal<int> OnLevelUp;
    [Tooltip("Sinal disparado quando o bonsai chaga no nível máximo. Não passa parametros")]
    public Signal OnFinalPhaseEntered;
    [Tooltip("Sinal disparado quando o jogador vence o jogo. Não passa parametros")]
    public Signal OnVictory;
    [Space]

    [SerializeField] private List<Fruit> fruitsByLevel;
    [SerializeField] private Transform fruitYeetPoint;
    [SerializeField] private Vector2 fruitYeetForceRange;
    [Space]

    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnIntervalLimit;
    [Space]

    [SerializeField] private int initialFertilizationRequired;
    [SerializeField] private int finalFertilizationRequired;

    /// <summary>
    /// Lista de frutas no chão que não estão no processo de ser coletadas por um cãozinho.
    /// </summary>
    public static List<Fruit> FruitsOnTheGround { get; private set; } = new List<Fruit>();

    /// <summary>
    /// Pool de frutas inativas (coletadas) para serem ativadas durante o spawn.
    /// </summary>
    private Queue<Fruit> inactiveFruits = new Queue<Fruit>();

    /// <summary>
    /// Pool de possível frutas para se spawnar.
    /// </summary>
    private List<Fruit> possibleFruits = new List<Fruit>();

    public int Level { get; private set; } = 0;
    private int maxLevel = 3;
    private int currentFertilizationRequired;
    private int _fertilization;
    public int CurrentFertilization
    {
        get => _fertilization;
        set
        {
            if (!onFinalPhase)
            {
                _fertilization = Mathf.Clamp(value, 0, currentFertilizationRequired);

                OnFertilization.Fire(_fertilization / (float)currentFertilizationRequired);

                if (_fertilization == currentFertilizationRequired)
                {
                    LevelUp();
                }
            } else
            {
                finalFertilization = value;

                OnFertilization.Fire(finalFertilization / (float)finalFertilizationRequired);

                if (finalFertilization >= finalFertilizationRequired)
                {
                    Victory();
                }
            }
        }
    }

    private int finalFertilization;

    private float _spawnInterval;
    private float currentSpawnInterval
    {
        get => _spawnInterval;
        set
        {
            _spawnInterval = Mathf.Clamp(value, spawnIntervalLimit, float.MaxValue);
        }
    }

    private bool onFinalPhase;
    private bool active = true;
    private WaitForSeconds waitSpawnInterval;


    private void Start()
    {
        currentSpawnInterval = spawnInterval;
        currentFertilizationRequired = initialFertilizationRequired;
        waitSpawnInterval = new WaitForSeconds(currentSpawnInterval);
        possibleFruits.Add(fruitsByLevel[0]);

        StartCoroutine(ManageFruitSpawn());
    }

    /// <summary>
    /// Atualiza o intervalo entre o spawn de cada fruta.
    /// </summary>
    /// <param name="value">valor pelo qual o intervalo atual será modificado.</param>
    public void ChangeSpawnInterval(float value)
    {
        currentSpawnInterval += value;
        waitSpawnInterval = new WaitForSeconds(currentSpawnInterval);
    }

    /// <summary>
    /// Gerencia o spawn de frutas de acordo com o intervalo de ativação atual do bonsai.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageFruitSpawn()
    {
        while (active)
        {
            SpawnFruits(1);

            yield return waitSpawnInterval;
        }
    }

    /// <summary>
    /// Cria e lança novas frutas no mapa.
    /// </summary>
    /// <param name="amount">Quantidade de frutas a se criar.</param>
    public void SpawnFruits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Fruit fruit = null;

            // Checa se existem frutas inativas na fila. Cria uma nova fruta e registra os callbacks
            // necessário caso a fila esteja vazia, ou seleciona a próxima fruta na fila e ativa esta
            // caso contrário.
            if (inactiveFruits.Count == 0)
            {
                var model = possibleFruits[Random.Range(0, possibleFruits.Count)];
                fruit = Instantiate(model, fruitYeetPoint.position, Quaternion.identity);
                fruit.OnGroundHit += () => FruitsOnTheGround.Add(fruit);
                fruit.OnDespawn += ReturnFruit;
            }
            else
            {
                fruit = inactiveFruits.Dequeue();
                fruit.gameObject.SetActive(true);
            }

            // Calcula a direção do arremesso e a uma força aleatória dentro dos limites estabelecidos.
            var fruitBody = fruit.GetComponent<Rigidbody>();

            var angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) / 2;
            direction += Vector3.up;

            var force = Random.Range(fruitYeetForceRange.x, fruitYeetForceRange.y);

            fruitBody.AddForce(direction * force);
        }
    }

    /// <summary>
    /// Retorna uma fruta que tenha sido coletada (ou potencialmente tenha estragado) para a fila de
    /// frutas inativas e a remove da lista de frutas no chão caso necessário.
    /// </summary>
    /// <param name="fruit"></param>
    private void ReturnFruit(Fruit fruit)
    {
        if (FruitsOnTheGround.Contains(fruit))
        {
            FruitsOnTheGround.Remove(fruit);
        }

        fruit.gameObject.SetActive(false);
        fruit.transform.position = fruitYeetPoint.position;
        inactiveFruits.Enqueue(fruit);
    }

    /// <summary>
    /// Atualiza o level, fertilização atual e fertilização necessária para o próximo level, além de
    /// aplicar os efeitos do novo level.
    /// </summary>
    private void LevelUp()
    {
        if (Level == maxLevel) return;

        Level++;
        CurrentFertilization = 0;
        currentFertilizationRequired += currentFertilizationRequired - 1;

        OnLevelUp.Fire(Level);

        if (Level == maxLevel)
        {
            onFinalPhase = true;
            OnFinalPhaseEntered.Fire();
            return;
        }
        possibleFruits.Add(fruitsByLevel[Level]);
    }

    private void Victory()
    {
        OnVictory.Fire();

        // IMPLEMENTAR FIM DE JOGO AQUI
    }
}
