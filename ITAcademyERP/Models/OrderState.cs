using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public enum OrderState
    {
        [EnumMember(Value = "Pendent de tractar")]
        PendentTractar,
        [EnumMember(Value = "En tractament")]
        EnTractament,
        [EnumMember(Value = "En repartiment")]
        EnRepartiment,
        [EnumMember(Value = "Completada")]
        Completada,
        [EnumMember(Value = "Cancel·lada")]
        Cancelada
    }
}
