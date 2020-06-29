using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCRBattleRecorder;
using PCRBattleRecorder.Config;
using OpenCvSharp;
using OpenCvSize = OpenCvSharp.Size;

namespace PCRBattleRecorder.PCRModel
{
    public class PCRUnit
    {

        public static List<int> GetAllUnitIds()
        {
            var r = new List<int>();
            return r;
        }
        
        public static PCRUnit FromUnitId(int unitId)
        {
            return new PCRUnit(unitId);
        }

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();

        private PCRUnit(int unitId)
        {
            Id = unitId;
        }

        public int Id { get; }

        public int Rank { get; }

        public int Stars { get; } = 3;

        public PCRAvatarLevel AvatarLevel
        {
            get
            {
                if (Stars >= PCRAvatarLevel.Level6.GetRequiredStars()) return PCRAvatarLevel.Level6;
                if (Stars >= PCRAvatarLevel.Level3.GetRequiredStars()) return PCRAvatarLevel.Level3;
                return PCRAvatarLevel.Level1;
            }
        }

        public string AvatarName
        {
            get { return $"icon_unit_{Id}{AvatarLevel.GetRequiredStars()}1.png"; }
        }


        public string GetAvatarPath()
        {
            var path = pcrTools.ChooseFilePath("Img/unit", null, AvatarName);
            return path;
        }

        public Mat GetAvatar()
        {
            var path = GetAvatarPath();
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetResizedAvatar()
        {
            var avatarTemplateSize = configMgr.UnitAvatarTemplateSize;
            return GetResizedAvatar(avatarTemplateSize.Width);
        }

        public Mat GetResizedAvatar(int templateWidth)
        {
            var avatarResSize = configMgr.UnitAvatarResourceSize;

            var viewportSize = pcrTools.GetViewportSize();
            var viewportTemplateSize = configMgr.MumuViewportTemplateSize;

            var viewportScale = 1.0 * viewportSize.Width / viewportTemplateSize.Width;
            var avatarScale = 1.0 * templateWidth / avatarResSize.Width * viewportScale;

            var mat = GetAvatar();
            var resized = mat.Resize(new OpenCvSize(mat.Width * avatarScale, mat.Height * avatarScale));
            return resized;
        }
    }

    public enum PCRAvatarLevel
    {
        Level1,
        Level3,
        Level6,
    }

    public static class PCRAvatarLevelExtension
    {
        public static int GetRequiredStars(this PCRAvatarLevel avatarLevel)
        {
            switch (avatarLevel)
            {
                case PCRAvatarLevel.Level1: return 1;
                case PCRAvatarLevel.Level3: return 3;
                case PCRAvatarLevel.Level6: return 6;
            }
            return 0;
        }
    }
}
