using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Proj.Models
{
    public class GroupFile
    {
        //Primary Key
        [Key]
        public int groupFileId { get; set;}  
        

        //Connection
        public int groupId  { get; set;}             // FK to Group
        [ForeignKey("groupId")]
        public virtual Group CurrentGroup { get; set; }
        
        public int fileId { get; set; }               // FK to FileCatalogue   
        [ForeignKey("fileId")]
        public virtual File CurrentFile { get; set; }
  

    }
}