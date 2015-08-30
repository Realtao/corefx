﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit;

public class CancelKeyPress
{
    private const int WaitFailTestTimeoutSeconds = 30;

    [Fact]
    public static void CanAddAndRemoveHandler()
    {
        ConsoleCancelEventHandler handler = (sender, e) =>
        {
            // We don't actually want to do anything here.  This will only get called on the off chance
            // that someone CTRL+C's the test run while the handler is hooked up.  This is just used to 
            // validate that we can add and remove a handler, we don't care about exercising it.
        };
        Console.CancelKeyPress += handler;
        Console.CancelKeyPress -= handler;
    }

    [Fact]
    [PlatformSpecific(PlatformID.AnyUnix)]
    public static void InvokingCancelKeyPressHandler()
    {
        // On Windows we could use GenerateConsoleCtrlEvent to send a ctrl-C to the process,
        // however that'll apply to all processes associated with the same group, which will
        // include processes like the code coverage tool when doing code coverage runs, causing
        // those other processes to exit.  As such, we test this only on Unix, where we can
        // send a SIGINT signal to this specific process only.

        using (var mres = new ManualResetEventSlim())
        {
            ConsoleCancelEventHandler handler = (sender, e) => {
                e.Cancel = true;
                mres.Set();
            };

            Console.CancelKeyPress += handler;
            try
            {
                Assert.Equal(0, kill(getpid(), SIGINT));
                Assert.True(mres.Wait(WaitFailTestTimeoutSeconds));
            }
            finally
            {
                Console.CancelKeyPress -= handler;
            }
        }
    }

    #region Unix Imports
    // P/Invokes included here rather than being pulled from Interop\Unix
    // to avoid platform-dependent includes in csproj
    [DllImport("libc", SetLastError = true)]
    private static extern int kill(int pid, int sig);

    [DllImport("libc")]
    private static extern int getpid();

    private const int SIGINT = 2;
    #endregion
}