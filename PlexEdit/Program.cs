using Dapper;
using PlexEdit.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexEdit
{
    class Program
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static List<librarySelectModel> LoadLibrarySelect()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<librarySelectModel>("select * from library_sections", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<metadataModel> LoadMetadata()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<metadataModel>("select * from metadata_items", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<tagsModel> LoadTags()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<tagsModel>("select * from tags", new DynamicParameters());
                return output.ToList();
            }
        }
        public static List<taggingsModel> LoadTaggings()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<taggingsModel>("select * from taggings", new DynamicParameters());
                return output.ToList();
            }
        }


        public static void addTaggings(taggingsModel taggings)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                //INSERT INTO "main"."taggings"("id", "metadata_item_id", "tag_id", "index", "text", "time_offset", "end_time_offset", "thumb_url", "created_at", "extra_data") VALUES (1000, 18, 746, 16, '', NULL, NULL, '', '2020-05-22 15:22:47', '');
                //cnn.Execute("insert into taggings (metadata_item_id, tag_id, [index], text, time_offset, end_time_offset, thumb_url, created_at, extra_data) values ( @metadata_item_id, @tag_id, @index, @text, @time_offset, @end_time_offset, @thumb_url, @created_at, @extra_data)", taggings);
                cnn.Execute("insert into taggings (metadata_item_id, tag_id, [index], text, thumb_url, created_at, extra_data) values ( @metadata_item_id, @tag_id, @index, @text, @thumb_url, @created_at, @extra_data)", taggings);
            }
        }
        public static void addTag(tagsModel tag)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                //INSERT INTO "main"."tags"("id", "metadata_item_id", "tag", "tag_type", "user_thumb_url", "user_art_url", "user_music_url", "created_at", "updated_at", "tag_value", "extra_data", "key", "parent_id") VALUES (6179, NULL, 'Grace', 4, '', '', '', '2020-10-24 18:27:03', '2020-10-24 18:27:03', NULL, '', '', NULL);
                cnn.Execute("insert into tags (tag, tag_type, user_thumb_url, user_art_url, user_music_url, created_at, updated_at, extra_data, key, parent_id) values (@tag, @tag_type, @user_thumb_url, @user_art_url, @user_music_url, @created_at, @updated_at, @extra_data, @key, @parent_id)", tag);
            }
        }

        static void createTaggings(metadataModel item, tagsModel tagged)
        {
            //search for tagging
            List<taggingsModel> taggings = LoadTaggings();
            taggings = taggings.FindAll(x => x.tag_id.Equals(tagged.id));
            //search for already added to taggings
            int taggingIndex = taggings.FindIndex(x => x.metadata_item_id.Equals(item.id));
            //findindex example from ms
            if (taggingIndex >= 0)
            {
                //found
                Console.WriteLine("Found a tagging at index {0}", taggings[taggingIndex].id);
            }
            else
            {
                Console.WriteLine("no taggings have been found for \"{0}\" with name {1}", item.tags_director, item.title);
                //create tagging index
                taggingsModel newTagging = new taggingsModel();
                newTagging.metadata_item_id = item.id;
                newTagging.tag_id = tagged.id;
                newTagging.index = 0;
                newTagging.text = "";
                newTagging.thumb_url = string.Empty;
                //create the time for Plex without ms
                newTagging.created_at = DateTime.Parse(
                                                        DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                                        System.Globalization.CultureInfo.CurrentCulture
                                                      );
                newTagging.extra_data = string.Empty;
                Console.WriteLine("Add new tagging");
                addTaggings(newTagging);
                Console.WriteLine("New tagging has been added!");
            }
        }
        static int tableWidth = 100;
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
        static int librarySelect = 0;
        static void Main(string[] args)
        {
            Console.Title = "Plex database tags fixer";
            Console.WriteLine("Plex database tags fixer");
            Console.WriteLine("Opening database file");

            //start select library
            Console.WriteLine("Found the following libraries in the database:");
            PrintLine();
            PrintRow("id", "Name", "ScannerType");
            PrintLine();
            foreach (librarySelectModel library in LoadLibrarySelect())
            {
                PrintRow(library.id.ToString(), library.name, library.scanner);
                //Console.WriteLine(library.id.ToString() + ": " + library.name + "\t\tScannerType: " + library.scanner);
            }
            PrintLine();
            Console.Write("Want to edit library id:");
            librarySelect = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("You have selected id:{0}", librarySelect.ToString());
            List<metadataModel> temp;
            temp = LoadMetadata();
            Console.WriteLine("Got some data!");

            Console.WriteLine("Loading the tags for type 4 database");
            List<tagsModel> allTags = LoadTags();
            allTags = allTags.FindAll(x => x.tag_type.Equals(4));


            Console.WriteLine("\nFind: Part where liberyId contains \"{1}\": {0}",
            temp.Find(x => x.library_section_id.Equals(librarySelect)).title, librarySelect.ToString());

            List<metadataModel> resultsMeta = temp.FindAll(x => x.library_section_id.Equals(librarySelect));
            foreach (metadataModel item in resultsMeta)
            {
                //filter the tags out of it.
                if(item.tags_director != string.Empty)
                {
                    //get all tags
                    String[] tags = System.Text.RegularExpressions.Regex.Split(item.tags_director, @"\|");
                    foreach(string tag in tags)
                    {
                        //search for existing tag
                        tagsModel tagged = allTags.Find(x => x.tag.Equals(tag));
                        if(allTags.FindIndex(x => x.tag.Equals(tag)) >= 0)
                        {
                            //search for tagging

                            createTaggings(item, tagged);
                        }
                        else
                        {
                            //add tag
                            Console.WriteLine("Building tag...");
                            tagsModel tagNew = new tagsModel();
                            tagNew.tag = tag;
                            tagNew.tag_type = 4;
                            tagNew.user_thumb_url = string.Empty;
                            tagNew.user_art_url = string.Empty;
                            tagNew.user_music_url = string.Empty;
                            tagNew.created_at = DateTime.Parse(
                                                                DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                                                System.Globalization.CultureInfo.CurrentCulture
                                                              );
                            tagNew.updated_at = tagNew.created_at;
                            tagNew.extra_data = string.Empty;
                            tagNew.key = string.Empty;


                            Console.WriteLine("Add new tag {0}", tag);
                            addTag(tagNew);
                            Console.WriteLine("New tag has been added!");

                            //rebuild tagging
                            allTags = LoadTags();
                            allTags = allTags.FindAll(x => x.tag_type.Equals(4));
                            tagged = allTags.Find(x => x.tag.Equals(tag));
                            //add tagging
                            createTaggings(item, tagged);
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("No director has been found for: {0}", item.title);
                }
                //Get the TAG id

                //notfound make new

                //save metadata id and tag id

                //next..
            }
            Console.WriteLine("");
            Console.WriteLine("Database it done, press enter to close.");
            Console.ReadLine();
        }
    }
}
