using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCReceiver : MonoBehaviour
{
    #region Network Settings
    public string TargetAddr;
    public int OutGoingPort;
    public int InComingPort;
    #endregion

    private Dictionary<string, ServerLog> servers;
    private int lastPacketIndex_ = 0;
    public GameObject dynamicObjectHandler = null;
    private DynamicObjectController controller_;

    void Start()
	{
        OSCHandler.Instance.Init(TargetAddr, OutGoingPort, InComingPort);
        servers = new Dictionary<string, ServerLog>();
        controller_ = dynamicObjectHandler.GetComponent<DynamicObjectController>();
    }

    void Update()
	{
        OSCHandler.Instance.UpdateLogs();

        servers = OSCHandler.Instance.Servers;
        foreach (KeyValuePair<string, ServerLog> item in servers) {
        	for (int i = lastPacketIndex_; i < item.Value.packets.Count; ++i) {
				var msg = new Dictionary<string, int>();
				switch (item.Value.packets[i].Address) {
					case "/LegoAnalyzer/AddBlock":
						msg["x"] = (int)item.Value.packets[i].Data[0];
						msg["y"] = (int)item.Value.packets[i].Data[1];
						controller_.SendMessage("addBlock", msg);
						break;
					case "/LegoAnalyzer/DeleteBlock":
						msg["x"] = (int)item.Value.packets[i].Data[0];
						msg["y"] = (int)item.Value.packets[i].Data[1];
						controller_.SendMessage("deleteBlock", msg);
						break;
					case "/LegoAnalyzer/ChangeBlock":
						msg["kind"] = (int)item.Value.packets[i].Data[0];
						controller_.SendMessage("changeBlock", msg);
						break;
					default:
						Debug.LogError("Unknown OSC msg, address is: " + item.Value.packets[i].Address);
						break;
				}
			}
            lastPacketIndex_ = item.Value.packets.Count;
        }
    }
}
