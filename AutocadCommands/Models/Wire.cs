﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutocadCommands.Models.IAutocadDirectionEnum;

namespace AutocadCommands.Models
{
    internal class Wire
    {
        public Entity WireEntity { get; set; }

        public Direction Direction { get; set; }

        public Point3d PointConnectedToMultiWire { get; set; }

        public Point3d TextCoordinate 
        { 
            get
            {
                var x = PointConnectedToMultiWire.X;
                var y = PointConnectedToMultiWire.Y;
                var space = 1.0;

                switch (Direction)
                {
                    case Direction.Above:
                        return new Point3d(x - space, y + space, 0);

                    case Direction.Left:
                        return new Point3d(x - space, y + space, 0);

                    case Direction.Below:
                        return new Point3d(x - space, y - space, 0);

                    case Direction.Right:
                        return new Point3d(x + space, y + space, 0);

                    default: return new Point3d(x - space, y + space, 0);

                }
            } 
             
        }
        
    }
}
