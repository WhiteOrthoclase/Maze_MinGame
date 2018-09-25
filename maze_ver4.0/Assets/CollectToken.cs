using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectToken : MonoBehaviour {

    // Check if FPSController player has hit a token
    void OnTriggerEnter(Collider info)
    {
        if (info.name == "Player")
        {
            Destroy(gameObject);
            TokenCounter.tokensInTotal++;
            Debug.Log("tokens = " + TokenCounter.tokensInTotal);

        }
    }
}
