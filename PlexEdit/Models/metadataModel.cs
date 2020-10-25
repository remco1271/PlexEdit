using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexEdit.Models
{
    public class metadataModel
    {
        public int id { get; set; }

        public int library_section_id { get; set; }

        public int parent_id { get; set; }

        public int metadata_type { get; set; }

        public string guid { get; set; }

        public int media_item_count { get; set; }

        public string title { get; set; }

        public string title_sort { get; set; }

        public string original_title { get; set; }

        public string studio { get; set; }

        public float rating { get; set; }

        public int rating_count { get; set; }

        public string tagline { get; set; }

        public string summary { get; set; }

        public string trivia { get; set; }

        public string quotes { get; set; }

        public string content_rating { get; set; }

        public int content_rating_age { get; set; }

        public int index { get; set; }

        public int absolute_index { get; set; }

        public int duration { get; set; }

        public string user_thumb_url { get; set; }

        public string user_art_url { get; set; }

        public string user_banner_url { get; set; }

        public string user_music_url { get; set; }

        public string user_fields { get; set; }

        public string tags_genre { get; set; }

        public string tags_collection { get; set; }

        public string tags_director { get; set; }

        public string tags_writer { get; set; }

        public string tags_star { get; set; }

        public DateTime originally_available_at { get; set; }

        public DateTime available_at { get; set; }

        public DateTime expires_at { get; set; }

        public DateTime refreshed_at { get; set; }

        public int year { get; set; }

        public DateTime added_at { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public DateTime deleted_at { get; set; }

        public string tags_country { get; set; }

        public string extra_data { get; set; }

        public string hash { get; set; }

        public float audience_rating { get; set; }

        public int changed_at { get; set; }

        public int resources_changed_at { get; set; }

        public int remote { get; set; }
    }
}
