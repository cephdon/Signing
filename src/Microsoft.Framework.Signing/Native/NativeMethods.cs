﻿using System;
using System.Runtime.InteropServices;

namespace Microsoft.Framework.Signing.Native
{
    internal static class NativeMethods
    {

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380228(v=vs.85).aspx
        [DllImport("Crypt32.dll", SetLastError = true)]
        public static extern SafeCryptMsgHandle CryptMsgOpenToDecode(
            CMSG_ENCODING dwMsgEncodingType,
            CMSG_OPENTODECODE_FLAGS dwFlags,
            uint dwMsgType,
            IntPtr hCryptProv,
            IntPtr pRecipientInfo,
            IntPtr pStreamInfo);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380219(v=vs.85).aspx
        [DllImport("Crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgClose(IntPtr hCryptMsg);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380231(v=vs.85).aspx
        [DllImport("Crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgUpdate(
            SafeCryptMsgHandle hCryptMsg,
            byte[] pbData,
            uint cbData,
            bool fFinal);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380227(v=vs.85).aspx
        [DllImport("Crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgGetParam(
            SafeCryptMsgHandle hCryptMsg,
            CMSG_GETPARAM_TYPE dwParamType,
            uint dwIndex,
            IntPtr pvData,
            ref uint pcbData);
    }

    internal enum CMSG_GETPARAM_TYPE : uint
    {
        // Source: wincrypt.h
        CMSG_TYPE_PARAM = 1,
        CMSG_CONTENT_PARAM = 2,
        CMSG_BARE_CONTENT_PARAM = 3,
        CMSG_INNER_CONTENT_TYPE_PARAM = 4,
        CMSG_SIGNER_COUNT_PARAM = 5,
        CMSG_SIGNER_INFO_PARAM = 6,
        CMSG_SIGNER_CERT_INFO_PARAM = 7,
        CMSG_SIGNER_HASH_ALGORITHM_PARAM = 8,
        CMSG_SIGNER_AUTH_ATTR_PARAM = 9,
        CMSG_SIGNER_UNAUTH_ATTR_PARAM = 10,
        CMSG_CERT_COUNT_PARAM = 11,
        CMSG_CERT_PARAM = 12,
        CMSG_CRL_COUNT_PARAM = 13,
        CMSG_CRL_PARAM = 14,
        CMSG_ENVELOPE_ALGORITHM_PARAM = 15,
        CMSG_RECIPIENT_COUNT_PARAM = 17,
        CMSG_RECIPIENT_INDEX_PARAM = 18,
        CMSG_RECIPIENT_INFO_PARAM = 19,
        CMSG_HASH_ALGORITHM_PARAM = 20,
        CMSG_HASH_DATA_PARAM = 21,
        CMSG_COMPUTED_HASH_PARAM = 22,
        CMSG_ENCRYPT_PARAM = 26,
        CMSG_ENCRYPTED_DIGEST = 27,
        CMSG_ENCODED_SIGNER = 28,
        CMSG_ENCODED_MESSAGE = 29,
        CMSG_VERSION_PARAM = 30,
        CMSG_ATTR_CERT_COUNT_PARAM = 31,
        CMSG_ATTR_CERT_PARAM = 32,
        CMSG_CMS_RECIPIENT_COUNT_PARAM = 33,
        CMSG_CMS_RECIPIENT_INDEX_PARAM = 34,
        CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM = 35,
        CMSG_CMS_RECIPIENT_INFO_PARAM = 36,
        CMSG_UNPROTECTED_ATTR_PARAM = 37,
        CMSG_SIGNER_CERT_ID_PARAM = 38,
        CMSG_CMS_SIGNER_INFO_PARAM = 39,
    }

    [Flags]
    internal enum CMSG_OPENTODECODE_FLAGS : uint
    {
        // Source: wincrypt.h
        None = 0,

        CMSG_DETACHED_FLAG = 0x00000004,
        CMSG_CRYPT_RELEASE_CONTEXT_FLAG = 0x00008000
    }

    [Flags]
    internal enum CMSG_ENCODING : uint
    {
        // Source: wincrypt.h
        X509_ASN_ENCODING = 0x00000001,
        PKCS_7_NDR_ENCODING = 0x00010000,

        Any = X509_ASN_ENCODING | PKCS_7_NDR_ENCODING
    }

    public class SafeCryptMsgHandle : SafeHandle
    {
        private SafeCryptMsgHandle() : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true) { }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CryptMsgClose(handle);
        }
    }
}