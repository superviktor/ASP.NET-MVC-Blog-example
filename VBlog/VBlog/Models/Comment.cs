//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VBlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public System.DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
