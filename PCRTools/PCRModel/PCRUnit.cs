using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCRBattleRecorder;
using PCRBattleRecorder.Config;
using PCRBattleRecorder.Csv;
using OpenCvSharp;
using OpenCvSize = OpenCvSharp.Size;

namespace PCRBattleRecorder.PCRModel
{
    public class PCRUnit
    {

        public static Csv.Csv GetUnitCsv()
        {
            return PCRTools.GetInstance().GetCsv("Unit");
        }

        public static string UnknownUnitID { get { return "1000"; } }

        public static List<string> GetAllUnitIDs()
        {
            var csv = GetUnitCsv();
            var keys = csv.GetKeys();
            return keys;
        }

        public static string GetUnitIDByUnitName(string unitName)
        {
            var csv = GetUnitCsv();
            var keys = csv.GetKeys();
            foreach (var key in keys)
            {
                var row = csv[key];
                var nickNames = row["NickNames"];
                if (nickNames.Contains(unitName))
                    return key;
            }
            throw new BreakException(Trans.T("找不到{0}的UnitID", unitName));
        }

        public static PCRUnit FromUnitName(string unitName, int stars)
        {
            var unitID = GetUnitIDByUnitName(unitName);
            return FromUnitID(unitID, stars);
        }

        public static PCRUnit FromUnitID(int unitID, int stars)
        {
            return FromUnitID(unitID.ToString(), stars);
        }

        public static PCRUnit FromUnitID(string unitID, int stars)
        {
            var unit = new PCRUnit()
            {
                ID = unitID,
                Name = GetUnitNameByID(unitID),
                Stars = stars
            };
            return unit;
        }

        public static PCRUnit FromUnitID(string unitID)
        {
            return FromUnitID(unitID, GetDefaultStars(unitID));
        }

        public static string GetUnitNameByID(string unitID)
        {
            var csv = GetUnitCsv();
            var nickNames = csv[unitID]["NickNames"];
            var i = nickNames.IndexOf(";");
            if (i != -1)
                return nickNames.Substring(0, i);
            else
                return nickNames;
        }

        public static PCRAvatarLevel GetAvatarLevelByStars(int stars)
        {
            if (stars >= PCRAvatarLevel.Level6.GetRequiredStars()) return PCRAvatarLevel.Level6;
            if (stars >= PCRAvatarLevel.Level3.GetRequiredStars()) return PCRAvatarLevel.Level3;
            return PCRAvatarLevel.Level1;
        }

        public static string GetAvatarName(string ID, PCRAvatarLevel avatarLevel)
        {
            return $"icon_unit_{ID}{avatarLevel.GetRequiredStars()}1.png";
        }

        public static int GetDefaultStars(string ID)
        {
            return 3;
        }

        private ConfigMgr configMgr = ConfigMgr.GetInstance();
        private PCRTools pcrTools = PCRTools.GetInstance();

        private PCRUnit()
        {
        }

        public string ID { get; private set; }

        public string Name { get; private set; }

        public int Level { get; private set; } = 1;

        public int Rank { get; private set; } = 1;

        public int Stars { get; private set; }

        public PCRAvatarLevel AvatarLevel
        {
            get { return GetAvatarLevelByStars(Stars); }
        }

        public string GetAvatarName(PCRAvatarLevel avatarLevel)
        {
            return GetAvatarName(ID, avatarLevel);
        }

        public string GetAvatarPath(PCRAvatarLevel avatarLevel)
        {
            var avatarName = GetAvatarName(avatarLevel);
            var path = pcrTools.ChooseFilePath("Img/unit", null, avatarName);
            return path;
        }

        public Mat GetAvatar(PCRAvatarLevel avatarLevel)
        {
            var path = GetAvatarPath(avatarLevel);
            var mat = OpenCvExtension.ReadMatFromFile(path);
            return mat;
        }

        public Mat GetResizedAvatar()
        {
            return GetResizedAvatar(AvatarLevel);
        }

        public Mat GetResizedAvatar(PCRAvatarLevel avatarLevel)
        {
            var avatarTemplateSize = configMgr.UnitAvatarTemplateSize;
            return GetResizedAvatar(avatarLevel, avatarTemplateSize.Width);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarLevel"></param>
        /// <param name="templateWidth">1280*640像素下的图标大小</param>
        /// <returns></returns>
        public Mat GetResizedAvatar(PCRAvatarLevel avatarLevel, int templateWidth)
        {
            var avatarResSize = configMgr.UnitAvatarResourceSize;

            var viewportSize = pcrTools.GetViewportSize();
            var viewportTemplateSize = configMgr.MumuViewportTemplateSize;

            var viewportScale = 1.0 * viewportSize.Width / viewportTemplateSize.Width;
            var avatarScale = 1.0 * templateWidth / avatarResSize.Width * viewportScale;

            var mat = GetAvatar(avatarLevel);
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
