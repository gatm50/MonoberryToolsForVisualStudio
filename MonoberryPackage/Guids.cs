// Guids.cs
// MUST match guids.h
using System;

namespace Monoberry.MonoberryPackage
{
    static class GuidList
    {
        public const string guidMonoberryPackagePkgString = "7e77c0e4-4413-4aa6-a172-da7ac2ebd69e";
        public const string guidMonoberryPackageCmdSetString = "ff107d1b-2b07-4715-966b-352bd6f20b91";

        public static readonly Guid guidMonoberryPackageCmdSet = new Guid(guidMonoberryPackageCmdSetString);
    };
}