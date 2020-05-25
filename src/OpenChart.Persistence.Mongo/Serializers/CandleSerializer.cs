using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using OpenChart.Domain.Entities;

namespace OpenChart.Persistence.Serializers
{
    public class CandleSerializer : IBsonSerializer<CandleDto>
    {
        private readonly GuidSerializer _guidSerializer;

        public CandleSerializer()
        {
            _guidSerializer = new GuidSerializer();
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CandleDto value)
        {
            context.Writer.WriteStartDocument();

            context.Writer.WriteName("Id");
            _guidSerializer.Serialize(context, args, value.Id);

            context.Writer.WriteName("o");
            context.Writer.WriteDecimal128(new Decimal128(value.Open));

            context.Writer.WriteName("c");
            context.Writer.WriteDecimal128(new Decimal128(value.Close));

            context.Writer.WriteName("h");
            context.Writer.WriteDecimal128(new Decimal128(value.High));

            context.Writer.WriteName("l");
            context.Writer.WriteDecimal128(new Decimal128(value.Low));

            context.Writer.WriteName("v");
            context.Writer.WriteDecimal128(new Decimal128(value.Volume));

            context.Writer.WriteName("t");
            context.Writer.WriteInt64(value.Date);

            context.Writer.WriteEndDocument();
        }

        public CandleDto Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var dto = new CandleDto();

            context.Reader.ReadStartDocument();

            dto.Id = _guidSerializer.Deserialize(context, args);
            dto.Open = (decimal)context.Reader.ReadDecimal128();
            dto.Close = (decimal)context.Reader.ReadDecimal128();
            dto.High = (decimal)context.Reader.ReadDecimal128();
            dto.Low = (decimal)context.Reader.ReadDecimal128();
            dto.Volume = (decimal)context.Reader.ReadDecimal128();
            dto.Date = context.Reader.ReadInt64();

            context.Reader.ReadEndDocument();

            return dto;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is CandleDto candleDto)
                Serialize(context, args, candleDto);
        }

        public Type ValueType => typeof(CandleDto);
    }
}