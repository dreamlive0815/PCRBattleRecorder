using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Threading;

namespace PCRBattleRecorder.Script
{
    public class StoryScript : ScriptBase
    {
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();

        public override string Description
        {
            get { return Trans.T("剧情"); }
        }

        public override string Name
        {
            get { return "StoryScript"; }
        }

        public override bool CanKeepOnWhenException { get; } = true;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            ClickTab(viewportRect, PCRTab.Story);
        }

        private int dragTimes = 0;

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            if (TryClickTemplateRect(viewportMat, viewportRect, SKIP_LABEL_MKEY))
            {
                logTools.Debug("StoryScript", "SKIP_LABEL");
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, SKIP_TAG_MKEY))
            {
                logTools.Debug("StoryScript", "SKIP_TAG");
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, MENU_TAG_MKEY))
            {
                logTools.Debug("StoryScript", "MENU_TAG");
            }
            else if (CanMatchTemplate(viewportMat, viewportRect, DATA_DOWNLOAD_TITLE_MKEY))
            {
                logTools.Debug("StoryScript", "DATA_DOWNLOAD");
                mumuTools.DoClick(DOWNLOAD_WITHOUT_VOICE_KEY);
            }
            else if (TryClickTemplateRect(viewportMat, viewportRect, STORY_ITEM_NEW_TAG_MKEY))
            {
                logTools.Debug("StoryScript", "STORY_ITEM_NEW_TAG");
            }
            else if (TryClickListItemNewTag(viewportMat, viewportRect))
            {
                logTools.Debug("StoryScript", "TryClickListItemNewTag");
            }
            else
            {
                if (!CanMatchTemplate(viewportMat, viewportRect, STORY_GUIDE_TAG_MKEY) || dragTimes > 5)
                {
                    ClickBack();
                    dragTimes = 0;
                }
                else
                {
                    mumuTools.DoClick(CHOICE_2_KEY);
                    Thread.Sleep(500);
                    DragDownList();
                    dragTimes++;
                }                
            }
        }
    }
}
