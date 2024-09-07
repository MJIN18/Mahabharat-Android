using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public GameObject playerPrefab; // Assign your player character prefab in the Inspector
    public Transform spawnPoint; // Set the spawn point where you want to place the player
    GameObject pl;

    public bool metaScene;

    void Start()
    {
        if(metaScene)
            SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            // Instantiate the player prefab at the spawn point
            //GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            GameManager manager = GameManager.instance;
            if(manager != null)
            {
                if(metaScene)
                    manager.UseArmor(GameManager.instance.selectedArmor, ArmorType.METAVERSE, spawnPoint, ref pl);
                else
                    manager.UseArmorChild(GameManager.instance.selectedArmor, ArmorType.GAMEPLAY, spawnPoint, ref pl);
            }

            // Optionally, you can do additional setup for the player here
        }
        else
        {
            Debug.LogError("Player prefab or spawn point not assigned in the inspector.");
        }
    }
}
