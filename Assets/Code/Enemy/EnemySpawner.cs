using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxSpawned;
    [SerializeField] private int _spawnPerSecond;
    private GameObject _enemyObject;
    private List<Enemy> _spawnedEnemies;

    private float spawnTimerSecondsRemaining = 0f;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimerSecondsRemaining = _spawnPerSecond;
        _enemyObject = Resources.Load<GameObject>("Enemies/BasicEnemy 2");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount < _maxSpawned)
        {
            spawnTimerSecondsRemaining -= Time.deltaTime;
            if (spawnTimerSecondsRemaining <= 0f)
            {
                Instantiate(_enemyObject, new Vector3(this.transform.position.x + GetModifier(), this.transform.position.y + GetModifier()), Quaternion.identity, this.transform);
                spawnTimerSecondsRemaining = _spawnPerSecond;
            }
        }
    }

    private float GetModifier()
    {
        float modifier = Random.Range(0f, 1f);
        if (Random.Range(0, 2) > 0) return -modifier;
        return modifier;
    }
}
