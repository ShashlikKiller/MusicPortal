using MusicPortal.Models.dbEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class UserProfile
    {
        public List<Composition> downloadedSongs { get; set; }
        public List<Composition> listenedSongs { get; set; }
        public List<Composition> favoriteSongs { get; set; }
        public List<MusicalGroup> favoriteGroups { get; set; }

        public UserProfile(List<Composition> downloaded = null, List<Composition> listened = null, List<Composition> favorite = null, List<MusicalGroup> groups = null)
        {
            downloadedSongs = downloaded;
            listenedSongs = listened;
            favoriteSongs = favorite;
            favoriteGroups = groups;

        }
    }
}