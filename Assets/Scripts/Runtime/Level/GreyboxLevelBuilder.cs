using System.Collections.Generic;
using UnityEngine;
using LastShift.Flow;

namespace LastShift.Level
{
    /// <summary>
    /// Procedural greybox level slice generator.
    /// Builds Office, Duct Loop, Rest Room, Pursuit Corridor, and False Exit per GDD specifications.
    /// Implements LVL-01..LVL-14 and END-01..08 spatial layout.
    /// </summary>
    public class GreyboxLevelBuilder : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private Transform _officePlayerSpawn;
        [SerializeField] private Transform _loopPlayerSpawn;
        [SerializeField] private Transform _pursuitPlayerSpawn;

        [Header("Cinematic Target Transforms")]
        [SerializeField] private Transform _darkMonitorCamPoint;
        [SerializeField] private Transform _idCardCamPoint;
        [SerializeField] private Transform _litMonitorCamPoint;
        [SerializeField] private Transform _slumpedBodyCamPoint;
        [SerializeField] private Transform _newspaperCamPoint;

        [Header("Interactables & Triggers")]
        [SerializeField] private InteractableItem _computerInteractable;
        [SerializeField] private InteractableItem _motherNoteInteractable;
        [SerializeField] private InteractableItem _deadlineNoteInteractable;
        [SerializeField] private InteractableItem _idCardInteractable;
        [SerializeField] private InteractableItem _newspaperInteractable;
        [SerializeField] private InteractableItem _ventEntranceInteractable;
        [SerializeField] private InteractableItem _ventExitInteractable;

        [Header("Environment Props")]
        [SerializeField] private GameObject _fireAlarmEffects;
        [SerializeField] private GameObject _officeBlockedDoor;
        [SerializeField] private GameObject _monsterSilhouette;

        public Transform OfficePlayerSpawn => _officePlayerSpawn;
        public Transform LoopPlayerSpawn => _loopPlayerSpawn;
        public Transform PursuitPlayerSpawn => _pursuitPlayerSpawn;

        public Transform DarkMonitorCamPoint => _darkMonitorCamPoint;
        public Transform IdCardCamPoint => _idCardCamPoint;
        public Transform LitMonitorCamPoint => _litMonitorCamPoint;
        public Transform SlumpedBodyCamPoint => _slumpedBodyCamPoint;
        public Transform NewspaperCamPoint => _newspaperCamPoint;

        public InteractableItem ComputerInteractable => _computerInteractable;
        public InteractableItem MotherNoteInteractable => _motherNoteInteractable;
        public InteractableItem DeadlineNoteInteractable => _deadlineNoteInteractable;
        public InteractableItem IdCardInteractable => _idCardInteractable;
        public InteractableItem NewspaperInteractable => _newspaperInteractable;
        public InteractableItem VentEntranceInteractable => _ventEntranceInteractable;
        public InteractableItem VentExitInteractable => _ventExitInteractable;

        public GameObject FireAlarmEffects => _fireAlarmEffects;
        public GameObject OfficeBlockedDoor => _officeBlockedDoor;
        public GameObject MonsterSilhouette => _monsterSilhouette;

        public void BuildLevelIfEmpty()
        {
            // First attempt to bind already-authored MCP scene objects under Environment
            var env = GameObject.Find("Environment");
            if (env != null)
            {
                BindExistingSceneObjects(env.transform);
                return;
            }

            if (transform.childCount > 0)
            {
                BindExistingSceneObjects(transform);
                return; // Level already generated
            }

            BuildOffice();
            BuildVentSystem();
            BuildMonsterVisual();
        }

        public void BindExistingSceneObjects(Transform root)
        {
            _officePlayerSpawn = root.Find("Office/Spawn_Office");
            _officeBlockedDoor = root.Find("Office/Door_Blocked")?.gameObject;
            _computerInteractable = root.Find("Office/ComputerMonitor")?.GetComponent<InteractableItem>();
            _motherNoteInteractable = root.Find("Office/MotherNote")?.GetComponent<InteractableItem>();
            _deadlineNoteInteractable = root.Find("Office/DeadlineNote")?.GetComponent<InteractableItem>();
            _newspaperInteractable = root.Find("Office/Newspaper")?.GetComponent<InteractableItem>();
            _ventEntranceInteractable = root.Find("Office/VentGrate_Entrance")?.GetComponent<InteractableItem>();

            _loopPlayerSpawn = root.Find("VentSystem/Spawn_Loop");
            _pursuitPlayerSpawn = root.Find("VentSystem/Spawn_Pursuit");
            _idCardInteractable = root.Find("VentSystem/RestRoom/Employee_ID_Card")?.GetComponent<InteractableItem>();
            _ventExitInteractable = root.Find("VentSystem/VentGrate_Exit")?.GetComponent<InteractableItem>();

            _monsterSilhouette = root.Find("Monster_Silhouette")?.gameObject;
        }

