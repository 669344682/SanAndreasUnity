﻿using System.Collections.Generic;
using UnityEngine;
using SanAndreasUnity.Utilities;
using SanAndreasUnity.Net;

namespace SanAndreasUnity.UI
{

	public class StartGameWindow : PauseMenuWindow
    {
		string m_portStr = NetManager.defaultListenPortNumber.ToString();
		bool m_dontListen = false;
		bool m_dedicatedServer = false;
		string m_maxNumPlayersStr = "40";
		[SerializeField] string[] m_availableScenes = new string[0];
		int m_selectedSceneIndex = 0;
	    bool m_registerAtMasterServer = false;

		public int width = 550;
		public int height = 400;
		public int minStartButtonWidth = 80;
		public int minStartButtonHeight = 30;
		public int minMapButtonHeight = 20;
		public int mapSelectionGridXCount = 4;


		StartGameWindow()
        {

			// set default parameters

			this.windowName = "Start Game";
			this.useScrollView = true;

		}

		void Start ()
        {
			// adjust rect
			this.windowRect = GUIUtils.GetCenteredRect(new Vector2(this.width, this.height));
		}

		void Update()
		{
			if (PauseMenu.IsOpened)
				this.IsOpened = false;
		}


		protected override void OnWindowGUI ()
		{
			
            GUILayout.Label ("Port:");
			m_portStr = GUILayout.TextField(m_portStr, GUILayout.Width(100));

			m_dontListen = GUILayout.Toggle(m_dontListen, "Don't listen");
            
			m_dedicatedServer = GUILayout.Toggle(m_dedicatedServer, "Dedicated server");
            
			GUILayout.Label("Max num players:");
			m_maxNumPlayersStr = GUILayout.TextField(m_maxNumPlayersStr, GUILayout.Width(100));

			GUILayout.Label("Map:");
			m_selectedSceneIndex = GUILayout.SelectionGrid(m_selectedSceneIndex, m_availableScenes, this.mapSelectionGridXCount, GUILayout.MinHeight(this.minMapButtonHeight));
			GUILayout.Space(5);

			m_registerAtMasterServer = GUILayout.Toggle(m_registerAtMasterServer, "Register at master server");

		}

		protected override void OnWindowGUIAfterContent()
		{
			GUILayout.Space(40);

			GUI.enabled = ! NetStatus.IsServer;
            if (GUIUtils.ButtonWithCalculatedSize("Start", this.minStartButtonWidth, this.minStartButtonHeight))
				StartGame();
            GUI.enabled = true;

            GUILayout.Space(10);

		}

		void StartGame()
		{
			try
			{
				ushort port = ushort.Parse(m_portStr);
				string scene = m_availableScenes[m_selectedSceneIndex];
				ushort maxNumPlayers = ushort.Parse(m_maxNumPlayersStr);

				MasterServerClient.Instance.IsServerRegistrationEnabled = m_registerAtMasterServer;

				NetManager.StartServer(port, scene, maxNumPlayers, m_dedicatedServer, m_dontListen);

			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex);
				MessageBox.Show("Error", ex.ToString());
			}
		}

	}

}
