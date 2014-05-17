/******************************************************************************\
* Copyright (C) 2012-2013 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
\******************************************************************************/
using System;
using System.Threading;
using Leap;
using System.Timers;
using APS.Data;

namespace APS.Fountain
{
    public class Sample
    {
        public static LeapListener Listener { get; set; }
        public static DateTime Start { get; set; }
        public static DateTime LastStart { get; set; }
        public static TimeSpan Max { get; set; }
        public static TimeSpan Min { get; set; }

        public static void Main()
        {
            using(var coord = new Coordinator())
            {
                coord.Run();
            }
        }
    }
}