using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Common.SerializeHelper
{
    public static class ObjSerializer
    {
        public static TargetObject SerializeTo<TargetObject, SourceObject>(SourceObject obj)
        {
            return JsonConvert.DeserializeObject<TargetObject>(JsonConvert.SerializeObject(obj));
        }

        public static TargetObject SerializeTo<TargetObject>(object obj)
        {
            return JsonConvert.DeserializeObject<TargetObject>(JsonConvert.SerializeObject(obj));
        }
    }
}
