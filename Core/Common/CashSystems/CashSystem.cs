using System;
using System.Collections.Generic;

namespace Sisyphus.CashSystems
{
    public struct CashStruct
    {
        public const byte ExpireTime = 15;
        public DateTime Time; public object CashValue;
    }

    public class CashSystem : Dictionary<string, CashStruct>
    {
        public object GetCashedValue(string propertyName, Func<object> method)
        {
            CashStruct currentCashStruct;
            if (TryGetValue(propertyName, out currentCashStruct) &&
                DateTime.Now <= currentCashStruct.Time.AddSeconds(CashStruct.ExpireTime))
                return currentCashStruct.CashValue;
            currentCashStruct.Time = DateTime.Now;
            currentCashStruct.CashValue = method();
            if (ContainsKey(propertyName)) Remove(propertyName);
            Add(propertyName, currentCashStruct);
            return currentCashStruct.CashValue;
        }
        public void FlushCash() { Clear(); }
    }
}