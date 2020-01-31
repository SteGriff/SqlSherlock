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
    }
}
