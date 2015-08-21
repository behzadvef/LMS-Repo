using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Proj.Models
{
    public class Activity
    {
        //Primary Key
        [Key]
        public int ActivityId { get; set;}

        [Display(Name = "Name")]
        public string Name { get; set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
           

        // Connection
//        [ForeignKey("Group")]
        public int? GroupId { get; set;}

        public Group Groups { get; set; }


//        [ForeignKey("File")]  
        public int? FileId { get; set; }

        public  File Attachment { get; set; }

        //FK Schedule
        public int? scheduleId { get; set; }
        public Schedule TimeSheet { get; set; }
        //public Schedule room { get; set; }

    }
}