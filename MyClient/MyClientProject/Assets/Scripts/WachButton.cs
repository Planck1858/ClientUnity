using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;



public class WachButton : MonoBehaviour {

	public delegate void OnActionPress(GameObject unit, bool state);
	public event OnActionPress OnPress;
	EventTrigger eventTrigger;

	void Start()
	{
		Debug.Log (this.gameObject.name);
		eventTrigger = this.gameObject.GetComponent<EventTrigger>();
		AddEventTrigger(OnPointDown, EventTriggerType.PointerDown);
		AddEventTrigger(OnPointUp, EventTriggerType.PointerUp);

	}

	void AddEventTrigger( UnityAction action, EventTriggerType triggerType)
	{
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
		trigger.AddListener( (eventData) => action() );

		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
		eventTrigger.triggers.Add (entry);
	}
	void OnPointDown()
	{
		if(OnPress != null)
		{
			OnPress(this.gameObject, true);
		}
	}

	void OnPointUp() 
	{
		if (OnPress != null)
		{
			OnPress(this.gameObject, false);
		}

	}

}
