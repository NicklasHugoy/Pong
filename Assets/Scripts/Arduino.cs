using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

namespace Arduino
{

	public enum PinModes
	{
		INPUT = 0,
		OUPUT = 1,
		INPUT_PULLUP = 2
	}

	public class ArduinoPort : IDisposable
	{

		public SerialPort serialPort;

		public ArduinoPort()
		{
			serialPort = CheckPorts();
		}

		/// <summary>
		/// Pin 2~13
		/// 14~19 = A0~A5
		/// </summary>
		public void SetPin(int pin, bool state)
		{
			if (state)
			{
				Send(new byte[] { (byte)pin, 1 });
			}
			else {
				Send(new byte[] { (byte)pin, 0 });
			}
		}
		/// <summary>
		/// Pin 2~13
		/// 14~19 = A0~A5
		/// </summary>
		public bool GetPin(int pin)
		{
			return Send(new byte[] { (byte)pin, 2 }, 1)[0] == 1;
		}
		/// <summary>
		/// Pin 2~13
		/// 14~19 = A0~A5
		/// result 0~1023
		/// </summary>
		public short GetAnalogPin(int pin)
		{
			byte[] response = Send(new byte[] { (byte)pin, 3 }, 2);
			return BitConverter.ToInt16(response, 0);
		}
		/// <summary>
		/// Pin 2~13
		/// 14~19 = A0~A5
		/// result 0~1023
		/// </summary>
		public bool PinMode(int pin, PinModes pinMode)
		{
			return Send(new byte[] { (byte)pin, 4, (byte)pinMode }, 1)[0] == 1;
		}

		public byte[] Send(byte[] command, int expectedReturn)
		{
			serialPort.Write(command, 0, command.Length);

			byte[] task = GetResponse(expectedReturn, 20);

			if (task != null)
			{
				return task;
			}
			else {
				return new byte[0];
			}
		}

		public void Send(byte[] command)
		{
			serialPort.Write(command, 0, command.Length);
		}

		private byte[] GetResponse(int expectedReturnSize, float timeout)
		{
			List<byte> data = new List<byte>();
			while (timeout > 0)
			{
				timeout -= Time.deltaTime;
				try
				{
					int newData = serialPort.ReadByte();
					if (newData == -1) continue;
					else data.Add((byte)newData);
					if (data.Count >= expectedReturnSize)
					{
						byte[] result = data.ToArray();


						return result;
					}
				}
				catch (Exception e)
				{
					continue;
				}
			}
			return null;
		}

		//private byte[] synchResponse(int expectedReturnSize) {
		//    while (true) {
		//        if (serialPort.BytesToRead >= expectedReturnSize) {
		//            byte[] result = new byte[expectedReturnSize];
		//            serialPort.Read(result, 0, expectedReturnSize);


		//            return result;
		//        }
		//    }
		//}

		private SerialPort CheckPorts()
		{

			List<string> ports = SerialPort.GetPortNames().ToList();
			Debug.Log(ports.Count);
			foreach (var cPort in ports)
			{
				Debug.Log(cPort);

				try
				{
					SerialPort newPort = new SerialPort(cPort, 9600);

					if (newPort == null)
					{
						Debug.Log("NULL");
					}

					newPort.Open();
					newPort.DiscardInBuffer();
					newPort.ReadTimeout = 1;
					bool check = CheckPort(newPort, 100f);

					if (check)
					{
						Debug.Log(cPort + ": " + "Success");
						return newPort;
					}
					else
					{
						Debug.Log(cPort + ": Fail");
					}
				}
				catch (Exception e)
				{
					Debug.Log(cPort + " Access denied");
				}
			}
			return null;
		}

		private bool CheckPort(SerialPort port, float time)
		{
			byte[] keyCheck = new byte[4];
			float timeout = time;
			while (timeout > 0)
			{
				timeout -= Time.deltaTime;
				Debug.Log(timeout);
				if (port.IsOpen)
				{
					try
					{
						List<byte> data = new List<byte>();
						while (true)
						{
							int newData = port.ReadByte();
							if (newData == -1)
							{
								break;
							}
							else {
								data.Add((byte)newData);
								if (data.Count > 0)
								{
									while (data.Count > 0)
									{
										keyCheck = ShiftLeft(keyCheck);
										keyCheck[3] = data[0];
										data.RemoveAt(0);

										if (CheckCombo(keyCheck, new byte[] { 153, 53, 85, 231 }))
										{
											port.BaseStream.WriteByte(197);
											port.DiscardInBuffer();
											return true;
										}
									}

								}
							}
						}

					}
					catch (Exception e) { }
				}
			}
			return false;
		}

		private bool CheckCombo(byte[] data, byte[] wanted)
		{
			if (data.Length != wanted.Length)
				return false;

			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] != wanted[i])
					return false;
			}

			return true;
		}

		private byte[] ShiftLeft(byte[] data)
		{
			for (int i = 0; i < data.Length - 1; i++)
			{
				data[i] = data[i + 1];
			}
			data[3] = 0;

			return data;
		}

		public void Dispose()
		{
			if (Send(new byte[] { 255 }, 1)[0] == 1)
			{
				Debug.Log("Arduino accepted close process");
			}
			else {
				Debug.Log("Arduino did not accept shutdown procedure");
			}
			serialPort.Close();
		}
	}
}