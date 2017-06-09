using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace Sisyphus.Common
{
    public static class UacHelper
    {
        private const string UacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        private const string UacRegistryValue = "EnableLUA";

        private const uint StandardRightsRead = 0x00020000;
        private const uint TokenQuery = 0x0008;
        private static readonly uint TokenRead = (StandardRightsRead | TokenQuery);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool GetTokenInformation(IntPtr tokenHandle, TOKEN_INFORMATION_CLASS tokenInformationClass, IntPtr tokenInformation, uint tokenInformationLength, out uint returnLength);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUiAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        private enum TokenElevationType
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        public static bool IsUacEnabled
        {
            get
            {
                var uacKey = Registry.LocalMachine.OpenSubKey(UacRegistryKey, false);
                var result = uacKey != null && uacKey.GetValue(UacRegistryValue).Equals(1);
                return result;
            }
        }

        public static bool IsProcessElevated
        {
            get
            {
                if (IsUacEnabled)
                {
                    IntPtr tokenHandle;
                    if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TokenRead, out tokenHandle))
                    {
                        throw new ApplicationException("Could not get process token.  Win32 Error Code: " + Marshal.GetLastWin32Error());
                    }

                    var elevationResult = TokenElevationType.TokenElevationTypeDefault;

                    var elevationResultSize = Marshal.SizeOf((int)elevationResult);
                    uint returnedSize;
                    var elevationTypePtr = Marshal.AllocHGlobal(elevationResultSize);

                    var success = GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevationTypePtr, (uint)elevationResultSize, out returnedSize);
                    if (success)
                    {
                        elevationResult = (TokenElevationType)Marshal.ReadInt32(elevationTypePtr);
                        var isProcessAdmin = elevationResult == TokenElevationType.TokenElevationTypeFull;
                        return isProcessAdmin;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to determine the current elevation.");
                    }
                }
                else
                {
                    var identity = WindowsIdentity.GetCurrent();
                    var principal = new WindowsPrincipal(identity);
                    var result = principal.IsInRole(WindowsBuiltInRole.Administrator);
                    return result;
                }
            }
        }
    }
}
