//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pet2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public Nullable<int> PetID { get; set; }
        public int CageID { get; set; }
        public System.DateTime StartsAt { get; set; }
        public System.DateTime EndsAt { get; set; }
        public bool IsPaid { get; set; }
        public int Duration { get; set; }
        public System.DateTime CreatedAt { get; set; }
    
        public virtual Cage Cage { get; set; }
        public virtual Client Client { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
