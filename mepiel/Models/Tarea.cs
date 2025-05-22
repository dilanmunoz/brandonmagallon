using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace mepiel.Models
{
    public class Tarea
    {
        [Display(Name = "#")]
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public byte prioridad { get; set; }
        public byte estado { get; set; }
        public DateTime registro { get; set; }
        [Display(Name = "Vencimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime vencimiento { get; set; }                

    }
}