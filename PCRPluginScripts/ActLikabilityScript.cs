using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCRBattleRecorder.Script
{
    public class ActLikabilityScript : ScriptBase
    {
        private LogTools logTools = LogTools.GetInstance();
        private MumuTools mumuTools = MumuTools.GetInstance();

        public override string Description
        {
            get { return Trans.T("活动关卡信赖度"); }
        }

        public override string Name
        {
            get { return "ActLikabilityScript"; }
        }

        public override bool CanKeepOnWhenException { get; } = true;

        public override void OnStart(Mat viewportMat, RECT viewportRect)
        {
            //ClickTab(viewportRect, PCRTab.Battle);
            //Thread.Sleep(2000);
            //mumuTools.DoClick(ACT_ENTRANCE_KEY);
        }

        public override void Tick(Mat viewportMat, RECT viewportRect)
        {
            if (CanMatchTemplate(viewportMat, viewportRect, DATA_DOWNLOAD_TITLE_MKEY))
            {
                logTools.Debug("ActLikabilityScript", "DATA_DOWNLOAD");
                mumuTools.DoClick(DOWNLOAD_WITHOUT_VOICE_KEY);
            }
            else if (CanMatchTemplate(viewportMat, viewportRect, LIKABILITY_TITLE_MKEY))
            {
                logTools.Debug("ActLikabilityScript", "LIKABILITY_TITLE");
                if (TryClickListItemNewTag(viewportMat, viewportRect))
                {
                    logTools.Debug("ActLikabilityScript", "TryClickListItemNewTag");
                }
                else
                {
                    logTools.Debug("ActLikabilityScript", "DragDownList");
                    DragDownList();
                }
            }
            else if (CanMatchTemplate(viewportMat, viewportRect, LIKABILITY_PREVIEW_TITLE_MKEY))
            {
                logTools.Debug("ActLikabilityScript", "LIKABILITY_PREVIEW_TITLE");
                if (TryClickTemplateRect(viewportMat, viewportRect, LIKABILITY_ITEM_NEW_TAG_MKEY))
                {
                    logTools.Debug("ActLikabilityScript", "LIKABILITY_ITEM_NEW_TAG");
                }
                else
                {
                    ClickBack();
                }
            }
            else
            {
                mumuTools.DoClick(CHOICE_2_KEY);
            }
            
        }
    }
}
