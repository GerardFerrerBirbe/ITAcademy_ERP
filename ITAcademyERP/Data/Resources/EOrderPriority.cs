using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Resources
{
    public enum EOrderPriority
    {
        [EnumMember(Value = "Baixa")]
        Baixa,
        [EnumMember(Value = "Mitjana")]
        Mitjana,
        [EnumMember(Value = "Alta")]
        Alta
    }
}
