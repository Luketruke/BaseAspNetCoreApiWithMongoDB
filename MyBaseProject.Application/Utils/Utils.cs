using System.Text.RegularExpressions;
using MyBaseProject.Domain.Exceptions;

namespace MyBaseProject.Application.Utils
{
    public static class Utils
    {
        private static readonly Regex ObjectIdRegex = new Regex("^[a-fA-F0-9]{24}$", RegexOptions.Compiled);

        public static void ValidateObjectId(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !ObjectIdRegex.IsMatch(id))
            {
                throw new InvalidIdFormatException(id);
            }
        }
    }
}