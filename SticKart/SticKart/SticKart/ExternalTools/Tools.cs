﻿// <auto-generated />

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Kinect.Toolbox
{
    /// <summary>
    /// Defines helper tools and class extenstions for development with the Kinect sensor. 
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Converts a joint collection to a list of 2D vectors.
        /// </summary>
        /// <param name="joints">The joint collection.</param>
        /// <returns>The joint collection as list of 2D vectors.</returns>
        public static List<Vector2> ToListOfVector2(this JointCollection joints)
        {
            return joints.Select(j => j.Position.ToVector2()).ToList();
        }

        /// <summary>
        /// Converts a skeleton point to a 3D vector.
        /// </summary>
        /// <param name="joints">The skeleton point.</param>
        /// <returns>The skeleton point as a 3D vector.</returns>
        public static Vector3 ToVector3(this SkeletonPoint vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Converts a skeleton point to a 2D vector.
        /// </summary>
        /// <param name="joints">The skeleton point.</param>
        /// <returns>The skeleton point as a 2D vector.</returns>
        public static Vector2 ToVector2(this SkeletonPoint vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Writes a skeleton point to a binary writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="vector">The skeleton point.</param>
        public static void Write(this BinaryWriter writer, SkeletonPoint vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        /// <summary>
        /// Reads a skeleton point from a binary reader.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The skeleton point.</returns>
        public static SkeletonPoint ReadVector(this BinaryReader reader)
        {
            SkeletonPoint result = new SkeletonPoint
                                {
                                    X = reader.ReadSingle(),
                                    Y = reader.ReadSingle(),
                                    Z = reader.ReadSingle()
                                };

            return result;
        }

        /// <summary>
        /// Attempts to adjust the Kinect sensor angle, with error handling.
        /// </summary>
        /// <param name="camera">The kinect sensor.</param>
        /// <param name="angle">The adjustment angle.</param>
        /// <returns>A value indicating whether the angle was adjusted successfully or not.</returns>
        public static bool TrySetElevationAngle(this KinectSensor camera, int angle)
        {
            bool success = false;
            try
            {
                camera.ElevationAngle = angle;
                success = true;
            }
            catch { }
            return success;
        }

        /// <summary>
        /// Retrieves the current skeletons from the skeleton frame, with error handling.
        /// </summary>
        /// <param name="frame">The skeleton frame.</param>
        /// <param name="skeletons">The skeleton storage to write to.</param>
        public static void GetSkeletons(this SkeletonFrame frame, ref Skeleton[] skeletons)
        {
            if (frame == null)
                return;

            if (skeletons == null || skeletons.Length != frame.SkeletonArrayLength)
            {
                skeletons = new Skeleton[frame.SkeletonArrayLength];
            }
            frame.CopySkeletonDataTo(skeletons);
        }

        /// <summary>
        /// Retrieves the current skeletons from the skeleton frame, with error handling.
        /// </summary>
        /// <param name="frame">The skeleton frame.</param>
        /// <returns>The skeletons.</returns>
        public static Skeleton[] GetSkeletons(this SkeletonFrame frame)
        {
            if (frame == null)
                return null;

            var skeletons = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletons);

            return skeletons;
        }

        /// <summary>
        /// Maps a 3D skeleton point to a 2D vector.
        /// </summary>
        /// <param name="sensor">The Kinect sensor.</param>
        /// <param name="position">The skeleton point to map.</param>
        /// <param name="coordinateMapper">The coordinate mapper.</param>
        /// <returns>The 2D mapped position.</returns>
        public static Vector2 Convert(KinectSensor sensor, SkeletonPoint position, CoordinateMapper coordinateMapper)
        {
            float width = 0;
            float height = 0;
            float x = 0;
            float y = 0;

            if (sensor.ColorStream.IsEnabled)
            {
                var colorPoint = coordinateMapper.MapSkeletonPointToColorPoint(position, sensor.ColorStream.Format);
                x = colorPoint.X;
                y = colorPoint.Y;

                switch (sensor.ColorStream.Format)
                {
                    case ColorImageFormat.RawYuvResolution640x480Fps15:
                    case ColorImageFormat.RgbResolution640x480Fps30:
                    case ColorImageFormat.YuvResolution640x480Fps15:
                        width = 640;
                        height = 480;
                        break;
                    case ColorImageFormat.RgbResolution1280x960Fps12:
                        width = 1280;
                        height = 960;
                        break;
                }
            }
            else if (sensor.DepthStream.IsEnabled)
            {
                var depthPoint = coordinateMapper.MapSkeletonPointToDepthPoint(position, sensor.DepthStream.Format);
                x = depthPoint.X;
                y = depthPoint.Y;

                switch (sensor.DepthStream.Format)
                {
                    case DepthImageFormat.Resolution80x60Fps30:
                        width = 80;
                        height = 60;
                        break;
                    case DepthImageFormat.Resolution320x240Fps30:
                        width = 320;
                        height = 240;
                        break;
                    case DepthImageFormat.Resolution640x480Fps30:
                        width = 640;
                        height = 480;
                        break;
                }
            }
            else
            {
                width = 1;
                height = 1;
            }

            return new Vector2(x / width, y / height);
        }
    }
}
