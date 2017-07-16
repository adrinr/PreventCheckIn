using System;

namespace PreventCheckinVsix
{
    internal sealed class PackageGuids
    {
        public const string guidPreventCheckinPackageString = "611c394d-527e-41d7-bca2-e5754b06000f";
        public static string guidPreventCheckinCmdSetString = "d111fe8e-8553-47e9-8f2b-ad8d069cb33d";
        public static Guid guidPreventCheckinPackage = new Guid(guidPreventCheckinPackageString);
        public static Guid guidPreventCheckinCmdSet = new Guid(guidPreventCheckinCmdSetString);
    }
    internal sealed class PackageIds
    {
        public const int AvoidCheckin = 0x0100;
    }
}