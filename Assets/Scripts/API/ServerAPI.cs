using UnityEngine;
using UnityEngine.Networking;

namespace TestTask.API
{
    public class ServerAPI
    {
        private string _authCode;
        private string _serverURL;

        public ServerAPI(string authCode, string serverUrl)
        {
            _authCode = authCode;
            _serverURL = serverUrl;
        }

        public void SendAction(int id, string action)
        {
            var request = new UnityWebRequest(_serverURL, UnityWebRequest.kHttpVerbPOST);
            request.SetRequestHeader("auth", _authCode);
            request.SetRequestHeader("action",action);
            request.SetRequestHeader("id",id.ToString());

            var operation = request.SendWebRequest();
            operation.completed += OnRequestCompleted;
        }

        private void OnRequestCompleted(AsyncOperation operation)
        {
            operation.completed -= OnRequestCompleted;            
            var webRequest = operation as UnityWebRequestAsyncOperation;
            if (webRequest==null)
                return;

            if (!string.IsNullOrEmpty(webRequest.webRequest.error))
            {
                Debug.LogError(string.Format("request failed with error: {0}",webRequest.webRequest.error));
            }
            else
            {
                Debug.Log(string.Format("request response code: {0}", webRequest.webRequest.responseCode));
            }

            webRequest.webRequest.Dispose();           
        }
    }
}
