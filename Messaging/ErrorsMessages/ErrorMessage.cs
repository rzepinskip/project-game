using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.ErrorsMessages
{
    [XmlType(XmlRootName)]
    public class ErrorMessage : Message
    {
        public const string XmlRootName = "Error";

        public ErrorMessage(string exceptionType, string exceptionMessage = "", string exceptionCauseParameterName = "")
        {
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;
            ExceptionCauseParameterName = exceptionCauseParameterName;
        }

        [XmlAttribute(DataType = "NCName")]
        public string ExceptionType { get; }

        [XmlAttribute]
        public string ExceptionMessage { get; }

        [XmlAttribute(DataType = "NCName")]
        public string ExceptionCauseParameterName { get; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new System.NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new System.NotImplementedException();
        }

        public override string ToLog()
        {
            return XmlRootName;
        }
    }
}