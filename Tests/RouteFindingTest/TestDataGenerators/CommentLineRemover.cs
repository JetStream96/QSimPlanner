using System;
using System.Text;

namespace Tests.RouteFindingTest.TestDataGenerators
{
    public static class CommentLineRemover
    {
        private static char COMMENT_CHAR = ';';

        public static string RemoveComments(string item)
        {
            var lines = item.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            var sb = new StringBuilder();

            foreach (var i in lines)
            {
                if ((i.Length != 0 && i[0] == COMMENT_CHAR) == false)
                {
                    sb.AppendLine(i);
                }
            }

            return sb.ToString();
        }

    }
}