        private void BuildOffice()
        {
            var officeRoot = new GameObject("Office_LVL01_02");
            officeRoot.transform.SetParent(transform);

            // Office Dimensions: 10m x 8m x 3.5m
            CreateBox("Floor", officeRoot.transform, new Vector3(0f, -0.1f, 0f), new Vector3(10f, 0.2f, 8f));
            CreateBox("Ceiling", officeRoot.transform, new Vector3(0f, 3.6f, 0f), new Vector3(10f, 0.2f, 8f));
            CreateBox("Wall_North", officeRoot.transform, new Vector3(0f, 1.75f, 4f), new Vector3(10f, 3.5f, 0.2f));
            CreateBox("Wall_South", officeRoot.transform, new Vector3(0f, 1.75f, -4f), new Vector3(10f, 3.5f, 0.2f));
            CreateBox("Wall_East", officeRoot.transform, new Vector3(5f, 1.75f, 0f), new Vector3(0.2f, 3.5f, 8f));
            CreateBox("Wall_West", officeRoot.transform, new Vector3(-5f, 1.75f, 0f), new Vector3(0.2f, 3.5f, 8f));

            // Office Desk
            var desk = CreateBox("Desk", officeRoot.transform, new Vector3(0f, 0.4f, 2f), new Vector3(2.4f, 0.8f, 1.2f));

            // Computer Monitor
            var monitor = CreateBox("ComputerMonitor", officeRoot.transform, new Vector3(0f, 1.1f, 2.3f), new Vector3(0.8f, 0.6f, 0.1f));
            _computerInteractable = monitor.AddComponent<InteractableItem>();

            // Mother's Note (NAR-ITEM-02)
            var note1 = CreateBox("MotherNote", officeRoot.transform, new Vector3(-0.6f, 0.85f, 2.0f), new Vector3(0.25f, 0.02f, 0.35f));
            _motherNoteInteractable = note1.AddComponent<InteractableItem>();

            // Deadline Note (NAR-ITEM-03)
            var note2 = CreateBox("DeadlineNote", officeRoot.transform, new Vector3(0.6f, 0.85f, 2.0f), new Vector3(0.25f, 0.02f, 0.35f));
            _deadlineNoteInteractable = note2.AddComponent<InteractableItem>();

            // Newspaper (NAR-04)
            var news = CreateBox("Newspaper", officeRoot.transform, new Vector3(0.9f, 0.85f, 1.8f), new Vector3(0.4f, 0.02f, 0.5f));
            _newspaperInteractable = news.AddComponent<InteractableItem>();

            // Fire Alarm Strobe Light (LVL-02)
            _fireAlarmEffects = new GameObject("FireAlarm_Strobe");
            _fireAlarmEffects.transform.SetParent(officeRoot.transform);
            _fireAlarmEffects.transform.position = new Vector3(0f, 3.2f, -3.8f);
            var light = _fireAlarmEffects.AddComponent<Light>();
            light.color = Color.red;
            light.intensity = 2f;
            light.range = 10f;
            _fireAlarmEffects.SetActive(false);

            // Office Main Door (Blocked in LVL-02)
            _officeBlockedDoor = CreateBox("Door_Blocked", officeRoot.transform, new Vector3(0f, 1.2f, -3.9f), new Vector3(1.6f, 2.4f, 0.2f));
            _officeBlockedDoor.SetActive(false);

            // Vent Entrance Grate (LVL-02 -> LVL-03)
            var ventGrate = CreateBox("VentGrate_Entrance", officeRoot.transform, new Vector3(-4.85f, 0.6f, 0f), new Vector3(0.2f, 1.0f, 1.0f));
            _ventEntranceInteractable = ventGrate.AddComponent<InteractableItem>();

            // Spawn Transform
            var spawnGO = new GameObject("Spawn_Office");
            spawnGO.transform.SetParent(officeRoot.transform);
            spawnGO.transform.position = new Vector3(0f, 0.1f, 0f);
            spawnGO.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _officePlayerSpawn = spawnGO.transform;

            // Camera target points for Ending Sequence (END-01..08)
            _darkMonitorCamPoint = CreatePoint("Cam_DarkMonitor", officeRoot.transform, new Vector3(0f, 1.2f, 1.6f), Quaternion.Euler(10f, 0f, 0f));
            _idCardCamPoint = CreatePoint("Cam_IdCard", officeRoot.transform, new Vector3(-0.4f, 1.0f, 1.9f), Quaternion.Euler(45f, 0f, 0f));
            _litMonitorCamPoint = CreatePoint("Cam_LitMonitor", officeRoot.transform, new Vector3(0f, 1.2f, 1.5f), Quaternion.Euler(5f, 0f, 0f));
            _slumpedBodyCamPoint = CreatePoint("Cam_SlumpedBody", officeRoot.transform, new Vector3(1.5f, 1.1f, 1.8f), Quaternion.Euler(15f, -60f, 0f));
            _newspaperCamPoint = CreatePoint("Cam_Newspaper", officeRoot.transform, new Vector3(0.9f, 1.1f, 1.6f), Quaternion.Euler(50f, 0f, 0f));
        }

