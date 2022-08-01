﻿using AutocadCommands.Services;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using LinkCommands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCommands
{
    public class Commands
    {
        // Link all wires
        [CommandMethod("LINKMULTIWIRES", CommandFlags.UsePickSet | CommandFlags.Redraw | CommandFlags.Modal)]
        public void LinkMultiWires()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var wiresLinker = new MultiWiresLinker(doc);
            if (wiresLinker.Init())
            {
                wiresLinker.Run();
            }
        }

        // Link all wires
        [CommandMethod("LINKWIRES", CommandFlags.UsePickSet | CommandFlags.Redraw | CommandFlags.Modal)]
        public void LinkWires()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var wiresLinker = new WiresLinker(doc);
            if (wiresLinker.Init())
            {
                wiresLinker.Run();
            }
        }

        // Link all wires
        [CommandMethod("LINKPAIRMULTIWIRES", CommandFlags.UsePickSet | CommandFlags.Redraw | CommandFlags.Modal)]
        public void LinkPairMultiWires()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var wiresLinker = new PairMultiWiresLinker(doc);
            if (wiresLinker.Init())
            {
                wiresLinker.Run();
            }
        }
    }
}