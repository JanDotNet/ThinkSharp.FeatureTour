// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace ThinkSharp.FeatureTouring
{
    internal static class PlacementHelper
    {
        // distance from popup corner to the center of the popup arrow
        private const int MARGIN = 30;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left; // X coordinate of top left point
            public int Top; // Y coordinate of top left point
            public int Right; // X coordinate of bottom right point
            public int Bottom; // Y coordinate of bottom right point
        }

        public static Placement GetActualPlacement(this Popup popup, Placement placement)
        {
            if (popup == null || !popup.IsOpen || popup.PlacementTarget == null)
                return placement;

            var child = popup.Child;
            if (child == null) return placement;

            var hwndSource = PresentationSource.FromVisual(child) as HwndSource;
            if (hwndSource == null) return placement;

            // If desired placement does not fir on screen, another placement will be used. That placement will be determined by popup automatically.
            IntPtr popupHandle = hwndSource.Handle;
            //IntPtr tagetHandle = (PresentationSource.FromVisual(popup.PlacementTarget) as HwndSource).Handle;

            var popupRect = new RECT();
            //var targetRect = new RECT();
            GetWindowRect(popupHandle, out popupRect);
            //GetWindowRect(tagetHandle, out targetRect);
// EXCEPTION HERE!!!
            var targetPoint = popup.PlacementTarget.PointToScreen(new Point(0, 0));
            var dx = targetPoint.X - popupRect.Left;
            var dy = popupRect.Top - targetPoint.Y;
            switch (placement)
            {
                // Bottom
                case Placement.BottomCenter:
                    if (dy < 0) return Placement.TopCenter;
                    break;
                case Placement.BottomLeft:
                    if (dy < 0) return Placement.TopLeft;
                    break;
                case Placement.BottomRight:
                    if (dy < 0) return Placement.TopRight;
                    break;

                // Top
                case Placement.TopCenter:
                    if (dy > 0) return Placement.BottomCenter;
                    break;
                case Placement.TopLeft:
                    if (dy > 0) return Placement.BottomLeft;
                    break;
                case Placement.TopRight:
                    if (dy > 0) return Placement.BottomRight;
                    break;

                // Left
                case Placement.LeftTop:
                    if (dx < 0) return Placement.RightTop;
                    break;
                case Placement.LeftCenter:
                    if (dx < 0) return Placement.RightCenter;
                    break;
                case Placement.LeftBottom:
                    if (dx < 0) return Placement.RightBottom;
                    break;

                // Right
                case Placement.RightTop:
                    if (dx > 0) return Placement.LeftTop;
                    break;
                case Placement.RightCenter:
                    if (dx > 0) return Placement.LeftCenter;
                    break;
                case Placement.RightBottom:
                    if (dx > 0) return Placement.LeftBottom;
                    break;
            }

            return placement;
        }

        public static CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Placement placement)
        {
            var placements = new List<Placement>();
            placements.Add(placement);

            switch (placement)
            {
                // Bottom
                case Placement.BottomCenter:
                    placements.Add(Placement.TopCenter);
                    break;
                case Placement.BottomLeft:
                    placements.Add(Placement.TopLeft);
                    break;
                case Placement.BottomRight:
                    placements.Add(Placement.TopRight);
                    break;

                // Top
                case Placement.TopCenter:
                    placements.Add(Placement.BottomCenter);
                    break;
                case Placement.TopLeft:
                    placements.Add(Placement.BottomLeft);
                    break;
                case Placement.TopRight:
                    placements.Add(Placement.BottomRight);
                    break;

                // Left
                case Placement.LeftTop:
                    placements.Add(Placement.RightTop);
                    break;
                case Placement.LeftCenter:
                    placements.Add(Placement.RightCenter);
                    break;
                case Placement.LeftBottom:
                    placements.Add(Placement.RightBottom);
                    break;

                // Right
                case Placement.RightTop:
                    placements.Add(Placement.LeftTop);
                    break;
                case Placement.RightCenter:
                    placements.Add(Placement.LeftCenter);
                    break;
                case Placement.RightBottom:
                    placements.Add(Placement.LeftBottom);
                    break;

                case Placement.Center:
                    placements.Add(Placement.LeftCenter);
                    placements.Add(Placement.TopCenter);
                    placements.Add(Placement.RightCenter);
                    placements.Add(Placement.BottomCenter);
                    break;
            }

            return GetCustomPopupPlacement(popupSize, targetSize, placements.ToArray()).ToArray();
        }

        private static IEnumerable<CustomPopupPlacement> GetCustomPopupPlacement(Size popupSize, Size targetSize, params Placement[] placements)
        {
            foreach (var placement in placements)
                yield return GetCustomPopupPlacement(popupSize, targetSize, placement);
        }

        private static CustomPopupPlacement GetCustomPopupPlacement(Size popupSize, Size targetSize, Placement placement)
        {
            switch (placement)
            {
                // Bottom
                case Placement.BottomCenter:
                    var x = (targetSize.Width - popupSize.Width) / 2;
                    var y = targetSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);
                case Placement.BottomLeft:
                    x = 0;
                    y = targetSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);
                case Placement.BottomRight:
                    x = targetSize.Width - popupSize.Width;
                    y = targetSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);

                // Top
                case Placement.TopCenter:
                    x = (targetSize.Width - popupSize.Width) / 2;
                    y = -popupSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);
                case Placement.TopLeft:
                    x = 0;
                    y = -popupSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);
                case Placement.TopRight:
                    x = targetSize.Width - popupSize.Width;
                    y = -popupSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Vertical);

                // Left
                case Placement.LeftTop:
                    x = -popupSize.Width;
                    y = targetSize.Height < (2 * MARGIN) ? (-MARGIN + targetSize.Height / 2) : 0;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
                case Placement.LeftCenter:
                    x = -popupSize.Width;
                    y = (targetSize.Height - popupSize.Height) / 2;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
                case Placement.LeftBottom:
                    x = -popupSize.Width;
                    y = targetSize.Height < (2 * MARGIN) ? (targetSize.Height - (popupSize.Height - MARGIN + targetSize.Height / 2)) : targetSize.Height - popupSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);

                // Right
                case Placement.RightTop:
                    x = targetSize.Width;
                    y = targetSize.Height < (2 * MARGIN) ? (-MARGIN + targetSize.Height / 2) : 0;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
                case Placement.RightCenter:
                    x = targetSize.Width;
                    y = (targetSize.Height - popupSize.Height) / 2;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
                case Placement.RightBottom:
                    x = targetSize.Width;
                    y = targetSize.Height < (2 * MARGIN) ? (targetSize.Height - (popupSize.Height - MARGIN + targetSize.Height / 2)) : targetSize.Height - popupSize.Height;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);

                // Center
                default:
                    x = (targetSize.Width - popupSize.Width) / 2;
                    y = (targetSize.Height - popupSize.Height) / 2;
                    return new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
            }
        }
    }
}
