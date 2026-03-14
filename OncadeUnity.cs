using UnityEngine;

namespace OncadeUnity
{
    [CreateAssetMenu(fileName = "OncadeConfig", menuName = "Oncade/Config")]
    public class OncadeConfig : ScriptableObject
    {
        public string apiBaseUrl = "https://api.oncade.xyz/v1";
        public string apiKey;
        public string gameId;
    }
}

/*Usage in Unity:

Right Click → Create → Oncade → Config
OncadeClient.cs

Singleton API gateway.*/

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OncadeUnity
{
    public class OncadeClient : MonoBehaviour
    {
        public static OncadeClient Instance;

        public OncadeConfig config;

        private static readonly HttpClient http = new HttpClient();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        async Task<string> SendRequest(string endpoint, HttpMethod method, string json = "{}")
        {
            var url = $"{config.apiBaseUrl}/{endpoint}";

            var req = new HttpRequestMessage(method, url);

            req.Headers.Add("Authorization", $"Bearer {config.apiKey}");
            req.Headers.Add("X-Game-Id", config.gameId);
            req.Headers.Add("X-Oncade-API-Version", "v1");
            req.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());

            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await http.SendAsync(req);
            var body = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                Debug.LogError($"Oncade Error: {body}");
                return null;
            }

            return body;
        }

        // ---------------------------------------------------
        // ACCOUNT LINKING
        // ---------------------------------------------------

        public async Task<Models.OncadeUserLinkResponse> InitiateAccountLink(string email = null)
        {
            string payload = email != null ?
                $"{{\"email\":\"{email}\"}}" :
                "{}";

            var res = await SendRequest(
                "users/link/initiate",
                HttpMethod.Post,
                payload
            );

            if (res == null) return null;

            return JsonUtility.FromJson<Models.OncadeUserLinkResponse>(res);
        }

        // ---------------------------------------------------
        // UGC CREATION
        // ---------------------------------------------------

        public async Task<string> CreateUGC(Models.OncadeUGCItem item)
        {
            var json = JsonUtility.ToJson(item);

            return await SendRequest(
                "ugc/create",
                HttpMethod.Post,
                json
            );
        }

        // ---------------------------------------------------
        // REMOVE UGC
        // ---------------------------------------------------

        public async Task<string> RemoveUGC(string itemId)
        {
            var payload = $"{{\"itemId\":\"{itemId}\"}}";

            return await SendRequest(
                "ugc/remove",
                HttpMethod.Post,
                payload
            );
        }

        // ---------------------------------------------------
        // PURCHASE
        // ---------------------------------------------------

        public async Task<string> RequestPurchase(string itemId, string buyerId)
        {
            var payload = $"{{\"itemId\":\"{itemId}\",\"buyerId\":\"{buyerId}\"}}";

            return await SendRequest(
                "purchases/create",
                HttpMethod.Post,
                payload
            );
        }

        // ---------------------------------------------------
        // PAYOUT
        // ---------------------------------------------------

        public async Task<string> RequestPayout(Models.OncadePayout payout)
        {
            var json = JsonUtility.ToJson(payout);

            return await SendRequest(
                "payouts/create",
                HttpMethod.Post,
                json
            );
        }
    }
}

//Models
//These mirror Oncade API schemas.
//OncadeUser.cs
using System;

namespace OncadeUnity.Models
{
    [Serializable]
    public class OncadeUser
    {
        public string id;
        public string email;
    }

    [Serializable]
    public class OncadeUserLinkResponse
    {
        public string url;
        public string sessionKey;
    }
}
//OncadeUGCItem.cs
using System;

namespace OncadeUnity.Models
{
    [Serializable]
    public class OncadeUGCItem
    {
        public string id;
        public string creatorId;
        public string name;
        public string description;
        public int price;
    }
}

//OncadePayout.cs
using System;

namespace OncadeUnity.Models
{
    [Serializable]
    public class OncadePayout
    {
        public string userId;
        public float amount;
        public string currency;
    }
}


using UnityEditor;
using UnityEngine;

/*Editor Tools
OncadeSettingsEditor.cs

Small quality-of-life inspector.
*/

namespace OncadeUnity.Editor
{
    [CustomEditor(typeof(OncadeConfig))]
    public class OncadeSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);

            if (GUILayout.Button("Open Oncade Docs"))
            {
                Application.OpenURL("https://dev.oncade.xyz/docs");
            }
        }
    }
}