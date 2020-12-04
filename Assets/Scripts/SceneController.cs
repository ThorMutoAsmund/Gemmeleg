using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemmeleg
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject spawnPoint = null;
        [SerializeField] private GameObject spawnPoint2 = null;
        [SerializeField] private GameObject spawnPoint3 = null;
        [SerializeField] private GameObject spawnPoint4 = null;


        private void Awake()
        {
            var sceneCamera = this.GetComponent<Camera>();
            sceneCamera.enabled = true;

            // Disable cameras
            foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (var camera in rootObject.GetComponentsInChildren<Camera>())
                {
                    if (camera.gameObject != null && camera != sceneCamera)
                    {
                        GameObject.Destroy(camera.gameObject);
                    }
                }
            }

        }

        private void Start()
        {
            IEnumerator SpawnPlayers()
            {
                yield return new WaitForSeconds(2f);
                SpawnPlayer(true, this.spawnPoint, 1);
                SpawnPlayer(false, this.spawnPoint2, 2);
                SpawnPlayer(false, this.spawnPoint3, 3);
                SpawnPlayer(false, this.spawnPoint4, 4);
                this.GetComponent<Camera>().enabled = false;
            }

            StartCoroutine(SpawnPlayers());
        }

        private void SpawnPlayer(bool isMain, GameObject spawnPoint, int playerNo)
        {
            var playerGameObject = GameObject.Instantiate(this.playerPrefab, null);
            playerGameObject.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);

            if (isMain)
            {
                playerGameObject.AddComponent<PlayerMovement>();
                playerGameObject.AddComponent<PlayerInteraction>();
            }

            var cameraGameObject = playerGameObject.GetComponentInChildren<Camera>().gameObject;
            cameraGameObject.SetActive(isMain);
            var photonView = playerGameObject.GetComponentInChildren<PhotonView>();
            photonView.ViewID = playerNo * 1000;

            this.sceneCameras.Add(cameraGameObject);
        }

        private List<GameObject> sceneCameras = new List<GameObject>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                ToggleCamera();
            }
        }

        private void ToggleCamera()
        {
            for (int i = 0; i < this.sceneCameras.Count; ++i)
            {
                if (this.sceneCameras[i].activeSelf)
                {
                    this.sceneCameras[i].SetActive(false);
                    this.sceneCameras[(i + 1) % this.sceneCameras.Count].SetActive(true);
                    return;
                }
            }

            this.sceneCameras[0].SetActive(true);
        }
    }
}