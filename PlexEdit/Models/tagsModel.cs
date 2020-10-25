using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexEdit.Models
{
    class tagsModel
    {
        //bevat alle tags voor de films
        public int id { get; set; }
        public int metadata_item_id { get; set; }
        public string tag { get; set; }
        public int tag_type { get; set; }
        public string user_thumb_url { get; set; }
        public string user_art_url { get; set; }
        public string user_music_url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int tag_value { get; set; }
        public string extra_data { get; set; }
        public string key { get; set; }
        public int parent_id { get; set; }
    }
}
