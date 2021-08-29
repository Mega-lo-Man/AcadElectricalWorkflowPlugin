﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutocadCommands.Models;
using AutocadCommands.Utils;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutocadCommands.Services
{
    public class TermDescriptionIncrement
    {
        private readonly Editor _ed;
        private readonly Document _doc;
        private readonly Database _db;

        public TermDescriptionIncrement(Editor ed, Document doc, Database db)
        {
            _ed = ed;
            _doc = doc;
            _db = db;
        }

        public void Run()
        {
            #region Dialog with user

            var promptResult = _ed.GetString("\nEnter the initial character sequence: ");
            if (promptResult.Status != PromptStatus.OK)
                return;

            var startSequence = promptResult.StringResult;
            if (startSequence == null)
                return;

            promptResult = _ed.GetString("\nEnter the start number: ");
            if (promptResult.Status != PromptStatus.OK)
                return;
            if (!int.TryParse(promptResult.StringResult, out var startNumber))
                return;

            var filter = new SelectionFilter(
                new[]
                {
                    new TypedValue(0, "INSERT"), new TypedValue(2, "*T0002_*")
                });

            var opts = new PromptSelectionOptions
            {
                MessageForAdding = "Select block references: "
            };

            //Make the selection   
            var res = _ed.GetSelection(opts, filter);
            if (res.Status != PromptStatus.OK)
                return;

            #endregion
            
            var terminals = new List<Terminal>();

            // Lock the document
            using var acLckDoc = _doc.LockDocument();
            var objIds = new ObjectIdCollection(res.Value.GetObjectIds());

            using (var acTrans = _db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId blkId in objIds)
                {
                    terminals = TerminalsHelper.GetTerminals(acTrans, objIds, false);
                }

                acTrans.Commit();
            }

            using (var acTrans = _db.TransactionManager.StartTransaction())
            {
                IComparer<Terminal> comparer = new TerminalsComparer();
                terminals.Sort(comparer);
                AutoNumb(terminals, startSequence, startNumber);
                TerminalsHelper.SetTerminals(acTrans, objIds, terminals);
                acTrans.Commit();
            }
        }

        private void AutoNumb(IEnumerable<Terminal> terminals, string startSequence, int startNumber)
        {
            var isMinus = false;
            var counter = startNumber;

            foreach (var terminal in terminals)
            {
                var desc1 = terminal.Description1;
                if (!desc1.Contains(startSequence)) continue;

                if (startSequence.EndsWith("ШС") ||
                    startSequence.EndsWith("ШСi") ||
                    startSequence.EndsWith("КЦ") ||
                    startSequence.EndsWith("КЦi"))
                {
                    var insertIndex = desc1.LastIndexOf(startSequence, StringComparison.Ordinal) + startSequence.Length;
                    if (insertIndex < 0)
                        return;
                    var cutStr = desc1.Substring(0, insertIndex);
                    if (!isMinus)
                    {
                        terminal.Description1 = cutStr + counter.ToString() + "+";
                        isMinus = true; // the next step should be in (isMinus) section
                    }
                    else
                    {
                        terminal.Description1 = cutStr + counter.ToString() + "-";
                        isMinus = false; // the next step should be in (!isMinus) section
                        counter++;
                    }
                }
                else
                {
                    var insertIndex = desc1.IndexOf(startSequence, StringComparison.Ordinal) + startSequence.Length;
                    var desc1WithoutPrefix = desc1.Substring(insertIndex);
                    var countingNumbStr = GetFirstDigitsNumber(desc1WithoutPrefix);

                    var desc1Suffix = desc1WithoutPrefix.Substring(countingNumbStr.Length);

                    terminal.Description1 = startSequence + counter + desc1Suffix;
                    counter++;
                }
            }
        }

        private string GetFirstDigitsNumber(string desc1WithoutPrefix)
        {
            var numbStr = new StringBuilder();
            foreach (var ch in desc1WithoutPrefix)
            {
                if (char.IsDigit(ch)) numbStr.Append(ch);
                else
                {
                    break;
                }
            }

            return numbStr.ToString();
        }
        /*
        private int GetInt(string str)
        {
            var digits = new string(str.Where(char.IsDigit).ToArray()); //Get all digits from str

            if (int.TryParse(string.Join("", digits), out var intNum))
            {
                return intNum;
            }
            else
            {
                return -1;
            }
        }
        */
    }
}