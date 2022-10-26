﻿using CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCommands.Models
{
    public static class NetTypeClassificator
    {
        private static readonly IEnumerable<string> _powerNegative = new List<string> { "0В", "GND", "ПИ-", "ПИ1-", "ПИ2-", "ПИ3-", "ПИ4-", "ПИ5-", "ПИ6-", "ПИ7-", "ПИ8-", "-U1", "-U2" };
        private static readonly IEnumerable<string> _powerPositive = new List<string> { "ПИ+", "ПИ1+", "ПИ2+", "ПИ3+", "ПИ4+", "ПИ5+", "ПИ6+", "ПИ7+", "ПИ8+", "+U1", "+U2"  };
        private static readonly IEnumerable<string> _rs485A = new List<string> { "RS485A", "RS485(A)", "A", "A1", "A2", "A3", "A4", "ЛС+" };
        private static readonly IEnumerable<string> _rs485B = new List<string> { "RS485B", "RS485(B)", "B", "B1", "B2", "B3", "B4", "ЛС-" };
        private static readonly IEnumerable<string> _rs485Gnd = new List<string> { "RS485GND", "RS485(GND)", "C", "C1", "C2", "C3", "C4" };
        private static readonly IEnumerable<string> _dplsPositive = new List<string> { "ДПЛС-1+", "ДПЛС-2+", "ДПЛС1+", "ДПЛС2+" };
        private static readonly IEnumerable<string> _dplsNegative = new List<string> { "ДПЛС-1-", "ДПЛС-2-", "ДПЛС1-", "ДПЛС2-" };

        public static bool IsPowerNegative(string terminalDescription)
        {
            if (terminalDescription.Count() <= 1)
                return false;

            if (_powerNegative.Any(o => o.StartsWith(terminalDescription)))
                return true;

            if (terminalDescription[0] == '-' && Mathematic.IsNumeric(terminalDescription[1].ToString()))
                return true;

            return false;
        }

        public static bool IsPowerPositive(string terminalDescription)
        {
            if (terminalDescription.Count() <= 1)
                return false;

            if(_powerPositive.Any(o => o.StartsWith(terminalDescription)))
                return true;

            if (terminalDescription[0] == '+' && Mathematic.IsNumeric(terminalDescription[1].ToString()))
                return true;

            return false;
        }
        public static bool IsRs485A(string terminalDescription) => _rs485A.Any(o => o.StartsWith(terminalDescription));
        public static bool IsRs485B(string terminalDescription) => _rs485B.Any(o => o.StartsWith(terminalDescription));
        public static bool IsRs485Gnd(string terminalDescription) => _rs485Gnd.Any(o => o.StartsWith(terminalDescription));
        public static bool IsDplsPositive(string terminalDescription) => _dplsPositive.Any(o => o.StartsWith(terminalDescription));
        public static bool IsDplsNegative(string terminalDescription) => _dplsNegative.Any(o => o.StartsWith(terminalDescription));

        public static NetTypes GetNetTypes(string description)
        {
            if(string.IsNullOrEmpty(description))
                return NetTypes.Unknown;
            if (IsPowerNegative(description))
                return NetTypes.PowerNegative; 
            if(IsPowerPositive(description)) 
                return NetTypes.PowerPositive;
            if (IsRs485A(description))
                return NetTypes.Rs485A;
            if (IsRs485B(description))
                return NetTypes.Rs485B;
            if (IsRs485Gnd(description))
                return NetTypes.Rs485Gnd;
            if (IsDplsPositive(description))
                return NetTypes.PowerPositive;
            if (IsDplsNegative(description))
                return NetTypes.DplsNegative;

            return NetTypes.Unknown;
        }
    }
}