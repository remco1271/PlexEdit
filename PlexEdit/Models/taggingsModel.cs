using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexEdit.Models
{
    class taggingsModel
    {
        //Bevat alle verwijzingen van tags naar videos

        public int id { get; set; }
        public int metadata_item_id { get; set; }
        public int tag_id { get; set; }
        public int index { get; set; }
        public string text { get; set; }
        public int time_offset { get; set; }
        public string thumb_url { get; set; }
        public DateTime created_at { get; set; }
        public string extra_data { get; set; }
    }
}
