using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class Controller : MonoBehaviour {

	public LoginPanel loginPanel;
	public SocketIOComponent socket;
	public Player playGameObj;
	public Text label;
		
	void Start () {

		StartCoroutine(ConnectToServer());
        socket.On ("USER_CONNECTED", OnUserConnected);
        socket.On ("PLAY", OnUserPlay);
		socket.On ("USER_DISCONNECTED", OnUserDisconnected);
		socket.On ("reply", onServerReply);
		loginPanel.playBtn.onClick.AddListener (OnClickPlayBtn);
    }

	/* ========================== */
	IEnumerator ConnectToServer()
	{
		yield return new WaitForSeconds (0.5f);
		socket.Emit("USER_CONNECT");
		yield return new WaitForSeconds(1f);
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["name"] = "Wachiii";
		socket.Emit("PLAY", new JSONObject(data));

	}


	private void OnUserConnected ( SocketIOEvent evt)
	{
		Debug.Log("Get the message from server is: " + evt.data + " OnUserConnected");
		GameObject otherPlayer = GameObject.Instantiate (playGameObj.gameObject, playGameObj.position, Quaternion.identity) as GameObject;
		Player otherPlayerCom = otherPlayer.GetComponent<Player> ();

		otherPlayerCom.playerName = JsonToString(evt.data.GetField("name").ToString(), "\"");
		otherPlayerCom.id = JsonToString (evt.data.GetField ("id").ToString (), "\"");

	}


	private void OnUserPlay(SocketIOEvent evt)
	{
		Debug.Log("Get the message from server is: " + evt.data + " OnUserPlay");
		loginPanel.gameObject.SetActive (false);

		//		GameObject player = GameObject.Instantiate (playGameObj.gameObject, playGameObj.position, Quaternion.identity) as GameObject;
		//		Player playerCom = player.GetComponent<Player> ();

		//		playerCom.playerName = JsonToString (evt.data.GetField ("name").ToString (), "\"");
		//		playerCom.transform.position = JsonToVector3 (JsonToString (evt.data.GetField ("position").ToString (), "\""));
		//		playerCom.id = JsonToString (evt.data.GetField("id").ToString (), "\"");
		//		joyStick.playerObj = player; 
	}


	string JsonToString (string target , string s)
	{
		string[] newString = Regex.Split (target, s);
		return newString [1];
	}


	private void OnUserDisconnected (SocketIOEvent obj)
	{
		Destroy(GameObject.Find(JsonToString (obj.data.GetField ("position").ToString (), "\"") ) ); 
	}


	private void onServerReply (SocketIOEvent obj)
	{
		label.text = obj.data.ToString ();

	}


	private void OnClickPlayBtn ()
	{ 
		if (loginPanel.inputField.text != "") {
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data ["name"] = loginPanel.inputField.text;
//			Vector3 position = new Vector3 (0, 0, 0);
//			data ["position"] = position.x + "," + position.y + "," + position.z;
			socket.Emit ("PLAY", new JSONObject (data));
		} else {
			loginPanel.inputField.text = "Please enter your name again";
		}
	}

		
	/*
	Vector3 JsonToVector3 (string target)
	{
		Vector3 newVector;
		string[] newString = Regex.Split (target, ",");
		newVector = new Vector3 (float.Parse (newString [0]), float.Parse (newString [1]), float.Parse (newString [2]));
		return newVector;
	} */
}