        private void BuildVentSystem()
        {
            var ventRoot = new GameObject("Vents_LVL03_04");
            ventRoot.transform.SetParent(transform);

            // Duct height is 1.2m (player must crouch: PLR-01)
            // Entrance corridor leading West from office
            CreateBox("Duct_Floor", ventRoot.transform, new Vector3(-10f, 0f, 0f), new Vector3(10f, 0.1f, 1.4f));
            CreateBox("Duct_Ceiling", ventRoot.transform, new Vector3(-10f, 1.3f, 0f), new Vector3(10f, 0.1f, 1.4f));
            CreateBox("Duct_Wall_L", ventRoot.transform, new Vector3(-10f, 0.65f, 0.7f), new Vector3(10f, 1.2f, 0.1f));
            CreateBox("Duct_Wall_R", ventRoot.transform, new Vector3(-10f, 0.65f, -0.7f), new Vector3(10f, 1.2f, 0.1f));

            // Spawn for Loop
            var loopSpawnGO = new GameObject("Spawn_Loop");
            loopSpawnGO.transform.SetParent(ventRoot.transform);
            loopSpawnGO.transform.position = new Vector3(-6f, 0.1f, 0f);
            loopSpawnGO.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            _loopPlayerSpawn = loopSpawnGO.transform;

            // Rest Room (LVL-11..13): Wider junction box with calm lighting
            var restRoom = new GameObject("RestRoom_LVL11_13");
            restRoom.transform.SetParent(ventRoot.transform);
            restRoom.transform.position = new Vector3(-16f, 0f, 0f);

            CreateBox("Rest_Floor", restRoom.transform, Vector3.zero, new Vector3(4f, 0.1f, 4f));
            CreateBox("Rest_Ceiling", restRoom.transform, new Vector3(0f, 1.5f, 0f), new Vector3(4f, 0.1f, 4f));

            // Employee ID Card in rest room (LVL-13)
            var idCard = CreateBox("Employee_ID_Card", restRoom.transform, new Vector3(0f, 0.1f, 0.5f), new Vector3(0.2f, 0.02f, 0.3f));
            _idCardInteractable = idCard.AddComponent<InteractableItem>();

            // Pursuit Corridor (LVL-04) branching West
            var pursuitSpawnGO = new GameObject("Spawn_Pursuit");
            pursuitSpawnGO.transform.SetParent(ventRoot.transform);
            pursuitSpawnGO.transform.position = new Vector3(-19f, 0.1f, 0f);
            pursuitSpawnGO.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            _pursuitPlayerSpawn = pursuitSpawnGO.transform;

            // Exit Grate (LVL-05 / END-01)
            var exitGrate = CreateBox("VentGrate_Exit", ventRoot.transform, new Vector3(-25f, 0.6f, 0f), new Vector3(0.2f, 1.0f, 1.0f));
            _ventExitInteractable = exitGrate.AddComponent<InteractableItem>();
        }

        private void BuildMonsterVisual()
        {
            _monsterSilhouette = new GameObject("Monster_Silhouette_THR01");
            _monsterSilhouette.transform.SetParent(transform);
            _monsterSilhouette.transform.position = new Vector3(-12f, 0.6f, 0f);

            // Monster silhouette geometry: dark distorted shape
            var body = CreateBox("Monster_Body", _monsterSilhouette.transform, Vector3.zero, new Vector3(0.6f, 1.1f, 0.6f));
            var head = CreateBox("Monster_Head", _monsterSilhouette.transform, new Vector3(0f, 0.7f, 0f), new Vector3(0.4f, 0.4f, 0.4f));

            _monsterSilhouette.SetActive(false);
        }

        private GameObject CreateBox(string name, Transform parent, Vector3 localPos, Vector3 size)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent);
            go.transform.localPosition = localPos;
            go.transform.localScale = size;
            return go;
        }

        private Transform CreatePoint(string name, Transform parent, Vector3 localPos, Quaternion localRot)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localPosition = localPos;
            go.transform.localRotation = localRot;
            return go.transform;
        }
    }
}
