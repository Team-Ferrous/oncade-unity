# OncadeUnity

**Oncade API Integration for the Unity Engine**

OncadeUnity is a lightweight Unity SDK designed to connect Unity-based games and interactive applications with the **Oncade platform**.

The integration provides a centralized API client for account linking, user-generated content (UGC) management, purchase requests, and payout requests, allowing developers to incorporate Oncade services directly into their Unity projects.

Built using C# and Unity's native development environment, OncadeUnity provides reusable data models, asynchronous HTTP communication, and editor configuration tools.

## Features

### Account Linking
Initiate the Oncade account-linking process through the API, with optional email-based identification.

- Initiate account-linking sessions.
- Retrieve account-link URLs and session keys.
- Support integration with external account authentication workflows.

### User-Generated Content (UGC)
Submit requests to create and remove user-generated content through Oncade's API.

- Create UGC entries with creator information.
- Define item names, descriptions, and prices.
- Request removal of existing UGC entries.

### Purchases and Transactions
Integrate purchase requests for marketplace content.

- Submit purchase requests using item and buyer identifiers.
- Communicate with the Oncade purchase endpoint.
- Receive API responses for application-specific processing.

### Payout Requests
Support payout workflows through the Oncade API.

- Submit payout requests associated with user accounts.
- Specify payout amounts and currencies.
- Receive responses from the payout service.

### Unity Editor Integration
OncadeUnity includes a ScriptableObject-based configuration system and a custom Unity Inspector.

Developers can configure their API endpoint, game identifier, and authentication settings directly inside Unity.

The integration also provides convenient access to the Oncade developer documentation.

## Architecture

OncadeUnity consists of four primary components:

| Component | Purpose |
|---|---|
| `OncadeConfig` | Stores API configuration and game identifiers |
| `OncadeClient` | Centralized HTTP client for API requests |
| `OncadeUnity.Models` | Serializable request and response models |
| `OncadeSettingsEditor` | Unity Inspector integration and documentation access |

The client uses asynchronous HTTP requests through .NET's `HttpClient` and JSON serialization through Unity's `JsonUtility`.

API requests include bearer authentication, game identification, an API-version header, and unique idempotency keys.

## Getting Started

1. Add the OncadeUnity scripts to your Unity project.
2. Create a configuration asset using **Create → Oncade → Config**.
3. Configure the Oncade API base URL and game identifier.
4. Add `OncadeClient` to a GameObject in your scene.
5. Assign your `OncadeConfig` asset to the client.
6. Access the client through `OncadeClient.Instance`.

### Example: Initiating Account Linking

```csharp
using UnityEngine;
using OncadeUnity;

public class OncadeExample : MonoBehaviour
{
    public async void LinkAccount()
    {
        var result = await OncadeClient.Instance
            .InitiateAccountLink();

        if (result != null)
        {
            Debug.Log($"Link URL: {result.url}");
        }
    }
}
```

## Requirements

- Unity with support for C# asynchronous programming
- Access to the Oncade API
- Valid game configuration and authentication credentials
- Network connectivity for API requests

## Security Considerations

Do not embed production secret API keys in distributed Unity clients or publicly committed configuration assets.

For production environments, privileged operations such as purchases, payouts, and authenticated marketplace management should be routed through a trusted backend that securely manages credentials and validates requests.

## Development Status

**Initial SDK implementation**

The current implementation provides foundational API request methods and serializable models. Additional production features, including token management, retries, request cancellation, structured error handling, transaction verification, and expanded API response models, may be added in future releases.

## Documentation

Oncade Developer Documentation: https://dev.oncade.xyz/docs

---

**OncadeUnity** — Connecting Unity applications with the Oncade ecosystem.
