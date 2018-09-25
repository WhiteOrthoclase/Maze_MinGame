using UnityEngine.UI;
using UnityEngine;

public class TokenCountDisplay : MonoBehaviour {
    public Text countText;

	// Update is called once per frame
	void Update () {
        countText.text = ("Tokens : "+ TokenCounter.tokensInTotal);
	}
}
