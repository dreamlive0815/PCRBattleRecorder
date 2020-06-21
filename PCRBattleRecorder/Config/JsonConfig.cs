using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder.Config
{
    class JsonConfig
    {
        private string jsonPath;

        public JsonConfig(string jsonPath)
        {
            LoadFromFile(jsonPath);
        }

        private void LoadFromFile(string jsonPath)
        {

        }
    }
}
