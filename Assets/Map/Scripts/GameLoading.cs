using System.Collections;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GameRootFolder.Scripts
{
    public class GameLoading : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
#if UNITY_IOS
            yield return Tracker();
#endif

            yield return LoadSceneAsync();
            yield return null;
        }


        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            //operation.allowSceneActivation = false;

            while (operation.isDone == false)
                yield return null;

            //operation.allowSceneActivation = true;
        }
        
#if UNITY_IOS
        private IEnumerator Tracker()
        {
            yield return null;

            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (status != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                yield break;

            ATTrackingStatusBinding.RequestAuthorizationTracking();

            while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                yield return null;
            }
        }
#endif
    }
}