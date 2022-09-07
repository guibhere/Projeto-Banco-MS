using System.Reflection;
using System.Runtime.CompilerServices;

namespace Extensions
{
    public static class MemberBaseExtension
    {
        public static string GetDeclaringName(this MethodBase methodBase, [CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}