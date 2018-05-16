using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Messaging.ActionsMessages;
using Messaging.ErrorsMessages;
using Messaging.InitializationMessages;
using Messaging.KnowledgeExchangeMessages;
using Messaging.Requests;

namespace Messaging.Serialization
{
    public class ExtendedMessageXmlDeserializer : ExtendedXmlSerializer
    {
        private readonly Dictionary<string, XmlSerializer> _serializers;

        public ExtendedMessageXmlDeserializer(string xmlNamespace) : base(xmlNamespace)
        {
            _serializers = new Dictionary<string, XmlSerializer>
            {
                // Actions
                {
                    AuthorizeKnowledgeExchangeRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(AuthorizeKnowledgeExchangeRequest))
                },
                {
                    DestroyPieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(DestroyPieceRequest))
                },
                {
                    DiscoverRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(DiscoverRequest))
                },
                {
                    MoveRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(MoveRequest))
                },
                {
                    PickUpPieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(PickUpPieceRequest))
                },
                {
                    PlacePieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(PlacePieceRequest))
                },
                {
                    TestPieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(TestPieceRequest))
                },
                // Errors
                {
                    ErrorMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(ErrorMessage))
                },
                {
                    GameMasterDisconnected.XmlRootName,
                    GetDefaultXmlSerializer(typeof(GameMasterDisconnected))
                },
                {
                    PlayerDisconnected.XmlRootName,
                    GetDefaultXmlSerializer(typeof(PlayerDisconnected))
                },
                // Initialization request
                {
                    GetGamesMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(GetGamesMessage))
                },
                {
                    JoinGameMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(JoinGameMessage))
                },
                {
                    RegisterGameMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(RegisterGameMessage))
                },
                // Initialization responses
                {
                    ConfirmGameRegistrationMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(ConfirmGameRegistrationMessage))
                },
                {
                    ConfirmJoiningGameMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(ConfirmJoiningGameMessage))
                },
                {
                    GameMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(GameMessage))
                },
                {
                    RegisteredGamesMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(RegisteredGamesMessage))
                },
                {
                    RejectGameRegistrationMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(RejectGameRegistrationMessage))
                },
                {
                    RejectJoiningGame.XmlRootName,
                    GetDefaultXmlSerializer(typeof(RejectJoiningGame))
                },
                // KnowledgeExchangeMessages
                {
                    KnowledgeExchangeRequestMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(KnowledgeExchangeRequestMessage))
                },
                {
                    RejectKnowledgeExchangeMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(RejectKnowledgeExchangeMessage))
                },
                // Other
                {
                    DataMessage.XmlRootName,
                    GetDefaultXmlSerializer(typeof(DataMessage))
                },
            };
        }

        public Message Deserialize(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                return ReadMessage(stream);
            }
        }

        public Message DeserializeFromFile(string filePath)
        {
            using (var stream = new StreamReader(filePath))
            {
                return ReadMessage(stream);
            }
        }

        private Message ReadMessage(TextReader stream)
        {
            var document = XDocument.Load(stream);

            var name = ReadRootName(document);
            var serializer = _serializers[name];

            Message message = null;

            if (document.Root != null)
                message = serializer.Deserialize(document.Root.CreateReader()) as Message;

            return message;
        }

        private string ReadRootName(XDocument document)
        {
            var node = document.Root;
            return node?.Name.LocalName;
        }
    }
}