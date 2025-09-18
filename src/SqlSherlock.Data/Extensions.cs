using System.Text.Json;

namespace SqlSherlock.Data
{
    public static class Extensions
    {
        public static string StripSqlCommentMarkers(this string sqlLine)
        {
            string pureLine = sqlLine
                .Replace(SqlSyntax.START_COMMENT, "")
                .Replace(SqlSyntax.END_COMMENT, "")
                .Replace("*", "")
                .Trim();

            if (pureLine.StartsWith(SqlSyntax.INLINE_COMMENT))
            {
                pureLine = pureLine
                    .Substring(2)
                    .Trim();
            }

            return pureLine;
        }

        public static object ToValue(this JsonElement jsonElement)
        {
            return jsonElement.ValueKind switch
            {
                JsonValueKind.String => jsonElement.GetString() ?? "",
                JsonValueKind.Number => jsonElement.GetDecimal(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => jsonElement.GetRawText()
            };
        }
    }
}
