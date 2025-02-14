using SRD.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRD.Utils
{
    internal class SRDMultiDisplayController : MonoBehaviour
    {
        SRDManager _mainManager;
        List<SRDManager> _srdManagers;

        bool _wallmountModeCache;

        SRDProjectSettings.MultiSRDMode _multiDisplayMode;
        int _numberOfDevices;

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
        }

#if UNITY_EDITOR
        Coroutine SwitchSRDManagerPositionCoroutine;
#endif

        public static readonly float SR2PanelWidth = 0.596736f;
        public static readonly float SR2PanelHeight = 0.335664f;
        public static readonly float SR2BodyWidth = SR2PanelWidth + 0.027f;
        public static readonly float SR2BodyHeight = SR2PanelHeight + 0.083f;

        public static readonly Dictionary<SRDProjectSettings.MultiSRDMode, Vector3[]> SRDManagerPositions = new Dictionary<SRDProjectSettings.MultiSRDMode, Vector3[]>
        {
            {
                SRDProjectSettings.MultiSRDMode.SingleDisplay, new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                }
            },
            {
                SRDProjectSettings.MultiSRDMode.MultiHorizontal, new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(SR2BodyWidth, 0, 0),
                    new Vector3(-(SR2BodyWidth), 0, 0)
                }
            },
            {
                SRDProjectSettings.MultiSRDMode.MultiVertical, new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, -SR2BodyHeight * Mathf.Cos(Mathf.PI / 4), -SR2BodyHeight * Mathf.Sin(Mathf.PI / 4)),
                    new Vector3(0, SR2BodyHeight * Mathf.Cos(Mathf.PI / 4), SR2BodyHeight * Mathf.Sin(Mathf.PI / 4)),
                    new Vector3(0, -SR2BodyHeight * Mathf.Cos(Mathf.PI / 4) * 2, -SR2BodyHeight * Mathf.Sin(Mathf.PI / 4) * 2)
                }
            },
            {
                SRDProjectSettings.MultiSRDMode.MultiGrid, new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(SR2BodyWidth, 0, 0),
                    new Vector3(0, -SR2BodyHeight * Mathf.Cos(Mathf.PI / 4), -SR2BodyHeight * Mathf.Sin(Mathf.PI / 4)),
                    new Vector3(SR2BodyWidth, -SR2BodyHeight * Mathf.Cos(Mathf.PI / 4), -SR2BodyHeight * Mathf.Sin(Mathf.PI / 4))
                }
            },
        };

        private void Awake()
        {
            _srdManagers = new List<SRDManager>();
            _currentIndex = 0;
        }

        private void Start()
        {
            if (_mainManager == null)
            {
                _mainManager = SRDSceneEnvironment.GetSRDManager();
            }
            _srdManagers.Add(_mainManager);

            _wallmountModeCache = _mainManager.IsWallmountMode;

            _multiDisplayMode = SRDProjectSettings.GetMutlipleDisplayMode();
            _numberOfDevices = SRDProjectSettings.GetNumberOfDevices();

#if UNITY_EDITOR
            SimulateMultiDisplay();
#else
            var numberOfSessions = SRDApplicationWindow.NumberOfConnectedDevices;
            StartCoroutine(AddManagers(numberOfSessions));
#endif
            CheckForceWallmount();
        }

        private void Update()
        {
            if (SRDProjectSettings.GetMutlipleDisplayMode() != _multiDisplayMode || SRDProjectSettings.GetNumberOfDevices() != _numberOfDevices)
            {
                _multiDisplayMode = SRDProjectSettings.GetMutlipleDisplayMode();
                _numberOfDevices = SRDProjectSettings.GetNumberOfDevices();
                CheckForceWallmount();
#if UNITY_EDITOR
                SimulateMultiDisplay();
#endif
            }
        }

        IEnumerator AddManagers(int numberOfSessions)
        {
            var targetDisplay = SRDApplicationWindow.DeviceIndexToDisplayIndex[0];
            SRDApplicationWindow.ActivateDisplay(targetDisplay);
            _mainManager.RegisterTargetDisplay(targetDisplay);

            var positions = SRDManagerPositions[_multiDisplayMode];
            var previousSession = _mainManager.Session;
            for (int i = 1; i < numberOfSessions; i++)
            {
                yield return new WaitUntil(previousSession.IsRunning);
                _currentIndex = i;

                var managerObject = new GameObject(_mainManager.name + i);
                SRDManager manager = managerObject.AddComponent<SRDManager>();
                manager.IsSpatialClippingActive = _mainManager.IsSpatialClippingActive;
                manager.IsHighImageQualityMode = _mainManager.IsHighImageQualityMode;
                manager._scalingMode = _mainManager._scalingMode;
                manager.IsWallmountMode = _mainManager.IsWallmountMode;
                manager.SRDViewSpaceScale = _mainManager.SRDViewSpaceScale;

                var positionShift = (positions[i] / manager.Settings.DeviceInfo.BodyBounds.ScaleFactor) * manager.SRDViewSpaceScale;

                manager.transform.position = _mainManager.transform.position + _mainManager.transform.rotation * positionShift;
                manager.transform.rotation = _mainManager.transform.rotation;

#if UNITY_EDITOR
                manager.RegisterTargetDisplay(i);
#else
                targetDisplay = SRDApplicationWindow.DeviceIndexToDisplayIndex[i];
                SRDApplicationWindow.ActivateDisplay(targetDisplay);
                manager.RegisterTargetDisplay(targetDisplay);
#endif
                _srdManagers.Add(manager);
                previousSession = manager.Session;
            }
        }

        private void CheckForceWallmount()
        {
            var isForceWallmountMode = _multiDisplayMode == SRDProjectSettings.MultiSRDMode.MultiVertical || _multiDisplayMode == SRDProjectSettings.MultiSRDMode.MultiGrid;

            foreach (var manager in _srdManagers)
            {
                manager.IsWallmountMode = _wallmountModeCache || isForceWallmountMode;
            }
        }

#if UNITY_EDITOR
        Vector3[] GetSortedPositions(SRDProjectSettings.MultiSRDMode mode, int deviceNum)
        {
            Vector3[] positions = new Vector3[deviceNum];
            Array.Copy(SRDManagerPositions[mode], positions, deviceNum);

            if (mode == SRDProjectSettings.MultiSRDMode.MultiVertical && deviceNum == 4)
            {
                var tmp = positions[3];
                positions[3] = positions[2];
                positions[2] = tmp;
            }

            return positions;
        }

        private void SimulateMultiDisplay()
        {
            if (SwitchSRDManagerPositionCoroutine != null)
            {
                StopCoroutine(SwitchSRDManagerPositionCoroutine);
            }
            var positions = GetSortedPositions(_multiDisplayMode, _numberOfDevices);

            SwitchSRDManagerPositionCoroutine = StartCoroutine(SwitchSRDManagerPosition(positions));
        }

        IEnumerator SwitchSRDManagerPosition(Vector3[] positions)
        {
            int index = 0;
            var presence = _mainManager.Presence;
            while (Application.isPlaying)
            {
                presence.localPosition = positions[index] / _mainManager.Settings.DeviceInfo.BodyBounds.ScaleFactor;
                index = (index + 1) % positions.Length;
                yield return new WaitForSeconds(SRDProjectSettings.GetPositionSwitchInterval());
            }
        }
#endif
    }
}