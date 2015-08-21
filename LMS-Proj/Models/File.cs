using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Proj.Models
{
    public class File
    {
        //Primary Key
        [Key]
        public int FileId { get; set; }     

        //Attributes

        [Display(Name = "Type")]
        public FileType Type { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Submission Date/Time")]
        public DateTime SubmissionDate { get; set; }

        [Display(Name = "File Name")]        
        public string FileName { get; set; }  // shortname on filesystem

        [Display(Name = "File Path")]        
        public string FilePath { get; set; }  // basepath on filesystem

        [Display(Name = "File Link")]       
        public string FileLink { get; set; }  // http/https url
        
        [Display(Name = "Comments")]        
        public string Comment { get; set; }
        

        //Visar att filen är läst eller oläst
        [Display(Name = "Read")]
        public bool ReadByReciever { get; set; }

      
        //public string uploadFile
        //{
        //    get { return FilePath.Replace(" ", string.Empty) + ".pdf"; }
        //}



        // Connection

 //       [ForeignKey("ApplicationUser")]
        //public string UserId { get; set; }

                                             
        public ApplicationUser Receiver { get; set; }  // Receiver of Comment FK

        [ForeignKey("Owner")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser Owner { get; set; }

        public int? GroupId { get; set; }

//        public Group Groups { get; set; }

       
        public virtual ICollection<Group> Groups {get; set;}

        public virtual ICollection<Activity> Activities { get; set; }
      
    }

    public enum FileType
    {
        Exercise,
        Lesson,
        Comment,
        SharedFile,
        Etc
    }
}