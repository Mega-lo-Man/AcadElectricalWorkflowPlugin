﻿using Autodesk.AutoCAD.Runtime;
using System;
using System.IO;
using AutocadCommands.Services;
using Autodesk.AutoCAD.ApplicationServices.Core;

namespace AutocadCommands
{
    public class Commands
    {
        private readonly string _configFilePath = Environment.CurrentDirectory + "\\config.txt";
        private ConfigProvider _configProvider;

        // Renumber terminals TERMXX properties.
        [CommandMethod("TERMCOUNT")]
        public void TerminalsRenumber()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var tRenumber = new TermRenumber(doc);
            if (tRenumber.Init())
            {
                tRenumber.Run();
            }
        }

        // Change the color of the terminals according to their purpose. 
        [CommandMethod("TERMCOLOR", CommandFlags.Modal)]
        public void TerminalsColorAutoReplace()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            if (!File.Exists(_configFilePath))
            {
                doc.Editor.WriteMessage("Config file (" + _configFilePath + ") not found.");
                return;
            }

            _configProvider = new ConfigProvider(_configFilePath);
            var tColorReplacer = new TermColorReplacer(doc, _configProvider);
            if (tColorReplacer.Init())
            {
                tColorReplacer.Run();
            }
            
        }

        // Advanced DESC1 increment of terminals
        [CommandMethod("TERMINCREMENT")]
        public void TerminalsDescriptionIncrement()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var tDescInc = new TermDescriptionIncrement(doc);
            if (tDescInc.Init())
            {
                tDescInc.Run();
            }
            
        }

        // Advanced blocks increment
        [CommandMethod("BINCREMENT")]
        public void BlocksAttributeIncrement()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var tBlocksInc = new BlocksAttributeIncrementer(doc);
            if (tBlocksInc.Init())
            {
                tBlocksInc.Run();
            }

        }

        // Add prefix to DESC1 of terminals
        [CommandMethod("TERMADDPREFIX")]
        public void TerminalsAddDescriptionPrefix()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var tDescInc = new TermAddDescriptionPrefix(doc);
            if (tDescInc.Init())
            {
                tDescInc.Run();
            }
            
        }

        // Find and replace part of DESC1 of terminals
        [CommandMethod("TERMFINDREPLACE")]
        public void TerminalsFindAndReplace()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var tDescInc = new TermFindAndReplace(doc);
            if (tDescInc.Init())
            {
                tDescInc.Run();
            }
        }

        // Set attribute value in selected blocks (default: "CABLEDESIGNATION")
        [CommandMethod("TERMSETATTRIBUTE")]
        public void SetBlocksAttribute()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var blockAttributeReplacer = new BlocksAttributeReplacer(doc);
            if (blockAttributeReplacer.Init())
            {
                blockAttributeReplacer.Run();
            }
        }

        // Link all wires
        [CommandMethod("LINKWIRES", CommandFlags.Session | CommandFlags.UsePickSet | CommandFlags.Redraw | CommandFlags.Modal)]
        public void LinkWires()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;

            var wiresLinker = new WiresLinker(doc);
            if (wiresLinker.Init())
            {
                wiresLinker.Run();
            }
        }

        
    }
}
