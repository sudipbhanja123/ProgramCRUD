using System.Drawing;
using System.Drawing.Imaging;

namespace ProgramCRUD.Model
{
    public class ProgramModel
    {
        public Guid? id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Skills { get; set; }

        public string? Benifits { get; set; }
        public string? Criteria { get; set; }
        public string? Type { get; set; }
        public string? Start { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public int Duration { get; set; }
        public string? Location { get; set; }
        public string? Qualification { get; set; }

        public int MaxNumOfApplications { get; set; }
        public Image CoverPhoto { get; set; }

        public String IpPhoto { get; set; }

    }
}
