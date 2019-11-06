//////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2019, National Instruments Corp.

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal static class ImaqdxEventManager
    {
        private const uint DefaultBufferInterval = 1;

        public static void InstallImageAcquiredEventHandler(ImaqdxSessionHandle session, ImaqdxFrameDoneEventHandler callback)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(callback != null, "The callback parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxRegisterFrameDoneEvent(session, DefaultBufferInterval, callback, IntPtr.Zero);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void InstallPnpEventHandler(ImaqdxSessionHandle session, ImaqdxPnpEvent pnpEvent, ImaqdxPnpEventHandler callback)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(callback != null, "The callback parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxRegisterPnpEvent(session, pnpEvent, callback, IntPtr.Zero);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }
    }
}
