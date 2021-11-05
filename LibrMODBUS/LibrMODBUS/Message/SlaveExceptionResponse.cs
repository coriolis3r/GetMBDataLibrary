using System;
using System.Collections.Generic;
using System.Globalization;

namespace Modbus.Message
{
	internal class SlaveExceptionResponse : ModbusMessage, IModbusMessage
	{
		private static readonly Dictionary<byte, string> _exceptionMessages = CreateExceptionMessages();		

		public SlaveExceptionResponse()
		{
		}

		public SlaveExceptionResponse(byte slaveAddress, byte functionCode, byte exceptionCode)
			: base(slaveAddress, functionCode)	
		{
			SlaveExceptionCode = exceptionCode;
		}

		public override int MinimumFrameSize
		{
			get { return 3; }
		}

		public byte SlaveExceptionCode
		{
			get { return MessageImpl.ExceptionCode.Value; }
			set { MessageImpl.ExceptionCode = value; }
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			string message = _exceptionMessages.ContainsKey(SlaveExceptionCode) ? _exceptionMessages[SlaveExceptionCode] : Resources.Unknown;
			return String.Format(CultureInfo.InvariantCulture, Resources.SlaveExceptionResponseFormat, Environment.NewLine, FunctionCode, SlaveExceptionCode, message);
		}

		internal static Dictionary<byte, string> CreateExceptionMessages()
		{
			Dictionary<byte, string> messages = new Dictionary<byte, string>(9);

			messages.Add(1, "IllegalFunction");
			messages.Add(2, "IllegalDataAddress");
			messages.Add(3, "IllegalDataValue");
			messages.Add(4, "SlaveDeviceFailure");
			messages.Add(5, "Acknowlege");
			messages.Add(6, "SlaveDeviceBusy");
			messages.Add(8, "MemoryParityError");
			messages.Add(10, "GatewayPathUnavailable");
			messages.Add(11, "GatewayTargetDeviceFailedToRespond");

			return messages;
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (FunctionCode <= Modbus.ExceptionOffset)
				throw new FormatException(Resources.SlaveExceptionResponseInvalidFunctionCode);

			SlaveExceptionCode = frame[2];
		}
	}
}
