
using System;
using System.Collections.Generic;
using System.Drawing;
using PCRBattleRecorder;

namespace PCRBattleRecorder.Script
{
    public abstract class ScriptBase
    {

        /// <summary>
        /// 单位：毫秒
        /// </summary>
        public virtual int Interval { get; set; } = 2000;

        public virtual bool CanKeepOnWhenException { get; } = false;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract void OnStart(Bitmap viewportCapture, RECT viewportRect);

        public abstract void Tick(Bitmap viewportCapture, RECT viewportRect);
    }
}
