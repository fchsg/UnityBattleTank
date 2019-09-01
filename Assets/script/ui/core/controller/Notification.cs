using UnityEngine;
using System;

public class Notification
{
	public Component _sender;
	public String _name;
	public System.Object _data;

	public Notification(System.Object data) { _data = data; _sender = null; _name = null; }
	public Notification(Component sender, String name) { _sender = sender; _name = name; _data = null; }
	public Notification(Component sender, String name, System.Object data) { _sender = sender; _name = name; _data = data; }
}