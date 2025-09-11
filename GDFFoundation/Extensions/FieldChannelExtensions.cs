#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj ChannelTool.cs create at 2025/09/01 11:09:08
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    public static class FieldChannelExtensions
    {
        public static void CheckChannelValidity(this IFieldChannel item)
        {
            if (item.Channel < GDFConstants.K_CHANNEL_MIN)
            {
                throw new FormatException($"The field `{nameof(IFieldChannel.Channel)}` must be a positive integer.");
            }

            if (item.Channel > GDFConstants.K_CHANNEL_MAX)
            {
                throw new FormatException($"The field `{nameof(IFieldChannel.Channel)}` must lower than 99.");
            }
        }
    }
}