﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Common.BoardObjects
{
    [DebuggerDisplay("[X = {X}, Y = {Y}], {PieceId}, {DistanceToPiece}")]
    public class TaskField : Field, IEquatable<TaskField>
    {
        protected TaskField()
        {
        }

        public TaskField(Location location, int distanceToPiece = -1, int? pieceId = null, int? playerId = null) : this(
            location, distanceToPiece, pieceId, playerId, DateTime.Now)
        {
        }

        public TaskField(Location location, int distanceToPiece, int? pieceId, int? playerId, DateTime date) : base(
            location, playerId, date)
        {
            DistanceToPiece = distanceToPiece;
            PieceId = pieceId;
        }

        public int DistanceToPiece { get; set; }
        public int? PieceId { get; set; }

        public int ManhattanDistanceTo(Location location)
        {
            return Math.Abs(X - location.X) + Math.Abs(Y - location.Y);
        }

        public override void ReadXml(XmlReader reader)
        {
            DistanceToPiece = int.Parse(reader.GetAttribute("distanceToPiece"));

            if (int.TryParse(reader.GetAttribute("pieceId"), out var pieceId))
                PieceId = pieceId;

            base.ReadXml(reader);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteAttributeString("distanceToPiece", DistanceToPiece.ToString());

            if (PieceId.HasValue)
                writer.WriteAttributeString("pieceId", PieceId.ToString());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TaskField);
        }

        public bool Equals(TaskField other)
        {
            return other != null &&
                   base.Equals(other) &&
                   DistanceToPiece == other.DistanceToPiece &&
                   EqualityComparer<int?>.Default.Equals(PieceId, other.PieceId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TaskField field1, TaskField field2)
        {
            return EqualityComparer<TaskField>.Default.Equals(field1, field2);
        }

        public static bool operator !=(TaskField field1, TaskField field2)
        {
            return !(field1 == field2);
        }
    }
}