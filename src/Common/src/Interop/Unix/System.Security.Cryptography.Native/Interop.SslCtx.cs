// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

internal static partial class Interop
{
    internal static partial class Ssl
    {
        internal delegate int AppVerifyCallback(IntPtr storeCtx, IntPtr arg);
        internal delegate int ClientCertCallback(IntPtr ssl, out IntPtr x509, out IntPtr pkey);

        [DllImport(Libraries.CryptoNative)]
        internal static extern SafeSslContextHandle SslCtxCreate(IntPtr method);

        [DllImport(Libraries.CryptoNative)]
        internal static extern void SslCtxDestroy(IntPtr ctx);

        [DllImport(Libraries.CryptoNative)]
        internal static extern void SslCtxSetCertVerifyCallback(SafeSslContextHandle ctx, AppVerifyCallback cb, IntPtr arg);

        [DllImport(Libraries.CryptoNative)]
        internal static extern void SslCtxSetClientCertCallback(SafeSslContextHandle ctx, ClientCertCallback callback);
    }
}

namespace Microsoft.Win32.SafeHandles
{
    internal sealed class SafeSslContextHandle : SafeHandle
    {
        private SafeSslContextHandle()
            : base(IntPtr.Zero, true)
        {
        }

        internal SafeSslContextHandle(IntPtr handle, bool ownsHandle)
            : base(handle, ownsHandle)
        {
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            Interop.Ssl.SslCtxDestroy(handle);
            SetHandle(IntPtr.Zero);
            return true;
        }
    }
}
