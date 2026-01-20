using UnityEngine;

public class wildwest : MonoBehaviour
{
    public GameObject[] trees;
    public GameObject[] cacti;
    public GameObject[] bees;
    public GameObject[] eki;

    [Range(0f, 1f)]
    [SerializeField] private float beeChance = 0.25f;

    [Range(0f, 1f)]
    [SerializeField] private float ekiChance = 0.1f;

    // Prevent eki from spawning twice in a row
    private static bool ekiSpawnedLastTime = false;

    private void Start()
    {
        // ===== EKI CHECK FIRST =====
        bool spawnEki = !ekiSpawnedLastTime && Random.value < ekiChance;

        if (spawnEki)
        {
            SetActive(eki, true);
            SetActive(trees, false);
            SetActive(cacti, false);
            SetActive(bees, false);

            ekiSpawnedLastTime = true;
            return;
        }

        // If eki did not spawn
        ekiSpawnedLastTime = false;
        SetActive(eki, false);

        // ===== NORMAL LOGIC =====
        bool spawnBees = Random.value < beeChance;

        if (!spawnBees)
        {
            /*
             * 0 = only cacti
             * 1 = only trees
             * 2 = trees + cacti
             */
            int choice = Random.Range(0, 3);

            switch (choice)
            {
                case 0:
                    SetActive(trees, false);
                    SetActive(cacti, true);
                    SetActive(bees, false);
                    break;

                case 1:
                    SetActive(trees, true);
                    SetActive(cacti, false);
                    SetActive(bees, false);
                    break;

                case 2:
                    SetActive(trees, true);
                    SetActive(cacti, true);
                    SetActive(bees, false);
                    break;
            }
        }
        else
        {
            /*
             * 0 = only bees
             * 1 = cacti + bees
             * 2 = trees + bees
             * 3 = trees + cacti + bees
             */
            int choice = Random.Range(0, 4);

            switch (choice)
            {
                case 0:
                    SetActive(trees, false);
                    SetActive(cacti, false);
                    SetActive(bees, true);
                    break;

                case 1:
                    SetActive(trees, false);
                    SetActive(cacti, true);
                    SetActive(bees, true);
                    break;

                case 2:
                    SetActive(trees, true);
                    SetActive(cacti, false);
                    SetActive(bees, true);
                    break;

                case 3:
                    SetActive(trees, true);
                    SetActive(cacti, true);
                    SetActive(bees, true);
                    break;
            }
        }
    }

    private void SetActive(GameObject[] objects, bool state)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}