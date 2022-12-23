using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBS.Notifications;
using ATBS.Extensions;
using System;

public class NotificationsMenu : MonoBehaviour
{
	[SerializeField] private PopupManager popupManager; // popup manager reference
	[SerializeField] private Button openImportant, openNonimportant, openSystem, nextInQueue;
	private Dictionary<string, Action> actions; // Dictionary of custom actions
	
	private void Awake()
	{
		// fill actions dictionary with own actions
		actions = new Dictionary<string, Action>{
			{"test".Clean(), Test},
			{"another".Clean(), Another},
			{"lastone".Clean(), () => {Debug.Log("Last one action");}}
		};

		SetupButtons();
	}

	private void SetupButtons()
	{
		openImportant.onClick.AddListener(() =>
		{
			// Open a new popup using OpenPopup and add actions dictionary to the new popup Actions dictionary
			popupManager?.OpenPopup(PopupType.Important, 0).Actions.Add(actions);
		});
		
		openNonimportant.onClick.AddListener(() =>
		{
			// Open a new popup using OpenPopup and add actions dictionary to the new popup Actions dictionary
			popupManager?.OpenPopup(PopupType.NonImportant, 0).Actions.Add(actions);;
		});

		openSystem.onClick.AddListener(() =>
		{
			// Open a new popup using OpenPopup and add actions dictionary to the new popup Actions dictionary
			popupManager?.OpenPopup(PopupType.System, 0).Actions.Add(actions);;
		});

		nextInQueue.onClick.AddListener(() =>
		{
			// Force closing of a currently open popup
			popupManager?.CloseCurrentPopup();
		});
	}

	private void Test()
	{
		Debug.Log(popupManager.CurrentPopup?[0]);
	}

	private void Another()
	{
		Debug.Log("My another action");
	}
}
