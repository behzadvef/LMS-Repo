using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Proj.Models
{
    public class Schedule
    {
        [Key]
        public int scheduleId { get; set; }
        [Display (Name ="room")]
        public string room { get; set; }

        [Display(Name="Title")]
        public string Title { get; set; }

        
        [DataType(DataType.Time)]
      // [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

    
        //[DisplayFormat(DataFormatString = "{h}", ApplyFormatInEditMode = true)]
         [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
      

        //Connection
        //FK
        public virtual ICollection<Activity> ListActivities { get; set; }

    }
}