using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Objects;
using Scripts.Utility;
using Packet;

public class RegiesterPopup : MonoBehaviour {


	public InputField IdField;
	public InputField PasswordField;
	public InputField UsernameField;
	public InputField WeightField;
	public Dropdown GenderField;
	public Dropdown BirthYearField;
	public Dropdown BirthMonthField;
	public Dropdown BirthDayField;

	public NetworkManager newtworkManager;
    public MessageBox msgBox;

    // Use this for initialization
    void Start () {

		for(int i=1970;i<System.DateTime.Now.Year;++i)
		{
			BirthYearField.options.Add(new Dropdown.OptionData() { text = i.ToString() });
		}

		for (int i = 1; i < 12; ++i)
		{
			BirthMonthField.options.Add(new Dropdown.OptionData() { text = i.ToString() });
		}

		// 고쳐야됨~
		for (int i = 1; i < 31; ++i)
		{
			BirthDayField.options.Add(new Dropdown.OptionData() { text = i.ToString() });
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onClickSignupButton()
	{
		if (newtworkManager.isConnected ())
		{
			PACEKT_REQ_CREATE_ACCOUNT packet = new PACEKT_REQ_CREATE_ACCOUNT();

			packet.Init();

			packet.login_id = IdField.text;
			packet.password = PasswordField.text;
			packet.username = UsernameField.text;

			packet.weight = byte.Parse(WeightField.text);
			packet.gender = GenderField.options[GenderField.value].text == "남성" ? true : false;

			int year = Int32.Parse(BirthYearField.options[BirthYearField.value].text);
			int month = Int32.Parse(BirthMonthField.options[BirthMonthField.value].text);
			int day = Int32.Parse(BirthDayField.options[BirthDayField.value].text);

			var time = new DateTime(year, month, day) - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

			packet.birthday = Convert.ToInt32(time.TotalSeconds);

			byte[] data = Utility.ToByteArray((object)packet);

			newtworkManager.Send(data);
		
		}
		else
		{
			Message _msg = new Message (MessageType.SOCKET_CLOSED);
			MessageQueue.getInstance.Push (_msg);
		}
	}
}
