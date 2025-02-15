﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    //Variable

    //Set up a static so that we can call this script from anywhere
    public static LaunchManager instance;

    public string username;
    public bool clearPrefs;

    #region Unity Methods

    private void Awake()
    {
        if (clearPrefs)
            DeletePlayerPrefs();

        username = "username";

        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("PLAYERNAME"))
        {
            PhotonNetwork.NickName = SystemInfo.deviceName + "_" + SystemInfo.deviceModel;
            username = PhotonNetwork.NickName;
        }
        else
        {
            username = PlayerPrefs.GetString("PLAYERNAME");
        }

        LoadSetting();
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
    #region Public Methods

    public void LoadSetting()
    {
        if(PlayerPrefs.HasKey("PLAYERNAME"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PLAYERNAME");
            ConnectToPhotonServers();
            PhotonNetwork.LoadLevel(1);
        }
    }
    public void ConnectToPhotonServers()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void InputName(string playerName)
    {
        if(string.IsNullOrEmpty(playerName))
        {
            return;
        }
        username = playerName;
    }

    public void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString("PLAYERNAME", username);
        PhotonNetwork.NickName = username;
        PlayerPrefs.SetInt("YourRank", 1);
        ConnectToPhotonServers();
        //PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel(1);
    }
    #endregion

    #region PUN Callbacks

    public override void OnConnected()
    {
        //base.OnConnected();
        Debug.Log(PhotonNetwork.NickName + " has connected to the Photon Servers");
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log(PhotonNetwork.NickName + " has connected to the Master Servers");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
    }
    #endregion
}
