using System;
using System.Collections;
using UnityEngine;
using LastShift.Player;

namespace LastShift.Flow
{
    /// <summary>
    /// Executes the 8-step scripted ending sequence.
    /// Implements END-01..END-10, NAR-01..04, PLR-04.
    /// </summary>
    public class EndingSequenceController : MonoBehaviour
    {
        [Header("Actor & Camera")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Camera _endingCamera;

        [Header("Cinematic Focus Points")]
        [SerializeField] private Transform _whiteLightPoint;      // END-01
        [SerializeField] private Transform _darkMonitorPoint;     // END-02, END-03
        [SerializeField] private Transform _idCardPoint;          // END-04
        [SerializeField] private Transform _litMonitorPoint;       // END-05
        [SerializeField] private Transform _slumpedBodyPoint;     // END-06
        [SerializeField] private Transform _newspaperPoint;        // END-07

        [Header("Audio (END-09, END-10)")]
        [SerializeField] private AudioSource _officeHumSource;
        [SerializeField] private AudioSource _notificationChimeSource;

        [Header("UI Overlays")]
        [SerializeField] private CanvasGroup _whiteoutGroup;
        [SerializeField] private CanvasGroup _blackoutGroup;
        [SerializeField] private GameObject _titleCardObject;

        public event Action<int> OnEndingStepReached;
        public event Action OnEndingSequenceCompleted;

        private void Start()
        {
            if (_endingCamera != null)
            {
                _endingCamera.gameObject.SetActive(false);
            }
            if (_whiteoutGroup != null) _whiteoutGroup.alpha = 0f;
            if (_blackoutGroup != null) _blackoutGroup.alpha = 0f;
            if (_titleCardObject != null) _titleCardObject.SetActive(false);
        }

        public void PlayEndingSequence()
        {
            StartCoroutine(EndingRoutine());
        }

        private IEnumerator EndingRoutine()
        {
            // PLR-04: Lock player input
            if (_playerController != null)
            {
                _playerController.SetInputLocked(true);
            }

            // Switch to ending cinematic camera
            if (_endingCamera != null)
            {
                _endingCamera.gameObject.SetActive(true);
                if (_playerController != null && _playerController.CameraTransform != null)
                {
                    _endingCamera.transform.position = _playerController.CameraTransform.position;
                    _endingCamera.transform.rotation = _playerController.CameraTransform.rotation;
                }
            }

            // END-01: Exiting vent into white light
            OnEndingStepReached?.Invoke(1);
            yield return StartCoroutine(FadeCanvasGroup(_whiteoutGroup, 0f, 1f, 1.5f));

            // END-02: White light & audio transition to dark computer monitor & office drone
            OnEndingStepReached?.Invoke(2);
            if (_officeHumSource != null && !_officeHumSource.isPlaying)
            {
                _officeHumSource.Play();
            }

            if (_endingCamera != null && _darkMonitorPoint != null)
            {
                _endingCamera.transform.position = _darkMonitorPoint.position;
                _endingCamera.transform.rotation = _darkMonitorPoint.rotation;
            }
            yield return StartCoroutine(FadeCanvasGroup(_whiteoutGroup, 1f, 0f, 2.0f));

            // END-03: Camera focuses on dark monitor reflection
            OnEndingStepReached?.Invoke(3);
            yield return new WaitForSeconds(3.0f);

            // END-04: Camera tilts down to employee ID card
            OnEndingStepReached?.Invoke(4);
            if (_endingCamera != null && _idCardPoint != null)
            {
                yield return StartCoroutine(MoveCameraTo(_idCardPoint, 2.0f));
            }
            yield return new WaitForSeconds(2.5f);

            // END-05: Monitor screen wakes up, unread message from mother, single notification chime
            OnEndingStepReached?.Invoke(5);
            if (_notificationChimeSource != null)
            {
                _notificationChimeSource.Play();
            }
            if (_endingCamera != null && _litMonitorPoint != null)
            {
                yield return StartCoroutine(MoveCameraTo(_litMonitorPoint, 2.0f));
            }
            yield return new WaitForSeconds(3.0f);

            // END-06: Camera shifts sideways to reveal slumped body in office chair
            OnEndingStepReached?.Invoke(6);
            if (_endingCamera != null && _slumpedBodyPoint != null)
            {
                yield return StartCoroutine(MoveCameraTo(_slumpedBodyPoint, 2.5f));
            }
            yield return new WaitForSeconds(3.5f);

            // END-07: Camera pans to newspaper on desk (found deceased after overtime)
            OnEndingStepReached?.Invoke(7);
            if (_endingCamera != null && _newspaperPoint != null)
            {
                yield return StartCoroutine(MoveCameraTo(_newspaperPoint, 2.0f));
            }
            yield return new WaitForSeconds(3.5f);

            // END-08: Fade to black and display title card "LAST SHIFT"
            OnEndingStepReached?.Invoke(8);
            yield return StartCoroutine(FadeCanvasGroup(_blackoutGroup, 0f, 1f, 2.0f));

            if (_titleCardObject != null)
            {
                _titleCardObject.SetActive(true);
            }
            yield return new WaitForSeconds(4.0f);

            OnEndingSequenceCompleted?.Invoke();
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
        {
            if (group == null) yield break;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                group.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }
            group.alpha = end;
        }

        private IEnumerator MoveCameraTo(Transform target, float duration)
        {
            if (_endingCamera == null || target == null) yield break;

            Vector3 startPos = _endingCamera.transform.position;
            Quaternion startRot = _endingCamera.transform.rotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                _endingCamera.transform.position = Vector3.Lerp(startPos, target.position, t);
                _endingCamera.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
                yield return null;
            }

            _endingCamera.transform.position = target.position;
            _endingCamera.transform.rotation = target.rotation;
        }
    }
}
