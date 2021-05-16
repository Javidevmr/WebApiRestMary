using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApiRestC.Models
{
    public partial class Eventos
    {
        public int Id { get; set; }
        public string Evento { get; set; }
        public string Detalle { get; set; }
        public int EsError { get; set; }
        public DateTime? FechayHora { get; set; }
    }
}
