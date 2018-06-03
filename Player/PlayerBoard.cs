using System.Linq;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Player
{
    public class PlayerBoard : BoardBase, IPlayerBoard
    {
        protected PlayerBoard()
        {
        }

        public PlayerBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }

        public void HandleTaskField(int playerId, TaskField taskField)
        {
            var oldTaskField = (TaskField) this[taskField];

            if (taskField.IsNewerThan(oldTaskField))
            {
                ClearPlayerFromField(oldTaskField);
                ClearPieceFromField(oldTaskField);

                HandlePlayerInField(taskField);
                HandlePieceInField(playerId, taskField);

                this[taskField] = new TaskField(taskField, taskField.DistanceToPiece, taskField.PieceId,
                    taskField.PlayerId);
            }
        }

        public void HandleGoalField(GoalField goalField)
        {
            var oldGoalField = this[goalField];
            if (goalField.IsNewerThan(oldGoalField))
            {
                ClearPlayerFromField(oldGoalField);

                HandlePlayerInField(goalField);

                this[goalField] = goalField;
            }
        }

        public void HandleGoalFieldAfterPlace(int playerId, GoalField goalField)
        {
            Players[playerId].Piece = null;

            this[goalField] = goalField;
        }

        public void HandlePiece(int playerId, Piece piece)
        {
            if (piece.PlayerId == playerId) Players[playerId].Piece = piece;

            if (piece.Type == PieceType.Destroyed && Players[playerId].Piece.Id == piece.Id)
                Players[playerId].Piece = null;
        }

        public void HandlePlayerLocation(int playerId, Location playerUpdatedLocation)
        {
            // Remove old data
            var playerInfo = Players[playerId];
            if (playerInfo.Location != null)
                this[playerInfo.Location].PlayerId = null;

            // Insert new data
            playerInfo.Location = playerUpdatedLocation;
            this[playerUpdatedLocation].PlayerId = playerId;
        }

        private void ClearPlayerFromField(Field field)
        {
            if (field.PlayerId.HasValue)
            {
                Players[field.PlayerId.Value].Location = null;
                field.PlayerId = null;
            }
        }

        private void ClearPieceFromField(TaskField taskField)
        {
            if (taskField.PieceId.HasValue) taskField.PieceId = null;
        }

        private void HandlePlayerInField(Field field)
        {
            if (!field.PlayerId.HasValue) return;

            var player = Players[field.PlayerId.Value];
            if (player.Location == null)
            {
                player.Location = new Location(field.X, field.Y);
            }
            else
            {
                var oldPlayerField = this[player.Location];

                if (field.IsNewerThan(oldPlayerField))
                {
                    oldPlayerField.PlayerId = null;
                    player.Location = new Location(field.X, field.Y);
                }
                else
                {
                    field.PlayerId = null;
                }
            }
        }

        private void HandlePieceInField(int playerId, TaskField taskField)
        {
            if (!taskField.PieceId.HasValue) return;

            var oldPieceField = FindFieldWithPiece(taskField.PieceId.Value);
            if (oldPieceField != null)
                if (taskField.IsNewerThan(oldPieceField))
                    oldPieceField.PieceId = null;
                else
                    taskField.PieceId = null;

            //place piece
            if (Players[playerId].Piece != null && taskField.PieceId == Players[playerId].Piece.Id)
                Players[playerId].Piece = null;
        }

        private TaskField FindFieldWithPiece(int pieceId)
        {
            TaskField result = null;
            for (var i = 0; i < Width; ++i)
            for (var j = GoalAreaSize; j < TaskAreaSize + GoalAreaSize; ++j)
            {
                var field = Content[i, j];
                if (!IsLocationInTaskArea(field)) continue;

                var taskFiled = (TaskField) field;
                if (taskFiled.PieceId != pieceId) continue;

                result = taskFiled;
                break;
            }

            return result;
        }

        public override BoardData ToBoardData(int senderId, int receiverId)
        {
            var taskFields = ToEnumerable().Where(f => f is TaskField taskField && taskField.DistanceToPiece != -1)
                .Select(t => (TaskField) t);
            var goalFields = ToEnumerable().Where(f => f is GoalField goalField).Select(t => (GoalField) t);

            return BoardData.Create(receiverId, taskFields.ToArray(), goalFields.ToArray());
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            reader.ReadEndElement();
        }
    }
}