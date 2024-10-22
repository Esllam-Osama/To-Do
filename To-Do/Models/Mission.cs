﻿namespace To_Do.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string FileUrl { get; set; }
    }
}
