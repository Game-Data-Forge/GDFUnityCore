#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_ANDROID
#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj CloudUrlTools.cs create at 2025/05/20 10:05:01
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GDFFoundation
{
    public class CountryConverter : JsonConverter<Country>
    {
        public override Country Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return (Country)reader.GetInt16();
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string value = reader.GetString();

            if (short.TryParse(value, out var shortValue))
            {
                return (Country)shortValue;
            }

            return Enum.Parse<Country>(value);
        }

        public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToCodeString());
        }

        public override Country ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return (Country)reader.GetInt16();
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string value = reader.GetString();

            if (short.TryParse(value, out var shortValue))
            {
                return (Country)shortValue;
            }

            return Enum.Parse<Country>(value);
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] Country value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(value.ToCodeString());
        }
    }
}

#endif