using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public event System.Action<int> OnNewWave;

    public bool developerMode;

    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float timeBetweenCampingchecks = 2;
    float campthresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisabled;

    void Start() {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingchecks + Time.time;
        campPositionOld = playerT.position;

        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    void Update() {
        if (!isDisabled) {
            if (Time.time > nextCampCheckTime) {
                nextCampCheckTime = Time.time + timeBetweenCampingchecks;
                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campthresholdDistance);
                campPositionOld = playerT.position;
            }

            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime) {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine("SpawnEnemy");
            }
        }

        if (developerMode) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StopCoroutine("SwpanEnemy");
                foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                    Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }


    IEnumerator SpawnEnemy() {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;
        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping) {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMaterial = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tileMaterial.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay) {
            tileMaterial.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;

            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
    }

    void OnPlayerDeath() {
        isDisabled = true;
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0) {
            NextWave();
        }
    }

    void ResetPlayerPosition() {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NextWave() {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length) {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null) {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;
        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColor;
    }

}
