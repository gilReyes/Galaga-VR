using System.Collections;
using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Flyer
{
    // This script handles getting the laser instances from
    // the object pool and firing them.
    public class EnemyLaserController : MonoBehaviour
    {
        [SerializeField] private FlyerGameController m_GameController;  // Reference to the game controller so firing can be limited to when the game is running.
        [SerializeField] private ObjectPool m_LaserObjectPool;          // Reference to the object pool the lasers belong to.
        [SerializeField] private float m_LaserSpawnRate = 0.2f;         // Rate of fire for lasers
        private AudioSource m_LaserAudio;
        private Transform m_LaserSpawnPos;                              // The positions the lasers should spawn from.// The audio source that should play firing sounds.
        private bool m_Spawning;

        private void Start()
        {
            // If the game isn't running return.
            if (!m_GameController.IsGameRunning)
                return;

            m_LaserSpawnPos = transform.Find("Space_Invader 1/EnemyShipLaserCenter");
            // Fire laser from each position.
            StartCoroutine(SpawnLaserRoutine());
            m_Spawning = true;
        }


        private void SpawnLaser(Transform gunPos)
        {
            // Get a laser from the pool.
            GameObject laserGameObject = m_LaserObjectPool.GetGameObjectFromPool();

            // Set it's position and rotation based on the gun positon.
            laserGameObject.transform.position = gunPos.position;
            laserGameObject.transform.rotation = gunPos.rotation;

            // Find the FlyerLaser component of the laser instance.
            FlyerLaser flyerLaser = laserGameObject.GetComponent<FlyerLaser>();

            // Set it's object pool so it knows where to return to.
            flyerLaser.ObjectPool = m_LaserObjectPool;

            // Restart the laser.
            flyerLaser.Restart();

            // Play laser audio.
            m_LaserAudio.Play();
        }

        private IEnumerator SpawnLaserRoutine()
        {
            // With an initial delay, spawn a laser and delay whilst the environment is spawning.
            yield return new WaitForSeconds(m_LaserSpawnRate);
            do
            {
                SpawnLaser(m_LaserSpawnPos);
                yield return new WaitForSeconds(m_LaserSpawnRate);
            }
            while (m_Spawning);
        }
    }
}